// -*- mode: C++; c-file-style: "stroustrup"; c-basic-offset: 4; indent-tabs-mode: nil; -*-

/* You need to include this header */
#include <pthread.h>

/* How to start threads. */

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

/* How to join threads. This will join on an array
 * of n threads.
 */

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

/* If you want to count cpus on a Linux machine
 * you can use this function with the appropriate
 * headers.
 */

#include <stdio.h>
#include <string.h>

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


/* If you have a Mac you can use this code provided
 * by Claus Thrane.
 */

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
