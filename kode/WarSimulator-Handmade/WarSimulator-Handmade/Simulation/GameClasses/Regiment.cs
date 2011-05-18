using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
			public Vector2 ToVector2()
			{
				return new Vector2(x, y);
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
			if (GetDistanceTo(regiment) == 1)
			{
				regiment.GetDamage(damage);
			}
		}
		public void MoveTowards(Regiment regiment)
		{
			double angle = Math.Atan2(position.y - regiment.position.y, position.x - regiment.position.x) * 180 / Math.PI;
			if (angle > 45 && angle <= 135)
			{
				MoveUp();
			}
			else if (angle > 135 && angle <= 215)
			{
				MoveLeft();
			}
			else if (angle > 215 && angle <= 305)
			{
				MoveDown();
			}
			else if (angle > 305 || angle < 45)
			{
				MoveRight();
			}
		}
		public void MoveAway(Regiment regiment)
		{
			double angle = Math.Atan2(position.y - regiment.position.y, position.x - regiment.position.x) * 180 / Math.PI;
			if (angle > 45 && angle <= 135)
			{
				MoveDown();
			}
			else if (angle > 135 && angle <= 215)
			{
				MoveRight();
			}
			else if (angle > 215 && angle <= 305)
			{
				MoveUp();
			}
			else if (angle > 305 || angle < 45)
			{
				MoveLeft();
			}
		}
		public void GetDamage(int damage)
		{
			if (size <= 0) { return; }
			while (damage > 0)
			{
				if (currentHealth < damage)
				{
					damage -= currentHealth;
					currentHealth = health;
					size--;
				}
				if (damage > 0)
				{
					int sizeRemoved = (int)Math.Floor((double)(damage / health));
					damage -= sizeRemoved * health;
					size -= sizeRemoved;
				}
				if (size <= 0)
				{
					break;
				}
			}
		}
		#endregion

		#region Move methods
		private void MoveUp()
		{
			if (position.y > 0)
			{
				position.y -= 1;
			}
		}
		private void MoveDown()
		{
			if (position.y < Grid.height)
			{
				position.y += 1;
			}
		}
		private void MoveLeft()
		{
			if (position.x > 0)
			{
				position.x -= 1;
			}
		}
		private void MoveRight()
		{
			if (position.x < Grid.width)
			{
				position.x += 1;
			}
		}
		#endregion
	}
}
