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
#include <semaphore.h>
#include <sys/time.h>
#include <sys/times.h>

unsigned long long mem;
int speedup;
size_t nb_threads;
double base_time;

/* This is used for the random generation of matrices.
 * You can use a constant value if you wish.
 */
#define RANGE(S) (S)

#define PIVOT 1    // Gaussian elimination with or without pivoting.
#define BLOCK 50   // Size of the blocks for block-matrix multiplication.

#define DECOMPOSITION 2 // 1-D or 2-D decomposition for block-matrix multiplication.

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

//COUNT CPUS ON MACINTOSH!!

#include <fcntl.h>
#include <unistd.h>
#include <stdio.h>

size_t count_cores(){

    char os[7];
    char cores_string[3];
    char* cmd = NULL;
    FILE* fp = popen("uname","r");

    if (NULL == fp)
    {
        perror("popen");
        abort();
    }

    fgets(os, 6, fp);
    pclose(fp);

    if(0 == strcmp("Darwi", os))
    {
        cmd = "sysctl hw | grep availcpu | cut -d'=' -f 2";
    }
    else if(0 == strcmp("Linux", os))
    {
        cmd = "cat /proc/stat | grep cpu[0-9][0-9]* | wc -l";
    }
    else
    {
        abort();
    }

    if (NULL == (fp = popen(cmd,"r")))
    {
        perror("popen");
        abort();
    }

    fgets(cores_string, 3, fp);
    pclose(fp);

    return atoi(cores_string);
}
//DONE COUNTING CPUS ON THE AWESOME MACINTOSHSHS!!
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
                  double *a, double *b, double *c, size_t dim)
{
    struct tms t1, t2;
    struct timeval v1, v2;
    double u, s, r = 0;
    int low, high;

    fprintf(stdout, "%s%s...", color ? color : DEF, title);
    fflush(stdout);

    if (!color)
    {
        // Untimed call.
        f(a, b, c, dim);
        fprintf(stdout, "done!\n");
    }
    else
    {
        // Timed call.
        timer(&t1, &v1);
        f(a, b, c, dim);
        timer(&t2, &v2);

        // How long it took.
        u = t2.tms_utime - t1.tms_utime;
        s = t2.tms_stime - t1.tms_stime;
        low = v2.tv_usec - v1.tv_usec;
        high = v2.tv_sec - v1.tv_sec;
        r = (low / 1000000.0) + high;
        u /= sysconf(_SC_CLK_TCK);
        s /= sysconf(_SC_CLK_TCK);

        // Evaluate speed-up.
        if (speedup)
        {
            fprintf(stdout,
                    "done!\treal: %gs, user: %gs, sys: %gs, idle: %g%%, %sS: %g, E: %g, T0: %gs%s\n",
                    r, u, s, // real, user, system time
                    // r*nb_threads = total work done
                    // u+s = total time spent
                    // idling = total work - total time spent (here in
                    // percent of the total work)
                    r*nb_threads < u+s ? 0.0 : 100*(1.0-(u+s)/(r*nb_threads)),
                    RED,  // color code
                    base_time/r,              // S : speedup
                    base_time/(r*nb_threads), // E : efficiency = normalized speedup
                    r*nb_threads-base_time,   // T0: total overhead compared to the serial algorithm base time
                    DEF); // color code
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
void gen_mat(double *a, double *dummy1, double *dummy2, size_t dim)
{
    size_t i,j;
    srand48(dim);
    for(i = 0; i < dim; ++i)
    {
        for(j = 0; j < dim; ++j)
        {
            double z = (drand48() - 0.5)*RANGE(dim);
            a[i*dim+j] = (z < 10*PRECISION && z > -10*PRECISION) ? 0.0 : z;
        }
    }
}


void block_mat_mult(double *a, double *b, double *c, size_t dim)
{
    // Copy your code from assignment 1.
}


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
    size_t dim, id;
} matmult_data_t;


void* job_block_mat_mult(matmult_data_t *data)
{
    // Copy your code from assignment 2.
}


void pthread_block_mat_mult(double *a, double *b, double *c, size_t dim)
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
        data[i].dim = dim;
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
    //Copies the memory
    memcpy(a, input, n*n*sizeof(double));
    //Sets the memory
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


// A structure that may help you.

typedef struct
{
    double *input, *a, *b;
    size_t n, id, *index;
    sem_t *sync1, *sync2;
} invmat_data_t;

// Wrapper function for sem_wait.

void sync_wait(sem_t *sync)
{
    if (sem_wait(sync))
    {
        perror("sem_wait");
        abort();
    }
}

// Wrapper function for sem_post.

void sync_post(sem_t *sync)
{
    if (sem_post(sync))
    {
        perror("sem_post");
        abort();
    }
}

/* The barrier. */

/* Implement a barrier here.
   1) Declare your variables.
   2) Write the init function to initialize the variables of your barrier.
   3) Write the barrier function.
*/


void init_barrier ((*b), int threadcount)
{
	pthread_mutex_lock(&(b -> count_lock));
	b -> count++;
	if(b -> count == threadcount)  
	{ 
		b -> count = 0;
		pthread_cond_broadcast(&(b -> ok_to_proceed));
	}
	else 
	{
		pthread_cond_wait(&(b-> ok_to_proceed), &b->count_lock));
	}
	pthread_mutex_unlock(&(b->count_lock));
}


void* job_inv_mat(invmat_data_t *data)
{
    // Typically you need to read your data.

    size_t n = data->n;
    double *a = data->a;
    double *b = data->b;
    size_t *index = data->index;
    int i,j,k;
    double x;

    // And here you go.

}


void pthread_inv_mat(double *input, double *a, double *b, size_t n)
{
    invmat_data_t data[nb_threads];     // Data for the threads.
    pthread_t threads[nb_threads];      // The threads.
    size_t index[n];                    // Shared index map.
    sem_t sync1[nb_threads];            // Useful array of semaphores.
    sem_t sync2;                        // Useful semaphore.
    size_t i;                           // Useful var.
    speedup = 1;                        // Inialize the speed-up evaluation.

    // Initialize your semaphores.
	int sem_init(sem_t *sem, int pshared, unsigned int value);

    // Initialize your barrier.
	init_barrier(*threads[nb_threads], nb_threads);
    // Initialize your data.
    for(i = 0; i < nb_threads; ++i)
    {
        data[i].a = a;
        data[i].b = b;
        data[i].c = c;
        data[i].dim = dim;
        data[i].id = i;
    }
    // Start your threads.
    for(i = 1; i < nb_threads; ++i)
    {
    start_thread(&threads[nb_threads],pthread_inv_mat,&data[i]);
    }
    // Join your threads.
    join_threads(threads, nb_threads);
    // Destroy your semaphores.
    int sem_destroy(sem_t *sem);
}


void check_identity(double *a, double *dummy1, double *dummy2, size_t dim)
{
    size_t i,j;
    double error = 0.0;
    for(i = 0; i < dim; ++i)
    {
        for(j = 0; j < dim; ++j)
        {
            double r = i == j ? a[i*dim+j] - 1.0 : a[i*dim+j];
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


void test(size_t dim)
{
    double *a = alloc_double(dim*dim);
    double *b = alloc_double(dim*dim);
    double *c = alloc_double(dim*dim);
    double tinv, tmul;

    timed_call(NULL, "Generating A", gen_mat, a, NULL, NULL, dim);
    // Save time.
    tinv = timed_call(BLUE, "Inverting", inv_mat, a, b, c, dim);
    timed_call(NULL, "Randomizing", gen_mat, c, NULL, NULL, dim);
    // Save time.
    tmul = timed_call(BLUE, "MultiplyingB", block_mat_mult, a, b, c, dim);
    timed_call(NULL, "Checking", check_identity, c, NULL, NULL, dim);
    timed_call(NULL, "Randomizing", gen_mat, c, NULL, NULL, dim);

    fprintf(stdout, "Using %u threads.\n", nb_threads);

    timed_call(NULL, "Randomizing", gen_mat, c, NULL, NULL, dim);
    base_time = tmul; // Base time to compare with.
    timed_call(BLUE, "PMultiplyingB", pthread_block_mat_mult, a, b, c, dim);
    timed_call(NULL, "Checking", check_identity, c, NULL, NULL, dim);
    timed_call(NULL, "Randomizing", gen_mat, b, NULL, NULL, dim);
    timed_call(NULL, "Randomizing", gen_mat, c, NULL, NULL, dim);
    base_time = tinv; // Base time to compare with.
    timed_call(BLUE, "PInverting", pthread_inv_mat, a, b, c, dim);
    timed_call(NULL, "Randomizing", gen_mat, c, NULL, NULL, dim);
    base_time = tmul; // Base time to compare with.
    timed_call(BLUE, "PMultiplyingB", pthread_block_mat_mult, a, b, c, dim);
    timed_call(NULL, "Checking", check_identity, c, NULL, NULL, dim);

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
        nb_threads = n < 1 ? count_cores() : n;
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
