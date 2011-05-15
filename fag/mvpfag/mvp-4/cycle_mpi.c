// -*- mode: C++; c-file-style: "stroustrup"; c-basic-offset: 4; indent-tabs-mode: nil; -*-

#include <mpi.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

void test(int n)
{
    int rank, size, i;
    MPI_Status status;
	MPI_Comm_rank(MPI_COMM_WORLD, &rank);
	MPI_Comm_size(MPI_COMM_WORLD, &size); 
    int *send_data = (int*) malloc(n*sizeof(int));
    int *recv_data = (int*) malloc(n*sizeof(int));

    if (!send_data || !recv_data)
    {
        fprintf(stderr, "Out of memory!\n");
        abort();
    }

    // rank is meant to be the rank of this process.
    
    for(i = 0; i < n; ++i)
    {
        send_data[i] = rank;
    }

    // size is meant to be the size of the communicator.

    for(i = 0; i < size; ++i)
    {
        // Send & receive.

		int destination = (rank+1)%size;
		int source = rank%size;	
		MPI_ISend(send_data, 1, MPI_INT, destination, 123, MPI_COMM_WORLD);
		MPI_IRecv(recv_data, 1, MPI_INT, source, 123, MPI_COMM_WORLD, &status);
        // You'll need that at some point.
        memcpy(send_data, recv_data, n*sizeof(int));
    }
	MPI_Finalize();  
}


int main(int argc, char *argv[])
{
    int n;

    

    if (argc < 2)
    {
        fprintf(stderr, "Usage: %s size\n", argv[0]);
        abort();
    }

    n = atoi(argv[1]);

    if (n < 1)
    {
        fprintf(stderr, "Minimum value is 1.\n");
        abort();
    }
	MPI_Init(&argc, &argv); 
    test(n);

    

    return 0;
}
