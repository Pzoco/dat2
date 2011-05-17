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
		Tile[,] tiles;
		public int width;
		public int height;

		public void InstantiateGrid()
		{
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
