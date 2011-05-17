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

		public Simulator(ConfigFile configFile, TeamFile[] teamFiles)
		{
			InitializeData(configFile, teamFiles);
			BeginSimulation();
		}

		private void InitializeData(ConfigFile configFile, TeamFile[] teamFiles)
		{
			//Retrive data an put it in the teams.
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

		}
    }
}
