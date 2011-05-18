using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade.Simulation
{
    class Grid
    {
		
		//GridSize, 
		public string name;
		public Tile[,] tiles;
		static public int width;
		static public int height;

		public void InstantiateGrid()
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
