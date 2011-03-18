// -*- mode: C++; c-file-style: "stroustrup"; c-basic-offset: 4; indent-tabs-mode: nil; -*-

/* You need to include this header */
#include <pthread.h>
#include <stdio.h>

/* How to start threads. */

typedef void* (*void_f)(void*);
void helloworld (int id)
{
	printf("Thread %d: Hello World!",id);
}

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

int main (int argc, char* argv[])
{
	int i = 0;
	int total = argc;
    pthread_t pth;
	for(i;i<=total;i++)
	{
		start_thread(pth,helloworld,i);
	}
	join_threads(pth,total);
}


