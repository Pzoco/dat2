// -*- mode: C++; c-file-style: "stroustrup"; c-basic-offset: 4; indent-tabs-mode: nil; -*-

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <unistd.h>
#include <stddef.h>
#include <pthread.h>

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

typedef void* (*void_f)(void*);

/*****************************************************
 * BMP image handling - you don't need to read this. *
 *****************************************************/

// BMP file & image headers merged.

typedef struct
{
    // Should begin with "BM" but because of data alignment, outside the struct.
    unsigned int bfSize;         // specifies the size of the file in bytes.
    unsigned int bfReserved;     // must always be set to zero.
    unsigned int bfOffbits;      // specifies the offset from the beginning of the file to the bitmap data (1078).
    unsigned int biSize;         // specifies the size of the BITMAPINFOHEADER structure, in bytes.
    int biWidth;                 // specifies the width of the image, in pixels.
    int biHeight;                // specifies the height of the image, in pixels.
    unsigned short biPlanes;     // specifies the number of planes of the target device, must be set to 1.
    unsigned short biBitCount;   // specifies the number of bits per pixel.
    unsigned int biCompression;  // specifies the type of compression, usually set to zero (no compression).
    unsigned int biSizeImage;    // specifies the size of the image data, in bytes. If there is no compression,
                                 // it is valid to set this member to zero.
    unsigned int biXPelsPerMeter;// specifies the the horizontal pixels per meter on the designated targer device,
                                 // usually set to zero.
    unsigned int biYPelsPerMeter;// specifies the the vertical pixels per meter on the designated targer device,
                                 // usually set to zero.
    unsigned int biClrUsed;      // specifies the number of colors used in the bitmap, if set to zero the
                                 // number of colors is calculated using the biBitCount member.
    unsigned int biClrImportant; // specifies the number of color that are 'important' for the bitmap,
                                 // if set to zero, all colors are important.
} bmp_header_t;

typedef struct
{
    unsigned char b,g,r,a;
} rgbquad_t;

// Image structure for 8 bits bitmap.

typedef struct
{
    bmp_header_t header;
    rgbquad_t colormap[256];
    unsigned char bitmap[];
} bmp8_t;

// Create a BMP image with a colormap (size 256).

bmp8_t* create_bmp8(const char* mapfilename, int width, int height)
{
    char buffer[256];
    size_t size_img = (width+3-(width+3)%4)*height; // Need to pad lines.
    size_t size = sizeof(bmp8_t) + size_img;
    bmp8_t *bmp = (bmp8_t*) malloc(size);
    size_t i;
    unsigned int r, g, b;
    FILE *file;
    if (bmp == NULL)
    {
        fprintf(stderr, "Out of memory.\n");
        abort();
    }
    bmp->header.bfSize = size + 2; // +2: BM
    bmp->header.bfReserved = 0;
    bmp->header.bfOffbits = sizeof(bmp8_t) + 2; // +2: BM
    bmp->header.biSize = 40;
    bmp->header.biWidth = width;
    bmp->header.biHeight = height;
    bmp->header.biPlanes = 1;
    bmp->header.biBitCount = 8;
    bmp->header.biCompression = 0;
    bmp->header.biSizeImage = size_img;
    bmp->header.biXPelsPerMeter = 0;
    bmp->header.biYPelsPerMeter = 0;
    bmp->header.biClrUsed = 256;
    bmp->header.biClrImportant = 256;
    file = fopen(mapfilename, "r");
    if (file == NULL)
    {
        fprintf(stderr, "Could not open %s, aborting.\n", mapfilename);
        abort();
    }
    for(i = 0; i < 256; ++i)
    {
        if (fgets(buffer, sizeof(buffer), file) == NULL)
        {
            fprintf(stderr, "Error reading the color map file.\n");
            abort();
        }
        if (sscanf(buffer, "%u%u%u", &r, &g, &b) == EOF)
        {
            perror("sscanf");
            abort();
        }
        bmp->colormap[i].b = (unsigned char) b;
        bmp->colormap[i].g = (unsigned char) g;
        bmp->colormap[i].r = (unsigned char) r;
        bmp->colormap[i].a = 0;
    }
    fclose(file);
    fprintf(stdout, "%s read.\n", mapfilename);
    memset(bmp->bitmap, 0, size_img);
    return bmp;
}


/*******************************************************
 * Computation of Mandelbrot's set. You don't need to  *
 * read this, although it is interesting.              *
 * Principle:                                          *
 * Let z and c be complex numbers.                     *
 * Initialize z = 0                                    *
 * Loop: z = z^2 + c                                   *
 * Stop loop when |z| > some constant.                 *
 * The loop iteration number gives the color.          *
 *******************************************************/

typedef struct
{
    bmp8_t *bmp;   // BMP image.
    size_t width;  // Width in pixels of the area to compute.
    size_t height; // Height in pixels of the area to compute.
    size_t padded_width; // Padded width in the BMP image.
    double r0,i0;  // Lower left (complex) point of the area to compute.
    double dr;     // Width along the real axis of the area to compute (delta).
    size_t max;    // Maximal number of iterations.
} job_data_t;


// Compute the color of a pixel.
// Keep aspect ratio w.r.t. horizontal axis so don't need height.

void compute(size_t x, size_t y, const job_data_t *data)
{
    double cr = data->r0 + (x/(double)data->width)*data->dr;
    double ci = data->i0 + (y/(double)data->width)*data->dr;
    double r = 0;
    double i = 0;
    size_t k, n = data->max;
    for(k = 0; k < n; ++k)
    {
        double new_r = r*r - i*i + cr;
        double new_i = 2*r*i + ci;
        r = new_r;
        i = new_i;
        if (r*r+i*i > 10.0)
        {
            break;
        }
    }
    data->bmp->bitmap[x + y*data->padded_width]= k % 256;
}


/**************************************************
 * Generation of the image.                       *
 * Go through every pixel and compute its color.  *
 **************************************************/

void generate(const char *mapfilename, const char *outfilename,
              int width, int height,
              double x0, double y0, double dx,
              size_t n)
{
    int fd;               // File descriptor
    job_data_t data =     // You can add more here for threads.
    {
        bmp:create_bmp8(mapfilename, width, height),
        width:width,
        height:height,
        padded_width:width + 3-(width+3)%4,
        r0:x0,
        i0:y0,
        dr:dx,
        max:n
    };


    // Compute image.
    // This is the loop to parallelize.

pthread_t threadcount[count_cpus()];
size_t cpucount = count_cpus();


void *startcomputation(void* coreid)
{
    size_t thread_id = (size_t)coreid;
	printf("Thread %d starting \n", thread_id);
    size_t x,y;
	for (y=(height/cpucount)*thread_id;y<(height/cpucount)*(thread_id +1); y++)
	// for (y=0; y<height;y++)
	{
		for (x=0;x<width;x++)
		{
			compute(x, y, &data);
		}
	}
	printf("Thread %d finished work \n", thread_id);
}

size_t mkthrcount;
for (mkthrcount=0;mkthrcount<cpucount;mkthrcount++)
{
	pthread_create(&threadcount[mkthrcount], NULL, startcomputation, (void*)mkthrcount);
}
for (mkthrcount=0;mkthrcount<cpucount;mkthrcount++)
{
	pthread_join(threadcount[mkthrcount], NULL);
}




















    // Save image when the computation is finished.

    fd = open(outfilename, O_WRONLY | O_CREAT, S_IRUSR | S_IWUSR);
    if (fd < 0)
    {
        perror("open");
        abort();
    }
    if (write(fd, "BM", 2) < 0 ||
        write(fd, data.bmp, data.bmp->header.bfSize) < 0)
    {
        perror("write");
        abort();
    }
    close(fd);
    free(data.bmp);
    fprintf(stdout, "%s written.\n", outfilename);
}


int main(int argc, char *argv[])
{
    // Default values.

    int width = 512;
    int height = 512;
    size_t n = 1024;
    float x = -2.5;
    float y = -1.8;
    float dx = 3.6;

    // Test arguments.

    if (argc < 3)
    {
        fprintf(stderr, "Usage: %s map_file out_file [width height x y dx n]\n", argv[0]);
        return 1;
    }

    // Read optional arguments.

    if (argc > 3)
    {
        width = atoi(argv[3]);
    }
    if (width < 16)
    {
        width = 16;
    }
    if (argc > 4)
    {
        height = atoi(argv[4]);
    }
    if (height < 16)
    {
        height = 16;
    }
    if (argc > 5)
    {
        sscanf(argv[5],"%f",&x);
    }
    if (argc > 6)
    {
        sscanf(argv[6],"%f",&y);
    }
    if (argc > 7)
    {
        sscanf(argv[7],"%f",&dx);
    }
    if (argc > 8)
    {
        n = atoi(argv[8]);
    }
    if (n < 256)
    {
        n = 256;
    }

    // Generate the image.

    generate(argv[1], argv[2], width, height, x, y, dx, n);

    return 0;
}
