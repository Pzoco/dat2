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
			public override string ToString()
			{
				return "<"+x+","+y+">";
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
		public bool hasAttacked = false;
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
			if (!hasAttacked)
			{
				int distance = GetDistanceTo(regiment);
				if (distance == 1)
				{
					GameState.messages.Add("Regiment "+name+" deals damage to "+regiment.name);
					regiment.GetDamage(damage);
				}
				else if (range > distance && type == AttackType.Ranged)
				{
					GameState.messages.Add("Regiment "+name+" deals damage to "+regiment.name);
					regiment.GetDamage(damage);
				}
				hasAttacked = true;
			}
		}
		public void MoveAway(Regiment regiment)
		{
			double angle = Math.Abs(Math.Atan2(position.y - regiment.position.y, 
												position.x - regiment.position.x) * 180 / Math.PI);
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
		public void MoveTowards(Regiment regiment)
		{
			double angle = Math.Abs(Math.Atan2(position.y - regiment.position.y,
										position.x - regiment.position.x) * 180 / Math.PI);
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
			if (currentSize <= 0) { return; }
			while (damage > 0)
			{
				if (currentHealth < damage)
				{
					damage -= currentHealth;
					currentHealth = health;
					currentSize--;
				}
				else
				{
					currentHealth -= damage;
					break;
				}
				if (damage > health)
				{
					int sizeRemoved = (int)Math.Floor((double)(damage / health));
					damage -= sizeRemoved * health;
					currentSize -= sizeRemoved;
				}
				if (currentSize <= 0)
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
				if (!Grid.tiles[position.x, position.y-1].occupied)
				{
					Console.WriteLine("Regiment {0} moves up", name);
					Grid.tiles[position.x, position.y].occupied = false;
					position.y -= 1;
					Grid.tiles[position.x, position.y].occupied = true;
				}
			}
		}
		private void MoveDown()
		{
			if (position.y < Grid.height-1)
			{
				if (!Grid.tiles[position.x, position.y + 1].occupied)
				{
					Console.WriteLine("Regiment {0} moves down", name);
					Grid.tiles[position.x, position.y].occupied = false;
					position.y += 1;
					Grid.tiles[position.x, position.y].occupied = true;
				}
			}
		}
		private void MoveLeft()
		{
			if (position.x > 0)
			{
				if (!Grid.tiles[position.x-1, position.y].occupied)
				{
					Console.WriteLine("Regiment {0} moves left", name);
					Grid.tiles[position.x, position.y].occupied = false;
					position.x -= 1;
					Grid.tiles[position.x, position.y].occupied = true;
				}
			}
		}
		private void MoveRight()
		{
			if (position.x < Grid.width-1)
			{
				if (!Grid.tiles[position.x + 1, position.y].occupied)
				{
					Console.WriteLine("Regiment {0} moves left", name);
					Grid.tiles[position.x, position.y].occupied = false;
					position.x += 1;
					Grid.tiles[position.x, position.y].occupied = true;
				}
			}
		}
		#endregion
	}
}
