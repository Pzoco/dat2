using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade.Simulation
{
	class BehaviourInterpreter : Visitor
	{
		//Used to keep track of the regiments assigned
		class RegimentAssignment
		{
			public Regiment regiment;
			public string identifier;
			public RegimentAssignment(Regiment regiment, string identifier)
			{
				this.regiment = regiment;
				this.identifier = identifier;
			}
		}
		private List<RegimentAssignment> regimentAssignments;
		private GameState currentGameState;

		public GameState InterpreteBehaviour(Regiment currentRegiment, GameState gameState)
		{
			currentGameState = gameState;
			currentRegiment.behaviour.Visit(this, null);
			return null;
		}



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
		#region Identifiers etc
		public Object VisitAttackType(AttackType ast, Object obj)
		{
			return DataType.AttackType;
		}
		public Object VisitBlockName(BlockName ast, Object obj)
		{
			return ast.i.Visit(this, null);
		}
		public Object VisitIdentifier(Identifier ast, Object obj)
		{
			return null;
		}
		public Object VisitIntegerLiteral(IntegerLiteral ast, Object obj)
		{
			return DataType.Integer;
		}
		public Object VisitOperator(Operator ast, Object obj)
		{
			return ast.spelling;
		}
		#endregion

		#region Regiment assignment related
		//Regiment Assignment
		public Object VisitRegimentDeclaration(RegimentDeclaration ast, Object obj)
		{
			string identifier = (string)ast.i.Visit(this, null);
			Regiment regiment = (Regiment)ast.rs.Visit(this, null);
			regimentAssignments.Add(new RegimentAssignment(regiment, identifier));
			return null;
		}
		public Object VisitRegimentDeclarationCommand(RegimentDeclarationCommand ast, Object obj)
		{
			ast.rd.Visit(this, null);
			return null;
		}

		//Regiment Search
		public Object VisitBinaryParameter(BinaryParameter ast, Object obj)
		{
			ast.p1.Visit(this, null);
			ast.p2.Visit(this, null);
			return null;
		}
		public Object VisitParameter(Parameter ast, Object obj)
		{
			ast.ust.Visit(this, null);
			DataType eType2 = (DataType)ast.il.Visit(this, null);
			string operatorSpelling = (string)ast.o.Visit(this, null);
			return ast;
		}

		public Object VisitRegimentSearch(RegimentSearch ast, Object obj)
		{
			string regimentSearchSpelling = (string)ast.rsn.Visit(this, null);
			if (regimentSearchSpelling == "SearchForFriends")
			{

			}
			else
			{

			}

			ast.p.Visit(this, null);
			return ast.p.Visit(this, null);
		}
		//Gets regiments by checking if there are any regiments where unitStat op value is true (parameter).
		private List<Regiment> GetRegiments(List<Regiment> regiments, string unitStat, int value, string op)
		{
			List<Regiment> regimentsToGet = new List<Regiment>();
			foreach (Regiment regiment in regiments)
			{
				Regiment foundRegiment = new Regiment();
				switch (unitStat)
				{
					case "AttackSpeed":
						switch (op)
						{
							case ">": break;
							case "<": break;
							case "==": break;

						} break;
					case "Damage":
						switch (op)
						{
							case ">": break;
							case "<": break;
							case "==": break;

						} break;
					case "Distance":
						switch (op)
						{
							case ">": break;
							case "<": break;
							case "==": break;

						} break;
					case "Size":
						switch (op)
						{
							case ">": break;
							case "<": break;
							case "==": break;

						} break;
					case "Range":
						switch (op)
						{
							case ">": break;
							case "<": break;
							case "==": break;

						} break;
					case "Health":
						switch (op)
						{
							case ">": break;
							case "<": break;
							case "==": break;
							case ">=": break;
							case "<=": break;

						} break;
				}
			}
			return regiments;
		}

		public Object VisitRegimentSearchName(RegimentSearchName ast, Object obj)
		{
			return ast.spelling;
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
			return null;
		}
		#endregion

		#region Not used in behaviourblock
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
			ast.bn.Visit(this, null);
			ast.gss.ForEach(x => x.Visit(this, null));
			return null;
		}
		public Object VisitMaximaBlock(MaximaBlock ast, Object obj)
		{
			ast.msds.ForEach(x => x.Visit(this, null));
			return null;
		}
		public Object VisitRegimentBlock(RegimentBlock ast, Object obj)
		{
			ast.bn.Visit(this, null);
			ast.usds.ForEach(x => x.Visit(this, null));
			ast.bb.Visit(this, null);
			return null;
		}
		public Object VisitRulesBlock(RulesBlock ast, Object obj)
		{
			ast.mb.Visit(this, null);
			ast.sb.Visit(this, null);
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
			ast.gsnv.Visit(this, null);
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
			DataType snType = (DataType)ast.sn.Visit(this, null);
			ast.il.Visit(this, null);
			return null;
		}
		public Object VisitUnitStatPositionDeclaration(UnitStatPositionDeclaration ast, Object obj)
		{
			DataType snType = (DataType)ast.sn.Visit(this, null);
			return null;
		}
		public Object VisitUnitStatTypeDeclaration(UnitStatTypeDeclaration ast, Object obj)
		{
			DataType snType = (DataType)ast.sn.Visit(this, null);
			return null;
		}
		public Object VisitUnitStatVName(UnitStatVName ast, Object obj)
		{
			if (ast.spelling == "Size" || ast.spelling == "Range" ||
				ast.spelling == "Damage" || ast.spelling == "Movement")
			{
				ast.dataType = DataType.Integer;
				return ast.dataType;
			}
			else if (ast.spelling == "RegimentPosition")
			{
				ast.dataType = DataType.Position;
				return ast.dataType;
			}
			else if (ast.spelling == "Type")
			{
				ast.dataType = DataType.AttackType;
				return ast.dataType;
			}
			return null;
		}
		#endregion
		//Not used for anything
		public Object VisitBinaryOperatorDeclaration(BinaryOperatorDeclaration ast, Object obj)
		{
			return null;
		}
		#endregion

	}
}
