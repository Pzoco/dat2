using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade.Simulation
{
	class Regiment
	{
		#region A struct for position and an enum for attacktype
		public struct Position
		{
			public int x, y;
			public Position(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}
		public enum AttackType { Melee, Ranged }
		#endregion

		#region Fields
		public int size;
		public int currentSize;
		public int currentHealth;
		public int health;
		public int range;
		public int damage;
		public int movement;
		public int attackSpeed;
		public int team;
		public string name;
		public Position position;
		public AttackType type;
		public BehaviourBlock behaviour;
		#endregion

		#region Public Methods

		public int GetDistanceTo(Regiment regiment)
		{
			int distance = 0;
			distance += Math.Abs(regiment.position.x - position.x);
			distance += Math.Abs(regiment.position.y - position.y);
			return distance;
		}
		public void Attack(Regiment regiment)
		{
			regiment.health -= damage * size;

		}
		public void MoveTowards()
		{

		}
		public void MoveAway()
		{

		}
		#endregion

		#region Private Methods
		private void GetDamage(int damage)
		{
			while (damage > 0)
			{
				
			}
		}
		#endregion
	}
}
