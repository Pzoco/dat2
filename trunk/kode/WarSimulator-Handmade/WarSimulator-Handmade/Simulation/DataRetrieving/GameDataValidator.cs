using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade.Simulation
{
	class GameDataValidator
	{
		MaximaLimit maximaLimit;
		Regiment standards;
		Grid grid;
		Team[] teams;
		public bool valid = true;

		public GameDataValidator(MaximaLimit maximaLimit, Regiment standards, Grid grid, Team[] teams)
		{
			this.maximaLimit = maximaLimit;
			this.standards = standards;
			this.grid = grid;
			this.teams = teams;
			CheckData();
		}

		private void CheckData()
		{
			if (!CheckForMaxima())
			{
				Console.WriteLine("Maxima Exceeded");
				valid = false;
			}
			
		}
		private bool CheckForMissingData()
		{
			return false;
		}
		private bool CheckForMaxima()
		{
			#region Checks number of teams
			if (teams.Length > maximaLimit.teams && maximaLimit.teams != 0)
			{
				return false;
			}
			#endregion
			#region Checks the number of regiments and unitstats of the regiments
			foreach (Team team in teams)
			{
				if (team.regiments.Count > maximaLimit.regiments && maximaLimit.regiments != 0)
				{
					Console.WriteLine("Number of regiments exceeded got {0} - Allowed {1}", team.regiments.Count, maximaLimit.regiments);
					return false;
				}
				foreach (Regiment regiment in team.regiments)
				{
					if (regiment.attackSpeed > maximaLimit.attackSpeed && maximaLimit.attackSpeed != 0) {return false; }
					else if (regiment.damage > maximaLimit.damage && maximaLimit.damage != 0){return false;}
					else if (regiment.range > maximaLimit.range && maximaLimit.range != 0) { return false; }
					else if (regiment.size > maximaLimit.size && maximaLimit.size != 0) { return false; }
					else if (regiment.movement > maximaLimit.movement && maximaLimit.movement != 0) { return false; }
					else if (regiment.health > maximaLimit.health && maximaLimit.health != 0) { return false; }
				}
			}
			#endregion
			
			return true;
		}
		private bool CheckPositions()
		{
			return false;
		}
	}
}
