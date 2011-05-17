using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarSimulator_Handmade.Simulation;
namespace WarSimulator_Handmade
{
    public class Simulator
	{
		#region Fields
		//Stores the order in which the regiment move
		private List<Regiment> regimentTurnOrder = new List<Regiment>();

		//Used to retrieve gamedata from the ASTs
		private GameDataRetriever gameDataRetriever = new GameDataRetriever();

		//Current state of the game
		private GameState currentGameState;
		#endregion

		#region Constructor
		public Simulator(ConfigFile configFile, TeamFile[] teamFiles)
		{
			currentGameState = gameDataRetriever.Retrieve(configFile, teamFiles);
			if (currentGameState != null)
			{
				BeginSimulation();
			}
			else
			{
				Console.WriteLine("Failed to start Simulation");
			}
		}
		#endregion

		#region Methods
		private void BeginSimulation()
		{
			UpdateTurnOrder();
			foreach (Regiment regiment in regimentTurnOrder)
			{
				
			}
		}
		private void UpdateTurnOrder()
		{
			//Sorts the regimentTurnOrder descending
			//regimentTurnOrder = currentGamestate.regiment.Sort((a, b) => a.movement.CompareTo(b.movement));
		}
		#endregion
	}
}
