using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade.Simulation
{
    class Regiment
    {
		public struct Position
		{
			int x,y;
			public Position (int x,int y)
			{
				this.x = x;
				this.y = y;
			}
		}
		public enum AttackType { Melee, Ranged }

		public int size;
		public int range;
		public int damage;
		public int movement;
		public int attackSpeed;
		public Position position;
		public AttackType type;
		public string name;
		public BehaviourBlock behaviour;

    }
}
