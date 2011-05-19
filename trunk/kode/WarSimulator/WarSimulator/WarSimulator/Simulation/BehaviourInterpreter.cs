using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade.Simulation
{
	class BehaviourInterpreter : Visitor
	{
		#region Nested Classes
		//Used to keep track of the regiments assigned
		private class RegimentAssignment
		{
			public Regiment regiment;
			public string identifier;
			public RegimentAssignment(Regiment regiment, string identifier)
			{
				this.regiment = regiment;
				this.identifier = identifier;
			}
		}
		private abstract class Value { }
		private class BoolValue : Value { public bool b;}
		private class IntValue : Value { public int i;}
		private class TypeValue : Value {public Regiment.AttackType t;}
		private class UndefinedValue : Value { }
		#endregion

		#region Fields
		//List of regimentAssignments
		private List<RegimentAssignment> regimentAssignments = new List<RegimentAssignment>();

		//Gamestate used to keep track of the grid and the other regiments
		private GameState currentGameState;

		//The regiment which has its turn
		private Regiment currentRegiment;

		#endregion

		#region Methods
		//Interpretes the behaviour of a regiment and updates the state of the game
		public GameState InterpreteBehaviour(Regiment regiment, GameState gameState)
		{
			regimentAssignments.Clear();
			GameState.messages.Clear();
			currentGameState = gameState;
			currentRegiment = regiment;
			currentRegiment.hasAttacked = false;
			currentRegiment.behaviour.Visit(this, null);
			return currentGameState;
		}

		//Gets a regiment from the regiment assignments
		private Regiment GetRegiment(string identifier)
		{
			foreach (RegimentAssignment regAss in regimentAssignments)
			{
				if (regAss.identifier == identifier)
				{
					return regAss.regiment;
				}
			}
			return null;
		}

		//Gets regiments by checking if there are any regiments where unitStat op value is true (parameter).
		private List<Regiment> GetRegiments(List<Regiment> regiments, Parameter parameter)
		{
			List<Regiment> regimentsToGet = new List<Regiment>();

			//Extracting data
			string unitStat = (string)parameter.ust.spelling;
			string op = (string)parameter.o.spelling;
			int value = Int32.Parse(parameter.il.spelling);

			//Tries to find a regiment which matches the criteria/parameter
			foreach (Regiment regiment in regiments)
			{
				if (regiment.team == currentRegiment.team) { continue; }
				switch (unitStat)
				{
					case "AttackSpeed":
						switch (op)
						{
							case ">": if (regiment.attackSpeed > value) { regimentsToGet.Add(regiment); } break;
							case "<": if (regiment.attackSpeed < value) { regimentsToGet.Add(regiment); } break;
							case "==": if (regiment.attackSpeed == value) { regimentsToGet.Add(regiment); } break;
							case ">=": if (regiment.attackSpeed >= value) { regimentsToGet.Add(regiment); } break;
							case "<=": if (regiment.attackSpeed <= value) { regimentsToGet.Add(regiment); } break;
						} break;
					case "Damage":
						switch (op)
						{
							case ">": if (regiment.damage > value) { regimentsToGet.Add(regiment); } break;
							case "<": if (regiment.damage < value) { regimentsToGet.Add(regiment); } break;
							case "==": if (regiment.damage == value) { regimentsToGet.Add(regiment); } break;
							case ">=": if (regiment.damage >= value) { regimentsToGet.Add(regiment); } break; ;
							case "<=": if (regiment.damage <= value) { regimentsToGet.Add(regiment); } break;
						} break;
					case "Distance":
						int distance = currentRegiment.GetDistanceTo(regiment);
						switch (op)
						{
							case ">": if (distance > value) { regimentsToGet.Add(regiment); } break;
							case "<": if (distance < value) { regimentsToGet.Add(regiment); } break;
							case "==": if (distance == value) { regimentsToGet.Add(regiment); } break;
							case ">=": if (distance >= value) { regimentsToGet.Add(regiment); } break;
							case "<=": if (distance <= value) { regimentsToGet.Add(regiment); } break;
						} break;
					case "Size":
						switch (op)
						{
							case ">": if (regiment.size > value) { regimentsToGet.Add(regiment); } break;
							case "<": if (regiment.size < value) { regimentsToGet.Add(regiment); } break;
							case "==": if (regiment.size == value) { regimentsToGet.Add(regiment); } break;
							case ">=": if (regiment.size >= value) { regimentsToGet.Add(regiment); } break;
							case "<=": if (regiment.size <= value) { regimentsToGet.Add(regiment); } break;
						} break;
					case "Range":
						switch (op)
						{
							case ">": if (regiment.range > value) { regimentsToGet.Add(regiment); } break;
							case "<": if (regiment.range < value) { regimentsToGet.Add(regiment); } break;
							case "==": if (regiment.range == value) { regimentsToGet.Add(regiment); } break;
							case ">=": if (regiment.range >= value) { regimentsToGet.Add(regiment); } break;
							case "<=": if (regiment.range <= value) { regimentsToGet.Add(regiment); } break;
						} break;
					case "Health":
						switch (op)
						{
							case ">": if (regiment.range > value) { regimentsToGet.Add(regiment); } break;
							case "<": if (regiment.range < value) { regimentsToGet.Add(regiment); } break;
							case "==": if (regiment.range == value) { regimentsToGet.Add(regiment); } break;
							case ">=": if (regiment.range >= value) { regimentsToGet.Add(regiment); } break;
							case "<=": if (regiment.range <= value) { regimentsToGet.Add(regiment); } break;
						} break;
				}
				
			}
			return regimentsToGet;
		}

		private Value CheckBinaryExpression(string op, Value v1, Value v2)
		{
			#region Int expressions
			if (v1 is IntValue)
			{
				int i1 = ((IntValue)v1).i;
				int i2 = ((IntValue)v2).i;
				BoolValue b = new BoolValue();
				IntValue i = new IntValue();
				switch (op)
				{
					case "<": if (i1 < i2) { b.b = true; } else { b.b = false; } return b;
					case ">": if (i1 > i2) { b.b = true; } else { b.b = false; } return b;
					case "==": if (i1 == i2) { b.b = true; } else { b.b = false; } return b;
					case ">=": if (i1 >= i2) { b.b = true; } else { b.b = false; } return b;
					case "<=": if (i1 <= i2) { b.b = true; } else { b.b = false; } return b;

					case "+": i.i = i1 + i2; return i;
					case "-": i.i = i1 - i2; return i;
					case "/": i.i = i1 / i2; return i;
					case "*": i.i = i1 * i2; return i;
				}
			}
			#endregion
			#region Bool expressions
			else if (v1 is BoolValue)
			{
				bool b1 = ((BoolValue)v1).b;
				bool b2 = ((BoolValue)v2).b;
				BoolValue b = new BoolValue();
				switch (op)
				{
					case "&&": if (b1 && b2) { b.b = true; } else { b.b = false; } return b;
					case "||": if (b1 && b2) { b.b = true; } else { b.b = false; } return b;
				}
			}
			#endregion
			#region Type expressions
			else if (v1 is TypeValue)
			{
				Regiment.AttackType t1 = ((TypeValue)v1).t;
				Regiment.AttackType t2 = ((TypeValue)v2).t;
				BoolValue b = new BoolValue();
				switch (op)
				{
					case "==": if (t1 == t2) { b.b = true; } else { b.b = false; } return b;
					case "!=": if (t1 != t2) { b.b = true; } else { b.b = false; } return b;
				}
			}
			#endregion
			Console.WriteLine("Found a undefined value - Maybe there is a problem");
			return new UndefinedValue();
		}
		#endregion

		#region Visitor Classes
		public Object VisitBehaviourBlock(BehaviourBlock ast, Object obj)
		{
			ast.bn.Visit(this, null);
			ast.sc.Visit(this, null);
			return null;
		}
		public Object VisitSequentialSingleCommand(SequentialSingleCommand ast, Object obj)
		{
			ast.S1.Visit(this, null);
			ast.S2.Visit(this, null);
			return null;
		}
		#region Control Structures
		public Object VisitIfCommand(IfCommand ast, Object obj)
		{
			BoolValue b = (BoolValue)ast.e.Visit(this, null);
			if (b.b)
			{
				ast.sc1.Visit(this, null);
			}
			else if (ast.eifc != null) { ast.eifc.ForEach(x => x.Visit(this, null)); }
			else if (ast.sc2 != null) { ast.sc2.Visit(this, null); }
			return null;
		}
		public Object VisitElseIfCommand(ElseIfCommand ast, Object obj)
		{
			BoolValue b = (BoolValue)ast.e.Visit(this, null);
			if (b.b)
			{
				ast.sc.Visit(this, null);
			}
			return null;
		}
		public Object VisitWhileCommand(WhileCommand ast, Object obj)
		{
			BoolValue b = (BoolValue)ast.e.Visit(this, null);
			while (b.b)
			{
				ast.sc.Visit(this, null);
				b = (BoolValue)ast.e.Visit(this, null);
			}
			return null;
		}
		#endregion
		#region Expressions
		public Object VisitBinaryExpression(BinaryExpression ast, Object obj)
		{
			Value v1 = (Value)ast.e1.Visit(this, null);
			Value v2 = (Value)ast.e2.Visit(this, null);
			string op = (string)ast.o.Visit(this, null);
			return CheckBinaryExpression(op, v1, v2);
		}
		public Object VisitIntegerExpression(IntegerExpression ast, Object obj)
		{
			IntValue i = new IntValue();
			i.i = Int32.Parse((string)ast.il.Visit(this,null));
			return i;
		}
		public Object VisitRegimentStatExpression(RegimentStatExpression ast, Object obj)
		{
			IntValue i = new IntValue();
			i.i = (int)ast.rs.Visit(this, null);
			return i;
		}

		//Vi har ikke engang unary expressions?
		public Object VisitUnaryExpression(UnaryExpression ast, Object obj)
		{
			return null;
		}
		public Object VisitUnitStatVNameExpression(UnitStatVNameExpression ast, Object obj)
		{
			IntValue i = new IntValue();
			string spelling = (string)ast.ust.Visit(this,null);
			switch (spelling)
			{
				case "Size": i.i = currentRegiment.size; break;
				case "Range": i.i = currentRegiment.range; break;
				case "Damage": i.i = currentRegiment.damage; break;
				case "Movement": i.i = currentRegiment.movement; break;
				case "AttackSpeed": i.i = currentRegiment.attackSpeed; break;
				case "Health": i.i = currentRegiment.health; break;
				case "Type": TypeValue t = new TypeValue(); t.t = currentRegiment.type; return t;
			}
			return i;
		}
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
			return Int32.Parse(ast.spelling);
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
		public Object VisitParameter(Parameter ast, Object obj)
		{
			ast.ust.Visit(this, null);
			ast.o.Visit(this, null);
			ast.il.Visit(this, null);
			return ast;
		}
		public Object VisitRegimentSearch(RegimentSearch ast, Object obj)
		{
			//Retrieves the type of regimentsearch we are doing
			string regimentSearchSpelling = (string)ast.rsn.Visit(this, null);

			//A list for storing all the regiments we find
			List<Regiment> regimentsFound = new List<Regiment>();
			if (regimentSearchSpelling == "SearchForFriends")
			{
				//Looks for regiments which are friends
				regimentsFound = currentGameState.teams[currentRegiment.team].regiments;
			}
			else
			{
				//Finds all the regiments which is not friendly
				foreach (Team team in currentGameState.teams)
				{
					if (team.number != currentRegiment.team)
					{
						regimentsFound.AddRange(team.regiments);
					}
				}
			}
			if (ast.p != null)
			{
				foreach (Parameter p in ast.p)
				{
					//Retrieves the parameter from the ast
					Parameter parameter = (Parameter)p.Visit(this, null);

					//Gets all the regiments which matches the parameter
					regimentsFound = GetRegiments(regimentsFound, parameter);
					if (regimentsFound.Count == 0) { break; }
				}
			}
			if (regimentsFound.Count == 0)
			{
				return null;
			}
			return regimentsFound[0];
		}
		public Object VisitRegimentSearchName(RegimentSearchName ast, Object obj)
		{
			return ast.spelling;
		}

		//Unit function
		public Object VisitUnitFunction(UnitFunction ast, Object obj)
		{
			string spelling = (string)ast.i.Visit(this, null);
			string functionName = (string)ast.ufn.Visit(this, null);
			Regiment regiment = GetRegiment(spelling);
			if (regiment != null)
			{
				switch (functionName)
				{
					case "Attack": currentRegiment.Attack(regiment); break;
					case "MoveTowards": currentRegiment.MoveTowards(regiment); break;
					case "MoveAway": currentRegiment.MoveAway(regiment); break;
				}
			}
			return null;
		}
		public Object VisitUnitFunctionName(UnitFunctionName ast, Object obj)
		{
			return ast.spelling;
		}

		//Regiment stat
		public Object VisitRegimentStat(RegimentStat ast, Object obj)
		{
			int value = 0;
			string identifier = (string)ast.i.Visit(this, null);
			string spelling = (string)ast.ust.Visit(this, null);
			Regiment regiment = GetRegiment(identifier);
			if (regiment != null)
			{
				switch (spelling)
				{
					case "Size": value = regiment.size; break;
					case "Distance": value = regiment.GetDistanceTo(currentRegiment); break;
					case "Range": value = regiment.range; break;
					case "AttackSpeed": value = regiment.attackSpeed; break;
					case "Health": value = regiment.health; break;
					case "Movement": value = regiment.health; break;
					case "Damage": value = regiment.damage; break;
				}
			}
			return value;
		}
		public Object VisitUnitStatType(UnitStatType ast, Object obj)
		{
			return ast.spelling;
		}
		#endregion

		#region Not used in BehaviourBlock
		#region Files
		public Object VisitTeamFile(TeamFile ast, Object obj)
		{
			ast.rbs.ForEach(x=>x.Visit(this, null));
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
		#endregion
	}
}
