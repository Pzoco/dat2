    // Compute image.
    // This is the loop to parallelize.

    for(x = 0; x < width; ++x)
    {
        for(y = 0; y < height; ++y)
        {
            compute(x, y, &data);
        }
    }
	
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

size_t cpus = count_cpus();
int i;
pthread_t pths[cpus];

for (i=0;i<cpus;i++)
{
	start_thread(&pths[i], lol, i);
}
join_threads(pths,cpus);

void lol(size_t input)
{
	for (x=0;x<width;x++)
	{
		for (y=(input/cpus);y<(input+1/cpus)*height; y++)
		{
			compute(x, y, &data);
		}
	}
}