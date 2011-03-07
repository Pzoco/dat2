/* -*- mode: C++; c-file-style: "stroustrup"; c-basic-offset: 4; indent-tabs-mode: nil; -*- */

// Compile like this: gcc -O2 -Wall bench.c -o bench

#include <stdio.h>
#include <stdlib.h>
#include <sys/time.h>

#define MIN_POWER 10
#define MAX_POWER 22

// One cache line = 4 words = 16 bytes.
#define LINE_WIDTH 16

int main(int argc, char *argv[])
{
    int i,j,k,c,dummy;
    struct timeval time1, time2;
    double delay;

    for(i = MIN_POWER; i <= MAX_POWER; ++i)
    {
        size_t size = 1 << i;
        char *mem = (char*) malloc(size);

        if (!mem)
        {
            fprintf(stderr, "Out of memory!\n");
            return 1;
        }

        for (j = 0; j < size; ++j)
        {
            mem[j] = (char) j;
        }

        gettimeofday(&time1, NULL);

        dummy = 0;
        for(k = 0; k < 1024; ++k)
        {
            for(c = 0; c < LINE_WIDTH; ++c)
            {
                for(j = 0; j < size/LINE_WIDTH; ++j)
                {
                    // Strided access.
                    dummy += mem[j*LINE_WIDTH+c];
                }
            }
        }

        gettimeofday(&time2, NULL);
        delay =
            ((time2.tv_usec - time1.tv_usec) / 1000000.0) +
            (time2.tv_sec - time1.tv_sec);
        
        printf("%d kB\t%g kB/s\t(%d)\n", size >> 10, size / delay, dummy);
        
        free(mem);
    }
    return 0;
}
