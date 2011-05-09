// -*- mode: C++; c-file-style: "stroustrup"; c-basic-offset: 4; indent-tabs-mode: nil; -*-

#include <mpi.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

void test(int n)
{
    int rank, size, i;
    MPI_Status status;

	
	MPI_Comm_size(MPI_COMM_WORLD, &size);    
	MPI_Comm_rank(MPI_COMM_WORLD, &rank);


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
		int x = (rank+1)%size;
		int y = (rank-1+size)%size;
		MPI_Send(&send_data[i], 1, MPI_INT, x, 0, MPI_COMM_WORLD);
		printf("I'm now printing the size %d and rank %d \n", size, rank);
		MPI_Recv(&recv_data[i], 1, MPI_INT, y, 0, MPI_COMM_WORLD, &status);		
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
	//printf("Du har skrevet %d \n", n);
	MPI_Init(&argc, &argv); 
    test(n);

    

    return 0;
}
