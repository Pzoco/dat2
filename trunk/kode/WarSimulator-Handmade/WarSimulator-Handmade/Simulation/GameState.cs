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
		public List<Team> teams;
		public GameState(Grid grid, List<Team> teams)
		{
			this.grid = grid;
			this.teams = teams;
		}

		//Checks if all the data is valid - Used after retrieving
		public bool Validate()
		{

			return false;
		}
	}
}

