using System;
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

		//Game has ended?
		bool gameEnded = false;

		//SpriteBatch used to draw sprites with
		private SpriteBatch spriteBatch;

		//Textures for reg and grid
		GraphicsDeviceManager graphics;
		private Texture2D gridTexture;
		private Texture2D regTexture;
		
		#endregion

		#region Constructor
		public Simulator(ConfigFile configFile, TeamFile[] teamFiles)
		{
			currentGameState = gameDataRetriever.Retrieve(configFile, teamFiles);
			teamsLeft = teamFiles.Length;
			Content.RootDirectory = "Content";
			graphics = new GraphicsDeviceManager(this);
		}
		#endregion

		#region XNA Methods
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			
			gridTexture = Content.Load<Texture2D>("gridTile");
			regTexture = Content.Load<Texture2D>("regTile");
			base.LoadContent();
		}
		protected override void Update(GameTime gameTime)
		{
			if (currentGameState != null && gameEnded == false)
			{
				Console.WriteLine("Starting round {0}", round);
				UpdateTurnOrder();
				foreach (Regiment regiment in regimentTurnOrder)
				{
					Console.WriteLine("Regiment {0}s turn - current size is {1}",regiment.name,regiment.currentSize);
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
					Thread.Sleep(500);
				}
				round++;
			}
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
				foreach (Regiment regiment in team.regiments)
				{
					spriteBatch.Draw(regTexture, regiment.position.ToVector2()*50, Color.White);
				}

			}
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
