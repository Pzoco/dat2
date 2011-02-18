/* -*- mode: C++; c-file-style: "stroustrup"; c-basic-offset: 4; indent-tabs-mode: nil; -*- */

// Compile like this: gcc -O2 -Wall matrix_fibo.c -o matrix_fibo

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <assert.h>

/* The matrices 'a' and 'b' are inputs.
 * The matrix 'c' is the output.
 * The dimension is 'dim'.
 * Assume: 'c' is neither 'a' or 'b'.
 */
void matrix_mult(const int* a, const int* b, int* c, size_t dim)
{
    int i, j, k;              /* You will need at least these. */
    assert(a != c && b != c); /* Check precondition. */

    /* Write your multiplication here. */
}

/* The idea of this algorithm for computing
 * Fibonacci numbers is explained here if
 * you are interested:
 http://www.cs.aau.dk/~adavid/teaching/AA/AA1-06/Xtra-PoweringNumbers+Fibonacci.pdf
*/

int log_fibo(int n)
{
    int a[4] = { 1,1,1,0 };
    int r[4] = { 1,0,0,1 };
    int tmp[4];

    for(; n > 0; n >>= 1)
    {
        if (n & 1)
        {
            /* r = r * a */
            memcpy(tmp, r, sizeof(r));
            matrix_mult(tmp, a, r, 2);
        }
        /* a = a * a */
        memcpy(tmp, a, sizeof(a));
        matrix_mult(tmp, tmp, a, 2);
    }

    return r[1];
}

/* Classic Fibonacci numbers. */

int linear_fibo(int n)
{
    int a = 0;
    int b = 1;

    for(; n > 0; --n)
    {
        int old_b = b;
        b += a;
        a = old_b;
    }

    return a;
}

void test_fibo(int n)
{
    int f1 = linear_fibo(n);
    int f2 = log_fibo(n);

    if (f1 != f2)
    {
        fprintf(stderr, "Error: fibo(%d) = %d, expected %d\n", n, f2, f1);
    }
    else
    {
        printf("OK: fibo(%d) = %d\n", n, f1);
    }
}

int main(int argc, char *argv[])
{
    int n;

    /* Note that you will get overflows after 46. */

    for(n = 0; n <= 46; ++n)
    {
        test_fibo(n);
    }

    return 0;
}
