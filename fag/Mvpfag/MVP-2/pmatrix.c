/* -*- mode: C++; c-file-style: "stroustrup"; c-basic-offset: 4; indent-tabs-mode: nil; -*- */

/* Compile: gcc -o pmatrix -Wall -O2 pmatrix.c -lpthread
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>
#include <unistd.h>
#include <math.h>
#include <pthread.h>
#include <sys/time.h>
#include <sys/times.h>

unsigned long long mem;
int speedup;
size_t nb_threads;
double base_time;
size_t turn;

/* For testing, you can use a
 * constant value if you wish.
 */
#define RANGE(S) (S)

#define PIVOT 1    // Gaussian elimination with or without pivoting.
#define BLOCK 40   // Size of the blocks for block-matrix multiplication.

#define DECOMPOSITION 1 // 1-D or 2-D decomposition for block-matrix multiplication.

#define BLUE "\033[1;34m"
#define RED  "\033[1;31m"
#define DEF  "\033[0;0m"

void* safe_malloc(size_t size)
{
    void* data = malloc(size);
    if (!data)
    {
        fprintf(stderr, "Could not allocate %u bytes!\n", size);
        abort();
    }
    mem += size;
    return data;
}


typedef void* (*void_f)(void*);

void start_thread(pthread_t *thread, void_f job, void* arg)
{
    static size_t id = 0;
    if (pthread_create(thread, NULL, job, arg))
    {
        fprintf(stderr, "Could not create thread %d, aborting.\n", id);
        abort();
    }
    id++;
}


void join_threads(pthread_t *threads, size_t n)
{
    size_t i;
    for(i = 0; i < n; ++i)
    {
        if (pthread_join(threads[i], NULL))
        {
            fprintf(stderr, "Could not wait for thread %d.\n", i);
            abort();
        }
    }
}


double* alloc_double(size_t size)
{
    return (double*) safe_malloc(size*sizeof(double));
}


size_t count_cpus()
{
    char s[256];
    size_t count = 0;
    FILE *f = fopen("/proc/stat", "r");
    if (!f)
    {
        fprintf(stderr, "/proc/stat not found!\n");
        abort();
    }
    while(fgets(s,sizeof(s),f) && !strncmp(s,"cpu",3))
    {
        count++;
    }
    fclose(f);
    return count > 1 ? count - 1 : count;
}


typedef void (*mat_f)(double*, double*, double*, size_t);


void timer(struct tms *t, struct timeval *tv)
{
    if (times(t) == (clock_t) -1)
    {
        perror("times");
    }
    if (gettimeofday(tv, NULL) < 0)
    {
        perror("gettimeofday");
    }
}


double timed_call(const char* color, const char *title, mat_f f,
                  double *a, double *b, double *c, size_t n)
{
    struct tms t1, t2;
    struct timeval v1, v2;
    double u, s, r = 0;
    int low, high;

    fprintf(stdout, "%s%s...", color ? color : DEF, title);
    fflush(stdout);

    if (!color)
    {
        f(a, b, c, n);
        fprintf(stdout, "done!\n");
    }
    else
    {
        timer(&t1, &v1);
        f(a, b, c, n);
        timer(&t2, &v2);

        u = t2.tms_utime - t1.tms_utime;
        s = t2.tms_stime - t1.tms_stime;
        low = v2.tv_usec - v1.tv_usec;
        high = v2.tv_sec - v1.tv_sec;
        r = (low / 1000000.0) + high;
        u /= sysconf(_SC_CLK_TCK);
        s /= sysconf(_SC_CLK_TCK);

        if (speedup)
        {
            fprintf(stdout,
                    "done!\treal: %gs, user: %gs, sys: %gs, idle: %g%%, %sS: %g, E: %g, T0: %gs%s\n",
                    r, u, s,
                    r*nb_threads < u+s ? 0.0 : 100*(1.0-(u+s)/(r*nb_threads)),
                    RED,
                    base_time/r,              // S
                    base_time/(r*nb_threads), // E
                    r*nb_threads-base_time,   // T0
                    DEF);
        }
        else
        {
            fprintf(stdout,
                    "done!\treal: %gs, user: %gs, sys: %gs, idle: %g%%%s\n",
                    r, u, s, r < u+s ? 0.0 : 100-(100*(u+s))/r, DEF);
        }
    }

    speedup = 0;
    return r;
}


// Precision for checking identity.
#define PRECISION 1e-6

/* Random generation, but always the same for
 * a given size for fair comparison.
 */
void gen_mat(double *a, double *dummy1, double *dummy2, size_t n)
{
    size_t i,j;
    srand48(n);
    for(i = 0; i < n; ++i)
    {
        for(j = 0; j < n; ++j)
        {
            double z = (drand48() - 0.5)*RANGE(n);
            a[i*n+j] = (z < 10*PRECISION && z > -10*PRECISION) ? 0.0 : z;
        }
    }
}

size_t min(size_t a, size_t b)
{
    if(a > b) { return b;}
    else{return a;}
}

void block_mat_mult(double *a, double *b, double *c, size_t n)
{
    size_t bi, bj, bk, i, j, k, maxi, maxj, maxk;

    for(bi = 0;bi<n;bi+=BLOCK)
    {
        maxi = min(bi+BLOCK,n);
        for(bj = 0;bj<n;bj+=BLOCK)
        {
            maxj = min(bj+BLOCK,n);
            for(bk = 0;bk < n;bk+=BLOCK)
            {
                maxk = min(bk+BLOCK,n);
                for(i = 0;i<maxi;i++)
                {
                    for(j= 0;j<maxj;j++)
                    {
                        double temp = 0;
                        for(k = 0;k<maxk;k++)
                        {
                        temp+= a[i*n+k]*b[k*n+j];
                        }
                        c[i*n+j] +=temp;
                    }
                }
            }
        }
    }
}


// This can be useful.

#if DECOMPOSITION == 1
#define DECOMPOSE_BY_ROW if (turn++ % nb_threads == data->id)
#define DECOMPOSE_BY_BLOCK
#else
#define DECOMPOSE_BY_ROW
#define DECOMPOSE_BY_BLOCK if (turn++ % nb_threads == data->id)
#endif


typedef struct
{
    double *a, *b, *c;
    size_t n, id;
} matmult_data_t;


void* job_block_mat_mult(matmult_data_t *data)
{
    double *a = data->a;
    double *b = data->b;
    double *c = data->c;
    size_t n = data->n;
    size_t id = data->id;
    size_t bi, bj, bk, i, j, k, maxi, maxj, maxk;

    for(bi = 0;bi<n;bi+=BLOCK)DECOMPOSE_BY_ROW
    {
        maxi = min(bi+BLOCK,n);
        for(bj = 0;bj<n;bj+=BLOCK)DECOMPOSE_BY_BLOCK
        {
            maxj = min(bj+BLOCK,n);
            for(bk = 0;bk < n;bk+=BLOCK)
            {
                maxk = min(bk+BLOCK,n);
                for(i = 0;i<maxi;i++)
                {
                    for(j= 0;j<maxj;j++)
                    {
                        double temp = 0;
                        for(k = 0;k<maxk;k++)
                        {
                        temp+= a[i*n+k]*b[k*n+j];
                        }
                        c[i*n+j] +=temp;
                    }
                }
            }
        }
    }
}


void pthread_block_mat_mult(double *a, double *b, double *c, size_t n)
{
    matmult_data_t data[nb_threads];
    pthread_t threads[nb_threads];
    size_t i;
    speedup = 1;
    for(i = 0; i < nb_threads; ++i)
    {
        data[i].a = a;
        data[i].b = b;
        data[i].c = c;
        data[i].n = n;
        data[i].id = i;
    }
    for(i = 1; i < nb_threads; ++i)
    {
        start_thread(&threads[i], (void_f) job_block_mat_mult, &data[i]);
    }
    job_block_mat_mult(&data[0]);
    join_threads(threads+1, nb_threads-1);
}


/* Check that a divider is not too close to 0. */
static void check_divider(double x)
{
    if (fabs(x) < PRECISION)
    {
        fprintf(stderr, "Matrix non-invertible or computationally unstable.\n");
        abort();
    }
}

/* Read a re-arranged matrix. */
#define A(I,J) a[index[I]*n+(J)]
#define B(I,J) b[index[I]*n+(J)]


/* a = invert(input), using b as temporary matrix
 */
void inv_mat(double *input, double *a, double *b, size_t n)
{
    int i,j,k;
    double x;
    size_t index[n];

    // Init a, b, and index.
    memcpy(a, input, n*n*sizeof(double));
    memset(b, 0, n*n*sizeof(double));
    for(i = 0; i < n; ++i)
    {
        index[i] = i;   // Identity mapping.
        b[i*n+i] = 1.0; // Identity matrix.
    }

    // Gaussian elimination.
    for(k = 0; k < n; ++k)
    {

#if PIVOT
        // Find pivot.
        x = fabs(A(k,k));
        j = k;
        for(i = k+1; i < n; ++i)
        {
            double y = fabs(A(i,k));
            if (y > x)
            {
                j = i;
                x = y;
            }
        }

        // Swap j <-> k.
        i = index[j];
        index[j] = index[k];
        index[k] = i;
#endif
        // Division step.
        check_divider(x = A(k,k));
        A(k,k) = 1.0;
        for(j = k+1; j < n; ++j) A(k,j) /= x;
        for(j = 0  ; j < n; ++j) B(k,j) /= x;

        // Elimination step.
        for(i = k+1; i < n; ++i)
        {
            for(j = k+1; j < n; ++j) A(i,j) -= A(i,k)*A(k,j);
            for(j = 0  ; j < n; ++j) B(i,j) -= A(i,k)*B(k,j);
            A(i,k) = 0.0;
        }
    }

    // Back-substitution.
    for(k = n-1; k > 0; --k)
    {
        for(i = k-1; i >= 0; --i)
        {
            for(j = 0; j < n; ++j) B(i,j) -= A(i,k)*B(k,j);
            //A(i,k) = 0.0; -- implicit and not used later.
        }
    }

    // Result.
    for(i = 0; i < n; ++i)
    {
        for(j = 0; j < n; ++j)
        {
            a[i*n+j] = B(i,j);
        }
    }
}


void check_identity(double *a, double *dummy1, double *dummy2, size_t n)
{
    size_t i,j;
    double error = 0.0;
    for(i = 0; i < n; ++i)
    {
        for(j = 0; j < n; ++j)
        {
            double r = i == j ? a[i*n+j] - 1.0 : a[i*n+j];
            if (r < -PRECISION || r > PRECISION)
            {
                fprintf(stderr, "Matrix is not identity.\n");
                abort();
            }
            error += fabs(r);
        }
    }
    /* Smaller error = better precision. */
    fprintf(stdout, "\033[K\rError=%g ", error);
}


void test(size_t n)
{
    double *a = alloc_double(n*n);
    double *b = alloc_double(n*n);
    double *c = alloc_double(n*n);
    double tmul;

    timed_call(NULL, "Generating A", gen_mat, a, NULL, NULL, n);
    timed_call(BLUE, "Inverting", inv_mat, a, b, c, n);

    timed_call(NULL, "Randomizing", gen_mat, c, NULL, NULL, n);
    tmul = timed_call(BLUE, "MultiplyingB", block_mat_mult, a, b, c, n);
    // timed_call(NULL, "Checking", check_identity, c, NULL, NULL, n);

    timed_call(NULL, "Randomizing", gen_mat, c, NULL, NULL, n);

    fprintf(stdout, "Using %u threads.\n", nb_threads);

    timed_call(NULL, "Randomizing", gen_mat, c, NULL, NULL, n);
    base_time = tmul;
    timed_call(BLUE, "PMultiplyingB", pthread_block_mat_mult, a, b, c, n);
    // timed_call(NULL, "Checking", check_identity, c, NULL, NULL, n);

    free(c);
    free(b);
    free(a);
}


int main(int argc, char *argv[])
{
    if (argc < 2)
    {
        fprintf(stderr, "Usage: %s size [threads]\n", argv[0]);
        return 1;
    }
    else
    {
        int i = atoi(argv[1]);
        int n = argc < 3 ? 0 : atoi(argv[2]);

        if (i < 1)
        {
            fprintf(stderr, "Minimum dimension is 1.\n");
            return 1;
        }

        mem = 0;
        speedup = 0;
        nb_threads = n < 1 ? count_cpus() : n;
        fprintf(stdout, "Threads: %u\n", nb_threads);
        test((size_t) i);

        if (mem < 1 << 10)
        {
            fprintf(stdout, "Used %llu bytes.\n", mem);
        }
        else if (mem < 1 << 20)
        {
            fprintf(stdout, "Used %llu kB.\n", mem >> 10);
        }
        else
        {
            fprintf(stdout, "Used %llu MB.\n", mem >> 20);
        }

        return 0;
    }
}
