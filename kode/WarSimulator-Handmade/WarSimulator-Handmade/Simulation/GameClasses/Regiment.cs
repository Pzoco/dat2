﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade.Simulation
{
    class Regiment
    {
		public struct Position
		{
			public int x,y;
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
		public int health;
		public int movement;
		public int attackSpeed;
		public int team;
		public string name;
		public Position position;
		public AttackType type;
		public BehaviourBlock behaviour;

		public int GetDistanceTo(Regiment regiment)
		{
			int distance = 0;
			distance += Math.Abs(regiment.position.x - position.x);
			distance += Math.Abs(regiment.position.y - position.y);
			return distance;
		}
    }
}
