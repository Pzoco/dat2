using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarSimulator_Handmade.Simulation
{
	//Class for the current gamestate - A gamestate is the state of the game at a specifc moment
	class GameState
	{
		public class Message
		{
			public string text;
			public Color color;
			public Message(string text, Color color)
			{
				this.text = text;
				this.color = color;
			}
		}

		public Grid grid;
		public Team[] teams;
		public static List<Message> messages = new List<Message>();
		public GameState(Grid grid, Team[] teams)
		{
			this.grid = grid;
			this.teams = teams;
		}
		public static void AddMessage(string text, Color color)
		{
			messages.Add(new Message(text, color));
		}
	}
}

