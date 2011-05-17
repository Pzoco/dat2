using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarSimulator_Handmade.Simulation;
namespace WarSimulator_Handmade
{
    public class Simulator
    {
		private Team[] teams;
		private Grid grid;
		private List<Regiment> regimentTurnOrder;
		private GameDataRetriever gameDataRetriever = new GameDataRetriever();
		private GameState currentGamestate;
		public Simulator(ConfigFile configFile, TeamFile[] teamFiles)
		{
			gameDataRetriever.Retrieve(configFile, teamFiles);
			BeginSimulation();
		}


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
			regimentTurnOrder.Sort((a, b) => a.movement.CompareTo(b.movement));

		}
    }
}
