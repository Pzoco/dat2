#include <stdio.h>
#include <mpi.h>

int main(int argc, char *argv[])
{

int rank, size;

MPI_Init(&argc, &argv); // Init
MPI_Comm_size(MPI_COMM_WORLD, &size); // Get the size
MPI_Comm_rank(MPI_COMM_WORLD, &rank); // Get the rank
printf("Process number %d of a total of %d says HELLO WORLD! \n", rank, size); // Hello World
MPI_Finalize();              // Finalize

return 0;

}

