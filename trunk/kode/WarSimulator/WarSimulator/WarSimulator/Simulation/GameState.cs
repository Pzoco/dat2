using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade.Simulation
{
	//Class for the current gamestate - A gamestate is the state of the game at a specifc moment
	class GameState
	{
		public Grid grid;
		public Team[] teams;
		public static List<string> messages = new List<string>();
		public GameState(Grid grid, Team[] teams)
		{
			this.grid = grid;
			this.teams = teams;
		}

	}
}

