
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade.Simulation
{
	class GameDataRetriever:Visitor
	{
		private Grid grid;
		private List<Team> teamList = new List<Team>();
		public GameDataRetriever() 
		{
		}
		public GameState Retrieve(ConfigFile configFile, TeamFile[] teamFiles)
		{
			foreach (TeamFile teamfile in teamFiles)
			{
				teamfile.Visit(this, null);	
			}
			configFile.Visit(this, null);
			return null;
		}


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
			return null;
		}
		#endregion
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

		#region //Not used for anything
		public Object VisitBinaryOperatorDeclaration(BinaryOperatorDeclaration ast, Object obj)
		{
			return null;
		}
		#endregion


		#endregion

	}
}
