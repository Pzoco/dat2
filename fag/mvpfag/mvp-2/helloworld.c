// -*- mode: C++; c-file-style: "stroustrup"; c-basic-offset: 4; indent-tabs-mode: nil; -*-

/* You need to include this header */
#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>

/* How to start threads. */

typedef void* (*void_f)(void*);

typedef struct whatever
{
	int id;
	int total;
} storetype;

void helloworld (void *input)
{
	storetype *t=input;
	//To måder at hente en værdi fra structen t.
	//t->id 
	//(*t).id
	int id=(*t).id+1;
	int total=t->total;
	printf("Thread %d / %d: Hello World! \n",id,total);
	return NULL;
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
	int i;
	int total = atoi(argv[1]);
	pthread_t pths[total];
	storetype storage[total];
	for(i=0;i<total;i++)
	{
		storage[i].id=i;
		storage[i].total=total;
		start_thread(&pths[i],helloworld,&storage[i]);
	}
	join_threads(pths,total);
	exit(EXIT_SUCCESS);
}
