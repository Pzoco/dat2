#!/bin/sh

gcc -o mandelbrot mandelbrot.c -Wall -O3 -lpthread || exit 1

time ./mandelbrot maps/firestrm.map test0.bmp 1024 1024
time ./mandelbrot maps/blues.map test1.bmp 1280 1280 -1.5 -0.1 0.5 1024
time ./mandelbrot maps/volcano.map test2.bmp 1280 1280 -1.5 -0.125 0.25 1024
time ./mandelbrot maps/royal.map test3.bmp 1280 1280 -0.80 0.1 0.0625 2048
time ./mandelbrot maps/neon.map test4.bmp 2360 2048 -0.75 0.0575 0.002 4096

