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

		//Used to interprete behaviours
		BehaviourInterpreter behaviourInterpreter = new BehaviourInterpreter();

		//Current state of the game
		private GameState currentGameState;

		//Round
		private int round = 0;
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
			//GameLoop
			while (true)
			{
				Console.WriteLine("Starting round {0}", round);
				UpdateTurnOrder();
				foreach (Regiment regiment in regimentTurnOrder)
				{
					currentGameState = behaviourInterpreter.InterpreteBehaviour(regiment, currentGameState);
				}
				round++;
			}
		}
		private void UpdateTurnOrder()
		{
			regimentTurnOrder.Clear();
			foreach (Team team in currentGameState.teams)
			{
				regimentTurnOrder.AddRange(team.regiments);
			}

			//Sorts the regimentTurnOrder descending
			regimentTurnOrder.Sort((a, b) => a.movement.CompareTo(b.movement));
		}
		#endregion
	}
}
