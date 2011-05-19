﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WarSimulator_Handmade.Simulation;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;

namespace WarSimulator_Handmade
{
	public class Simulator : Microsoft.Xna.Framework.Game
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

		//Teams left
		private int teamsLeft = 0;

		//timer
		private int timer;

		//Game has ended?
		bool gameEnded = false;

		//SpriteBatch used to draw sprites with
		private SpriteBatch spriteBatch;

		//Textures for reg and grid
		GraphicsDeviceManager graphics;
		private Texture2D gridTexture;
		private Texture2D regTexture;
		private SpriteFont spriteFont;
		
		#endregion

		#region Constructor
		public Simulator(ConfigFile configFile, TeamFile[] teamFiles)
		{
			currentGameState = gameDataRetriever.Retrieve(configFile, teamFiles);
			teamsLeft = teamFiles.Length;
			Content.RootDirectory = "Content";
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 500;
		}
		#endregion

		#region XNA Methods
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			
			gridTexture = Content.Load<Texture2D>("gridTile");
			regTexture = Content.Load<Texture2D>("regTile");
			spriteFont = Content.Load<SpriteFont>("font");
			base.LoadContent();
		}
		protected override void Update(GameTime gameTime)
		{
			Console.WriteLine(timer);
			if (timer %300 == 0)
			{
				if (currentGameState != null && gameEnded == false)
				{
					round++;
					UpdateTurnOrder();
					GameState.messages.Clear();
					foreach (Regiment regiment in regimentTurnOrder)
					{
						GameState.messages.Add("Regiment " + regiment.name + "s turn - regiments size is " + regiment.currentSize);
						if (regiment.currentSize > 0)
						{
							currentGameState = behaviourInterpreter.InterpreteBehaviour(regiment, currentGameState);
						}
						else
						{
							currentGameState.teams[regiment.team].regiments.Remove(regiment);
							if (currentGameState.teams[regiment.team].regiments.Count <= 0)
							{
								teamsLeft--;
								if (teamsLeft == 1)
								{
									gameEnded = true;
								}
							}
						}
					}
				}
			}
			timer++;
			base.Update(gameTime);
		}
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
			spriteBatch.Begin();
			for (int x = 0; x < Grid.width; x++)
			{
				for (int y = 0; y < Grid.height; y++)
				{
					spriteBatch.Draw(gridTexture, new Vector2(x * 50, y * 50), Color.White);
				}
			}
			foreach (Team team in currentGameState.teams)
			{
				Color color = new Color();
				switch (team.number)
				{
					case 0: color = Color.Red; break;
					case 1: color = Color.Blue; break;
					case 2: color = Color.Yellow; break;
					case 3: color = Color.Green; break;
				}
				foreach (Regiment regiment in team.regiments)
				{
					spriteBatch.Draw(regTexture, regiment.position.ToVector2()*50, color);
				}
			}
			for (int i = 0; i < GameState.messages.Count; i++)
			{
				spriteBatch.DrawString(spriteFont, GameState.messages[i], new Vector2(350, 40 * (i+1)),Color.Black);
			}
			spriteBatch.DrawString(spriteFont, round.ToString(), new Vector2(350, 0), Color.Black);
			spriteBatch.End();
			base.Draw(gameTime);
		}
		#endregion

		#region Methods
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
