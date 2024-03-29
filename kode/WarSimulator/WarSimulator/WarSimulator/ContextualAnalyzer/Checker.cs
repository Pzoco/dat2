﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public enum DataType { Boolean, Integer, AttackType, Position, Regiment }
    public class Checker : Visitor
    {
        private IdentificationTable idTable = new IdentificationTable();
        private ErrorReporter reporter = new ErrorReporter();
		private TeamFile teamFile;
		private ConfigFile configFile;

		#region Constructors
		public Checker(TeamFile ast,ErrorReporter reporter)
        {
            this.reporter = reporter;
            EstablishStandardEnviroment();
			teamFile = ast;
        }
        public Checker(ConfigFile ast, ErrorReporter reporter)
        {
			this.reporter = reporter;
            EstablishStandardEnviroment();
			configFile = ast;
        }
		#endregion

		public TeamFile CheckTeamFile()
		{
			TeamFile dast = (TeamFile)teamFile.Visit(this, null);
			if (reporter.numbErrors > 0)
			{
				Console.WriteLine("Contextual analyzing failed - Found {0} errors", reporter.numbErrors);
			}
			else
			{
				Console.WriteLine("Contextual analyzing success");
			}
			return dast;
		}
		public ConfigFile CheckConfigFile()
		{
			ConfigFile dast = (ConfigFile)configFile.Visit(this, null);
			if (reporter.numbErrors > 0)
			{
				Console.WriteLine("Contextual analyzing failed - Found {0} errors", reporter.numbErrors);
			}
			else
			{
				Console.WriteLine("Contextual analyzing success");
			}
			return dast;
		}

		#region Visitor methods
		#region Blocks
		public Object VisitBehaviourBlock(BehaviourBlock ast, Object obj)
        {
            idTable.Open();
            ast.bn.Visit(this, null);
            ast.sc.Visit(this, null);
            idTable.Close();
            return null;
        }

        public Object VisitGridBlock(GridBlock ast, Object obj)
        {
            idTable.Open();
            ast.bn.Visit(this, null);
            ast.gss.ForEach(x => x.Visit(this, null));
            idTable.Close();
            return null;
        }
        public Object VisitMaximaBlock(MaximaBlock ast, Object obj)
        {
            idTable.Open();
            ast.msds.ForEach(x => x.Visit(this, null));
            idTable.Close();
            return null;
        }
        public Object VisitRegimentBlock(RegimentBlock ast, Object obj)
        {
            idTable.Open();
            ast.bn.Visit(this, null);
            ast.usds.ForEach(x => x.Visit(this, null));
            ast.bb.Visit(this, null);
            idTable.Close();
            return null;
        }
        public Object VisitRulesBlock(RulesBlock ast, Object obj)
        {
            idTable.Open();
            ast.mb.Visit(this, null);
            ast.sb.Visit(this, null);
            idTable.Close();
            return null;
        }
        public Object VisitStandardsBlock(StandardsBlock ast, Object obj)
        {
            idTable.Open();
            ast.bb.Visit(this, null);
            ast.usds.ForEach(x => x.Visit(this, null));
            idTable.Close();
            return null;
        }
        #endregion

        #region Control Structures
        public Object VisitIfCommand(IfCommand ast, Object obj)
        {
            DataType type = (DataType)ast.e.Visit(this, null);
            if (type != DataType.Boolean)
            {
                reporter.ReportCheckerError("Expression was not of type Boolean", "", ast.e.position);
            }
            ast.sc1.Visit(this, null);
            if (ast.eifc != null) { ast.eifc.ForEach(x => x.Visit(this, null)); }
            if (ast.sc2 != null) { ast.sc2.Visit(this, null); }
            return null;
        }
        public Object VisitElseIfCommand(ElseIfCommand ast, Object obj)
        {
            DataType type = (DataType)ast.e.Visit(this, null);
            if (type != DataType.Boolean)
            {
                reporter.ReportCheckerError("Expression was not of type Boolean", "", ast.e.position);
            }
            ast.sc.Visit(this, null);
            return null;
        }
        public Object VisitWhileCommand(WhileCommand ast, Object obj)
        {
            DataType type = (DataType)ast.e.Visit(this, null);
            if (type != DataType.Boolean)
            {
                reporter.ReportCheckerError("Expression was not of type Boolean", "", ast.e.position);
            }
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
                if (!(declaration is BinaryOperatorDeclaration))
                {
                    reporter.ReportCheckerError("Operator was not a binary operator declaration", "", ast.o.position);
                }
                BinaryOperatorDeclaration bod = (BinaryOperatorDeclaration)declaration;
                if (bod.arg1 != bod.arg2)
                {
                    reporter.ReportCheckerError("Datatype of argument 1 doesn't match datatype of argument 2", "", ast.position);
                }
                else if (eType1 != bod.arg1)
                {
                    reporter.ReportCheckerError("Wrong argument given at arg1", "", ast.position);
                }
                else if (eType2 != bod.arg2)
                {
                    reporter.ReportCheckerError("Wrong argument given at arg2", "", ast.position);
                }
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
            Declaration declaration = idTable.RetrieveEntry(ast.spelling);
            if (declaration != null)
            {
                ast.declaration = declaration;
            }
            return declaration;
        }
        public Object VisitIntegerLiteral(IntegerLiteral ast, Object obj)
        {
            return DataType.Integer;
        }
        public Object VisitOperator(Operator ast, Object obj)
        {
            Declaration declaration = idTable.RetrieveEntry(ast.spelling);
            if (declaration != null)
            {
                ast.declaration = declaration;
            }
            return declaration;
        }
        #endregion

        #region Misc
        //Hvad skal vi gøre med denne?
        public Object VisitBehaviourAssignment(BehaviourAssignment ast, Object obj)
        {
            return null;
        }
		public Object VisitSequentialSingleCommand(SequentialSingleCommand ast, Object obj)
		{
			ast.S1.Visit(this, null);
			ast.S2.Visit(this, null);
			return null;
		}
        #endregion

        #region Regiment assignment related
        //Regiment Assignment
        public Object VisitRegimentDeclaration(RegimentDeclaration ast, Object obj)
        {
            ast.rs.Visit(this, null);
            Declaration declaration = (Declaration)ast.i.Visit(this, null);
            if (declaration != null)
            {
                reporter.ReportCheckerError("Tried to declare % which was already declared", ast.i.spelling, ast.position);
            }
            idTable.EnterEntry(ast, ast.i.spelling);
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
                if (!(declaration is BinaryOperatorDeclaration))
                {
                    reporter.ReportCheckerError("Operator was not a binary operator declaration", "", ast.o.position);
                }
                BinaryOperatorDeclaration bod = (BinaryOperatorDeclaration)declaration;
                if (bod.arg1 != bod.arg2)
                {
                    reporter.ReportCheckerError("Datatype of argument 1 doesn't match datatype of argument 2", "", ast.position);
                }
                else if (eType1 != bod.arg1)
                {
                    reporter.ReportCheckerError("Wrong argument given at arg1", "", ast.position);
                }
                else if (eType2 != bod.arg2)
                {
                    reporter.ReportCheckerError("Wrong argument given at arg2", "", ast.position);
                }
            }
            return null;
        }

        public Object VisitRegimentSearch(RegimentSearch ast, Object obj)
        {
            ast.rsn.Visit(this, null);
			if (ast.p != null)
			{
				ast.p.ForEach(x => x.Visit(this, null));
			}
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
            if (declaration == null)
            {
                reporter.ReportCheckerError("Regiment % was not declared", ast.i.spelling, ast.i.position);
            }
			else if (!(declaration is RegimentDeclaration))
            {
                reporter.ReportCheckerError("% was not of type Regiment", ast.i.spelling, ast.i.position);
            }
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
            Declaration declaration = idTable.RetrieveEntry(ast.i.spelling);
            if (declaration == null)
            {
                reporter.ReportCheckerError("Regiment % wasn't declared", ast.i.spelling, ast.i.position);
            }
            else if (ast.i.type != DataType.Regiment)
            {
                reporter.ReportCheckerError("% was not of type Regiment", ast.i.spelling, ast.i.position);
            }
            ast.ust.Visit(this, null);
            return null;
        }
        public Object VisitUnitStatType(UnitStatType ast, Object obj)
        {
            return DataType.Integer;
        }
        #endregion

        #region Stats

        public Object VisitGridStatDeclaration(GridStatDeclaration ast, Object obj)
        {
            if (!(idTable.EnterEntry(ast, ast.gsnv.spelling)))
            {
                reporter.ReportCheckerError("Tried to declare % which was already declared", ast.gsnv.spelling, ast.position);
            }
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
            if (!(idTable.EnterEntry(ast, ast.msv.spelling)))
            {
                reporter.ReportCheckerError("Tried to declare % which already was declared", ast.msv.spelling, ast.position);
            }
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
            if (snType != DataType.Integer)
            {
                reporter.ReportCheckerError("Tried to assign an Integer to %", ast.sn.spelling, ast.position);
            }
            if (!(idTable.EnterEntry(ast, ast.sn.spelling)))
            {
                reporter.ReportCheckerError("Tried to declare % which already was declared", ast.sn.spelling, ast.position);
            }
            return null;
        }
        public Object VisitUnitStatPositionDeclaration(UnitStatPositionDeclaration ast, Object obj)
        {
            DataType snType = (DataType)ast.sn.Visit(this, null);

            if (snType != DataType.Position)
            {
                reporter.ReportCheckerError("Tried to assign a position to %", ast.sn.spelling, ast.position);
            }
            if (!(idTable.EnterEntry(ast, ast.sn.spelling)))
            {
                reporter.ReportCheckerError("Tried to declare % which already was declared", ast.sn.spelling, ast.position);
            }
            return null;
        }
        public Object VisitUnitStatTypeDeclaration(UnitStatTypeDeclaration ast, Object obj)
        {
            DataType snType = (DataType)ast.sn.Visit(this, null);
            if (snType != DataType.AttackType)
            {
                reporter.ReportCheckerError("Tried to assign an attacktype to %", ast.sn.spelling, ast.position);
            }
            if (!(idTable.EnterEntry(ast, ast.sn.spelling)))
            {
                reporter.ReportCheckerError("Tried to declare % which already was declared", ast.sn.spelling, ast.position);
            }
            return null;
        }
        public Object VisitUnitStatVName(UnitStatVName ast, Object obj)
        {
            if (ast.spelling == "Size" || ast.spelling == "Range" ||
                ast.spelling == "Damage" || ast.spelling == "Movement" || 
				ast.spelling == "AttackSpeed" || ast.spelling == "Health")
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

        public BinaryOperatorDeclaration DeclareStandardBinaryOperator(DataType arg1, DataType arg2, string op, DataType result)
        {
            BinaryOperatorDeclaration declaration;
            declaration = new BinaryOperatorDeclaration(arg1, arg2, new Operator(op, null), result);
            idTable.EnterEntry(declaration, op);
            return declaration;
        }

        //Not used for anything
        public Object VisitBinaryOperatorDeclaration(BinaryOperatorDeclaration ast, Object obj)
        {
            return null;
        }

        //Saves all the legal types of binary expression that can be made
        private void EstablishStandardEnviroment()
        {
            DeclareStandardBinaryOperator(DataType.Integer, DataType.Integer, ">", DataType.Boolean);
            DeclareStandardBinaryOperator(DataType.Integer, DataType.Integer, "<", DataType.Boolean);
            DeclareStandardBinaryOperator(DataType.Integer, DataType.Integer, ">=", DataType.Boolean);
            DeclareStandardBinaryOperator(DataType.Integer, DataType.Integer, "<=", DataType.Boolean);
            DeclareStandardBinaryOperator(DataType.Integer, DataType.Integer, "==", DataType.Boolean);

			DeclareStandardBinaryOperator(DataType.Integer, DataType.Integer, "+", DataType.Integer);
			DeclareStandardBinaryOperator(DataType.Integer, DataType.Integer, "-", DataType.Integer);
			DeclareStandardBinaryOperator(DataType.Integer, DataType.Integer, "/", DataType.Integer);
			DeclareStandardBinaryOperator(DataType.Integer, DataType.Integer, "*", DataType.Integer);
		}
		#endregion
	}
}
