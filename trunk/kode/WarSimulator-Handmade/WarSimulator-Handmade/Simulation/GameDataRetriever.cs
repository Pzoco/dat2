
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade.Simulation
{
	class GameDataRetriever:Visitor
	{
		private Grid grid = new Grid();
		private Regiment currentRegiment;
		private int currentTeam;
		private Team[] teams;
		private bool checkingForRegiments = true;

		#region public Methods
		public GameState Retrieve(ConfigFile configFile, TeamFile[] teamFiles)
		{
			teams = new Team[teamFiles.Length];
			for (currentTeam = 0; currentTeam < teamFiles.Length; currentTeam++)
			{
				teams[currentTeam] = new Team();
				currentRegiment = new Regiment();
				teamFiles[currentTeam].Visit(this, null);
			}
			configFile.Visit(this, null);
			//Check for if anything is missing in the standards.

			return null;
		}
		public bool CheckUnitStats(Regiment regiment)
		{
			bool errorFound = false;
			if (regiment.size == 0) { errorFound = true; Console.WriteLine("Missing Size for {0}",regiment.name);}
			else if (regiment.health == 0) { errorFound = true; Console.WriteLine("Missing Size for {0}", regiment.name); }
			else if (regiment.movement == 0) { errorFound = true; Console.WriteLine("Missing Movement for {0}", regiment.name); }
			else if (regiment.range == 0) { errorFound = true; Console.WriteLine("Missing Range for {0}", regiment.name); }
			return errorFound;
		}
		#endregion

		#region Visitor Classes
		#region Used in the team/config files
		#region Files
		public Object VisitTeamFile(TeamFile ast, Object obj)
		{
			ast.rb.Visit(this, null);
			return null;
		}
		public Object VisitConfigFile(ConfigFile ast, Object obj)
		{
			ast.rb.Visit(this, null);
			ast.gb.Visit(this, null);
			return null;
		}
		#endregion
		#region Misc
		//Hvad skal vi gøre med denne?
		public Object VisitBehaviourAssignment(BehaviourAssignment ast, Object obj)
		{
			return null;
		}
		#endregion
		#region Blocks
		public Object VisitGridBlock(GridBlock ast, Object obj)
		{
			grid.name = (string)ast.bn.Visit(this, null);
			
			ast.gss.ForEach(x => x.Visit(this, null));
			return null;
		}
		public Object VisitMaximaBlock(MaximaBlock ast, Object obj)
		{
			ast.msds.ForEach(x => x.Visit(this, null));
			return null;
		}
		public Object VisitRegimentBlock(RegimentBlock ast, Object obj) //Useful
		{
			ast.bn.Visit(this, null);
			ast.usds.ForEach(x => x.Visit(this, null));
			currentRegiment.behaviour = (BehaviourBlock)ast.bb.Visit(this, null);
			teams[currentTeam].regiments.Add(currentRegiment);
			return null;
		}
		public Object VisitRulesBlock(RulesBlock ast, Object obj)
		{
			ast.sb.Visit(this, null);
			ast.mb.Visit(this, null);
			return null;
		}
		public Object VisitStandardsBlock(StandardsBlock ast, Object obj)
		{
			ast.bb.Visit(this, null);
			ast.usds.ForEach(x => x.Visit(this, null));
			return null;
		}
		#endregion
		#region Stats

		public Object VisitGridStatDeclaration(GridStatDeclaration ast, Object obj)
		{
			ast.il.Visit(this, null);
			if ((string)ast.gsnv.Visit(this, null) == "Width")
			{
				grid.width = (int)ast.il.Visit(this, null);
			}
			else if ((string)ast.gsnv.Visit(this, null) == "Height")
			{
				grid.height = (int)ast.il.Visit(this, null);
			}
			return null;
		}
		public Object VisitGridStatVName(GridStatVName ast, Object obj)
		{
			return null;
		}
		public Object VisitMaximaStatDeclaration(MaximaStatDeclaration ast, Object obj)
		{
			ast.il.Visit(this, null);
			ast.msv.Visit(this, null);
			return null;
		}
		public Object VisitMaximaStatVName(MaximaStatVName ast, Object obj)
		{
			return null;
		}
		public Object VisitUnitStatIntegerDeclaration(UnitStatIntegerDeclaration ast, Object obj)
		{
			string spelling = (string)ast.sn.Visit(this, null);
			int value = (int)ast.il.Visit(this, null);
			switch (spelling)
			{
				case "Size": currentRegiment.size = value; break;
				case "Damage": currentRegiment.damage = value; break;
				case "Range": currentRegiment.range = value; break;
				case "Health": currentRegiment.health = value; break;
				case "Movement": currentRegiment.movement = value; break;
				case "AttackSpeed": currentRegiment.attackSpeed = value; break;
			}
			return null;
		}
		public Object VisitUnitStatPositionDeclaration(UnitStatPositionDeclaration ast, Object obj)
		{
			int x = Int32.Parse((string)ast.x.Visit(this, null));
			int y = Int32.Parse((string)ast.y.Visit(this,null));
			currentRegiment.position = new Regiment.Position(x,y);
			return null;
		}
		public Object VisitUnitStatTypeDeclaration(UnitStatTypeDeclaration ast, Object obj)
		{
			string spelling = (string)ast.at.Visit(this,null);
			currentRegiment.type = (Regiment.AttackType)Enum.Parse(typeof(Regiment.AttackType), spelling, true);
			return null;
		}
		public Object VisitUnitStatVName(UnitStatVName ast, Object obj)
		{
			return ast.spelling;
		}
		public Object VisitBinaryOperatorDeclaration(BinaryOperatorDeclaration ast, Object obj)
		{
			return null;
		}
		#endregion
		#endregion
	
		#region Not used here
		public Object VisitBehaviourBlock(BehaviourBlock ast, Object obj)
		{
			ast.bn.Visit(this, null);
			ast.sc.Visit(this, null);
			return null;
		}
		#region Control Structures
		public Object VisitIfCommand(IfCommand ast, Object obj)
		{
			DataType type = (DataType)ast.e.Visit(this, null);
			ast.sc1.Visit(this, null);
			if (ast.eifc != null) { ast.eifc.ForEach(x => x.Visit(this, null)); }
			if (ast.sc2 != null) { ast.sc2.Visit(this, null); }
			return null;
		}
		public Object VisitElseIfCommand(ElseIfCommand ast, Object obj)
		{
			DataType type = (DataType)ast.e.Visit(this, null);
			ast.sc.Visit(this, null);
			return null;
		}
		public Object VisitWhileCommand(WhileCommand ast, Object obj)
		{
			DataType type = (DataType)ast.e.Visit(this, null);
			ast.sc.Visit(this, null);
			return null;
		}
		#endregion
		#region Expressions
		public Object VisitBinaryExpression(BinaryExpression ast, Object obj)
		{
			DataType eType1 = (DataType)ast.e1.Visit(this, null);
			DataType eType2 = (DataType)ast.e2.Visit(this, null);
			Declaration declaration = (Declaration)ast.o.Visit(this, null);
			if (declaration != null)
			{
				BinaryOperatorDeclaration bod = (BinaryOperatorDeclaration)declaration;
				ast.type = bod.result;
			}
			return ast.type;
		}
		public Object VisitIntegerExpression(IntegerExpression ast, Object obj)
		{
			ast.type = DataType.Integer;
			return ast.type;
		}
		public Object VisitRegimentStatExpression(RegimentStatExpression ast, Object obj)
		{
			ast.type = DataType.Integer;
			return ast.type;
		}

		//Vi har ikke engang unary expressions?
		public Object VisitUnaryExpression(UnaryExpression ast, Object obj)
		{
			return null;
		}
		public Object VisitUnitStatVNameExpression(UnitStatVNameExpression ast, Object obj)
		{
			ast.type = (DataType)ast.ust.Visit(this, null);
			return ast.type;
		}
		#endregion

		#region Regiment assignment related
		//Regiment Assignment
		public Object VisitRegimentDeclaration(RegimentDeclaration ast, Object obj)
		{
			ast.rs.Visit(this, null);
			Declaration declaration = (Declaration)ast.i.Visit(this, null);
			ast.i.type = DataType.Regiment;
			return null;
		}
		public Object VisitRegimentDeclarationCommand(RegimentDeclarationCommand ast, Object obj)
		{
			ast.rd.Visit(this, null);
			return null;
		}

		//Regiment Search
		public Object VisitParameter(Parameter ast, Object obj)
		{
			DataType eType1 = (DataType)ast.ust.Visit(this, null);
			DataType eType2 = (DataType)ast.il.Visit(this, null);
			Declaration declaration = (Declaration)ast.o.Visit(this, null);
			if (declaration != null)
			{
				BinaryOperatorDeclaration bod = (BinaryOperatorDeclaration)declaration;
			}
			return null;
		}

		public Object VisitRegimentSearch(RegimentSearch ast, Object obj)
		{
			ast.rsn.Visit(this, null);
			ast.p.ForEach(x=> x.Visit(this, null));
			return null;
		}
		public Object VisitRegimentSearchName(RegimentSearchName ast, Object obj)
		{
			return null;
		}

		//Unit function
		public Object VisitUnitFunction(UnitFunction ast, Object obj)
		{
			Declaration declaration = (Declaration)ast.i.Visit(this, null);
			ast.ufn.Visit(this, null);
			return null;
		}
		public Object VisitUnitFunctionName(UnitFunctionName ast, Object obj)
		{
			return null;
		}

		//Regiment stat
		public Object VisitRegimentStat(RegimentStat ast, Object obj)
		{
			ast.i.Visit(this, null);
			ast.ust.Visit(this, null);
			return null;
		}
		public Object VisitUnitStatType(UnitStatType ast, Object obj)
		{
			return ast.spelling;
		}
		#endregion
		#endregion

		#region Identifiers etc
		public Object VisitAttackType(AttackType ast, Object obj)
		{
			return ast.spelling;
		}
		public Object VisitBlockName(BlockName ast, Object obj)
		{
			return ast.i.Visit(this, null);
		}
		public Object VisitIdentifier(Identifier ast, Object obj)
		{
			return ast.spelling;
		}
		public Object VisitIntegerLiteral(IntegerLiteral ast, Object obj)
		{
			return ast.spelling;
		}
		public Object VisitOperator(Operator ast, Object obj)
		{
			return ast.spelling;
		}
		#endregion
		#endregion
	}
}
