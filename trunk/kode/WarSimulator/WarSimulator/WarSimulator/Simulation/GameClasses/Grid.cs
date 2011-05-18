using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade.Simulation
{
	class Grid
	{
		public static string name;
		public static int width;
		public static int height;
		public static Tile[,] tiles;

		public static void InstantiateGrid()
		{
			tiles = new Tile[width, height];
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					tiles[x, y] = new Tile();
				}
			}
		}
	}
}
