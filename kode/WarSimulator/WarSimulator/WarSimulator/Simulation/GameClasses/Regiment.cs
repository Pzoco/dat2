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
				return "<" + x + "," + y + ">";
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
		public int currentMovement;
		public int attackSpeed;
		public int team;
		public string name;
		public Position position;
		public AttackType type;
		public BehaviourBlock behaviour;
		public bool hasAttacked = false;
		public Color color;
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
				if (range >= distance)
				{
					GameState.AddMessage(name + " deals " + (damage * attackSpeed*currentSize) + " damage to " + regiment.name, color);
					regiment.GetDamage((damage * attackSpeed));
					hasAttacked = true;
				}
			}
		}
		public void MoveAway(Regiment regiment)
		{
			if (currentMovement > 0)
			{
				double angle = GetAngle(regiment.position, position);

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
		}
		public void MoveTowards(Regiment regiment)
		{
			if (currentMovement > 0)
			{
				double angle = GetAngle(position, regiment.position);
				Console.WriteLine("Trying to move angle is "+angle);
				if (angle >= 215 && angle <= 305 && !Grid.tiles[position.x, position.y + 1].occupied)
				{
					MoveDown();
				}
				else if (angle >= 135 && angle <= 215 && !Grid.tiles[position.x + 1, position.y].occupied)
				{
					MoveRight();
				}
				else if (angle >= 45 && angle <= 135 && !Grid.tiles[position.x, position.y - 1].occupied)
				{
					MoveUp();
				}
				else if (angle >= 305 || angle <= 45 && !Grid.tiles[position.x - 1, position.y].occupied)
				{
					MoveLeft();
				}
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
					GameState.AddMessage(name + " died", color);
					Grid.tiles[position.x, position.y].occupied = false;
					break;
				}
			}
		}
		private double GetAngle(Position pos1, Position pos2)
		{
			double radian = Math.Atan2(pos1.y - pos2.y,
												pos1.x - pos2.x);
			if (radian < 0)
			{
				return (radian * (180 / Math.PI))+360;
			}
			return radian * (180 / Math.PI);
		}
		#endregion

		#region Move methods
		private void MoveUp()
		{
			if (position.y > 0)
			{
				if (!Grid.tiles[position.x, position.y - 1].occupied)
				{
					Grid.tiles[position.x, position.y].occupied = false;
					position.y -= 1;
					GameState.AddMessage(name + " moves up to " + position.ToString(), color);
					Grid.tiles[position.x, position.y].occupied = true;
					currentMovement--;
				}
			}
		}
		private void MoveDown()
		{
			if (position.y < Grid.height - 1)
			{
				if (!Grid.tiles[position.x, position.y + 1].occupied)
				{
					Grid.tiles[position.x, position.y].occupied = false;
					position.y += 1;
					GameState.AddMessage(name + " moves down to " + position.ToString(), color);
					Grid.tiles[position.x, position.y].occupied = true;
					currentMovement--;
				}
			}
		}
		private void MoveLeft()
		{
			if (position.x > 0)
			{
				if (!Grid.tiles[position.x - 1, position.y].occupied)
				{
					Grid.tiles[position.x, position.y].occupied = false;
					position.x -= 1;
					GameState.AddMessage(name + " moves left to " + position.ToString(), color);
					Grid.tiles[position.x, position.y].occupied = true;
					currentMovement--;
				}
			}
		}
		private void MoveRight()
		{
			if (position.x < Grid.width - 1)
			{
				if (!Grid.tiles[position.x + 1, position.y].occupied)
				{
					Grid.tiles[position.x, position.y].occupied = false;
					position.x += 1;
					GameState.AddMessage(name + " moves right to " + position.ToString(), color);
					Grid.tiles[position.x, position.y].occupied = true;
					currentMovement--;
				}
			}
		}
		#endregion
	}
}
