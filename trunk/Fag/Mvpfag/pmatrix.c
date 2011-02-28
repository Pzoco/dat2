/* -*- mode: C++; c-file-style: "stroustrup"; c-basic-offset: 4; indent-tabs-mode: nil; -*- */

// Compile like this: gcc -O2 -Wall pmatrix.c -o pmatrix

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>
#include <unistd.h>
#include <math.h>
#include <assert.h>
#include <time.h>

unsigned long long mem;
double base_time;

/* For testing, you can use a
 * constant value if you wish.
 */
#define RANGE(S) (S)

#define PIVOT 1    // Gaussian elimination with or without pivoting.
#define BLOCK 50   // Size of the blocks for block-matrix multiplication.

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

double* alloc_double(size_t size)
{
    return (double*) safe_malloc(size*sizeof(double));
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
        f(a, b, c, dim);
        fprintf(stdout, "done!\n");
    }
    else
    {
        timer(&t1, &v1);
        f(a, b, c, dim);
        timer(&t2, &v2);

        u = t2.tms_utime - t1.tms_utime;
        s = t2.tms_stime - t1.tms_stime;
        low = v2.tv_usec - v1.tv_usec;
        high = v2.tv_sec - v1.tv_sec;
        r = (low / 1000000.0) + high;
        u /= sysconf(_SC_CLK_TCK);
        s /= sysconf(_SC_CLK_TCK);

        fprintf(stdout,
                "done!\treal: %gs, user: %gs, sys: %gs, idle: %g%%%s\n",
                r, u, s, r < u+s ? 0.0 : 100-(100*(u+s))/r, DEF);
    }

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


void mat_mult1(double *a, double *b, double *c, size_t dim)
{
    size_t i, j, k;
    assert(a != b && a != c);

    for(i= 0;i<n;i++)
	{
		for(j = 0;j<n;j++)
		{
		    double temp = 0;
			for(k = 0;k<n;k++)
			{
                temp+= a[i*n+k]*b[k*n+k];
			}
            c[i*n+j] +=temp;
		}
	}
}


void mat_mult2(const double* a, const int* b, int* c, size_t n)
{
    int i, j, k;              /* You will need at least these. */
    assert(a != c && b != c); /* Check precondition. */

    /* Write here your matrix multiplication. */
    for(k = 0;k<n;k++)
    {
        for(i= 0;i<n;i++)
        {
            double temp = 0;
            for(j = 0;j<n;j++)
            {
                temp+= a[i*n+k]*b[k*n+k];
			}
            c[i*n+j] +=temp;
		}
	}
}


void mat_mult3(double *a, double *b, double *c, size_t dim)
{
    size_t bi, bj, bk, i, j, k, maxi, maxj, maxk;

    /* Block-matrix multiplication. */
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
                fprintf(stderr, "Matrix is not .\n");
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

    timed_call(NULL, "Generating A", gen_mat, a, NULL, NULL, dim);
    timed_call(BLUE, "Inverting", inv_mat, a, b, c, dim);

    timed_call(NULL, "Randomizing", gen_mat, c, NULL, NULL, dim);
    timed_call(BLUE, "Multiplying1", mat_mult1, a, b, c, dim);
    timed_call(NULL, "Checking", check_identity, c, NULL, NULL, dim);

    // Uncomment when you've written mat_mult2.
    timed_call(NULL, "Randomizing", gen_mat, c, NULL, NULL, dim);
    timed_call(BLUE, "Multiplying2", mat_mult2, a, b, c, dim);
    timed_call(NULL, "Checking", check_identity, c, NULL, NULL, dim);

    // Uncomment when you've written mat_mult3.
    //timed_call(NULL, "Randomizing", gen_mat, c, NULL, NULL, dim);
    //timed_call(BLUE, "Multiplying3", mat_mult3, a, b, c, dim);
    //timed_call(NULL, "Checking", check_identity, c, NULL, NULL, dim);

    free(c);
    free(b);
    free(a);
}


int main(int argc, char *argv[])
{
    if (argc < 2)
    {
        fprintf(stderr, "Usage: %s size\n", argv[0]);
        return 1;
    }
    else
    {
        int i = atoi(argv[1]);

        if (i < 1)
        {
            fprintf(stderr, "Minimum dimension is 1.\n");
            return 1;
        }

        mem = 0;
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
