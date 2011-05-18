using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WarSimulator_Handmade.Simulation;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
			if (currentGameState != null)
			{
				Console.WriteLine("Starting round {0}", round);
				UpdateTurnOrder();
				foreach (Regiment regiment in regimentTurnOrder)
				{
					currentGameState = behaviourInterpreter.InterpreteBehaviour(regiment, currentGameState);
				}
				round++;
			}
			base.Update(gameTime);
		}
		protected override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			foreach (Team team in currentGameState.teams)
			{
				foreach (Regiment regiment in team.regiments)
				{
					spriteBatch.Draw(regTexture, regiment.position.ToVector2(), Color.White);
				}
			}
			for (int x = 0; x < Grid.width; x++)
			{
				for (int y = 0; y < Grid.height; y++)
				{
					spriteBatch.Draw(gridTexture, new Vector2(x * 50, y * 50), Color.White);
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
