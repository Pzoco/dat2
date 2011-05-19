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
		List<Regiment.Position> regimentPositions = new List<Regiment.Position>();
		public bool valid = true;

		public GameDataValidator(MaximaLimit maximaLimit, Regiment standards, Grid grid, Team[] teams)
		{
			this.maximaLimit = maximaLimit;
			this.standards = standards;
			this.grid = grid;
			this.teams = teams;
			SetStandardsData(standards);
			CheckData();
		}

		private void SetStandardsData(Regiment regiment)
		{
			if (regiment.attackSpeed == 0) { regiment.attackSpeed = 1; }
			if (regiment.damage == 0) { regiment.attackSpeed = 1; }
			if (regiment.health == 0) { regiment.attackSpeed = 1; }
			if (regiment.movement == 0) { regiment.attackSpeed = 1; }
			if (regiment.range == 0) { regiment.attackSpeed = 1; }
			if (regiment.size == 0) { regiment.attackSpeed = 1; }
		}
		private bool CheckData()
		{
			#region Checks number of teams
			if (teams.Length > maximaLimit.teams && maximaLimit.teams != 0)
			{
				return false;
			}
			#endregion

			foreach (Team team in teams)
			{
				if (team.regiments.Count > maximaLimit.regiments && maximaLimit.regiments != 0)
				{
					Console.WriteLine("Number of regiments exceeded got {0} - Allowed {1}", team.regiments.Count, maximaLimit.regiments);
					return false;
				}
				foreach (Regiment regiment in team.regiments)
				{
					if (!CheckMaximastats(regiment)) { return false; }
					if (!CheckForMissingData(regiment)){return false;}
					regimentPositions.Add(regiment.position);
				}
				if (!CheckPositions()) { return false; }
			}

			return true;
		}
		private bool CheckMaximastats(Regiment regiment)
		{
			if (regiment.attackSpeed > maximaLimit.attackSpeed && maximaLimit.attackSpeed != 0) { return false; }
			else if (regiment.damage > maximaLimit.damage && maximaLimit.damage != 0) { return false; }
			else if (regiment.range > maximaLimit.range && maximaLimit.range != 0) { return false; }
			else if (regiment.size > maximaLimit.size && maximaLimit.size != 0) { return false; }
			else if (regiment.movement > maximaLimit.movement && maximaLimit.movement != 0) { return false; }
			else if (regiment.health > maximaLimit.health && maximaLimit.health != 0) { return false; }
			return true;
		}
		private bool CheckForMissingData(Regiment regiment)
		{
			if (regiment.attackSpeed == 0) { regiment.attackSpeed = standards.attackSpeed; }
			if (regiment.damage == 0) { regiment.attackSpeed = standards.damage; }
			if (regiment.health == 0) { regiment.attackSpeed = standards.health; }
			if (regiment.movement == 0) { regiment.attackSpeed = standards.movement; }
			if (regiment.range == 0) { regiment.attackSpeed = standards.range; }
			if (regiment.size == 0) { regiment.attackSpeed = standards.size; }
			return true;
		}
		private bool CheckPositions()
		{
			List<Regiment.Position> duplicates = new List<Regiment.Position>();
			HashSet<Regiment.Position> uniques = new HashSet<Regiment.Position>();
			foreach (Regiment.Position p in regimentPositions)
			{
				Grid.tiles[p.x, p.y].occupied = true;
				if (uniques.Contains(p))
				{
					duplicates.Add(p);
				}
				else
				{
					uniques.Add(p);
				}
			}
			if (duplicates.Count > 0) { return false; }
			return true;
		}
	}
}
