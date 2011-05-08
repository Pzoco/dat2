using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public enum DataType { Boolean, Integer, AttackType, Position, Regiment}
    public class Checker:Visitor
    {
        IdentificationTable idTable = new IdentificationTable();
        ErrorReporter reporter = new ErrorReporter();
        public void Check(TeamFile ast)
        {
            ast.Visit(this, null);
        }
        public void Check(ConfigFile ast)
        {
            ast.Visit(this, null);
        }

        #region Blocks
        public Object VisitBehaviourBlock(BehaviourBlock ast, Object obj) 
        {
            idTable.Open();
            ast.bn.Visit(this, null);
            ast.scs.ForEach(x => x.Visit(this,null));
            idTable.Close();
            return null; 
        }
        public Object VisitGridBlock(GridBlock ast, Object obj)
        {
            idTable.Open();
            ast.bn.Visit(this, null);
            ast.gss.ForEach(x=> x.Visit(this, null));
            idTable.Close();
            return null;
        }
        public Object VisitMaximumsBlock(MaximumsBlock ast, Object obj)
        {
            idTable.Open();
            ast.msds.ForEach(x=>x.Visit(this, null));
            idTable.Close();
            return null;
        }
        public Object VisitRegimentBlock(RegimentBlock ast, Object obj)
        {
            idTable.Open();
            ast.bn.Visit(this, null);
            ast.usds.ForEach(x=> x.Visit(this, null));
            ast.bb.Visit(this, null);
            idTable.Close();
            return null;
        }
        public Object VisitRulesBlock(RulesBlock ast, Object obj)
        {
            idTable.Open();
            ast.mb.Visit(this,null);
            ast.sb.Visit(this,null);
            idTable.Close();
            return null;
        }
        public Object VisitStandardsBlock(StandardsBlock ast, Object obj)
        {
            idTable.Open();
            ast.bb.Visit(this, null);
            ast.usds.ForEach(x=>x.Visit(this, null));
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
                reporter.ReportError("Expression was not of type Boolean", "", ast.e.position);
            }
            ast.sc1.Visit(this,null);
            if(ast.eifc != null){ast.eifc.ForEach(x => x.Visit(this,null));}
            if (ast.sc2 != null) { ast.sc2.Visit(this, null); }
            return null;
        }
        public Object VisitElseIfCommand(ElseIfCommand ast, Object obj)
        {
            DataType type = (DataType)ast.e.Visit(this, null);
            if (type != DataType.Boolean)
            {
                reporter.ReportError("Expression was not of type Boolean", "", ast.e.position);
            }
            ast.sc.Visit(this, null);
            return null;
        }
        public Object VisitWhileCommand(WhileCommand ast, Object obj)
        {
            DataType type = (DataType)ast.e.Visit(this, null);
            if (type != DataType.Boolean)
            {
                reporter.ReportError("Expression was not of type Boolean", "", ast.e.position);
            }
            ast.sc.Visit(this, null);
            return null;
        }
        #endregion

        #region Expressions
        public Object VisitBinaryExpression(BinaryExpression ast, Object obj)
        {
            DataType eType1 = (DataType) ast.e1.Visit(this,null);
            DataType eType2 = (DataType) ast.e2.Visit(this,null);
            Declaration declaration = (Declaration) ast.o.Visit(this, null);
            if (declaration != null)
            {
                if (!(declaration is BinaryOperatorDeclaration))
                {
                    reporter.ReportError("Operator was not a binary operator declaration", "", ast.o.position);
                }
                BinaryOperatorDeclaration bod = (BinaryOperatorDeclaration)declaration;
                if (bod.arg1 != bod.arg2)
                {
                    reporter.ReportError("Datatype of argument 1 doesn't match datatype of argument 2", "", ast.position);
                }
                else if (eType1 != bod.arg1)
                {
                    reporter.ReportError("Wrong argument given at arg1", "", ast.position);
                }
                else if (eType2 != bod.arg2)
                {
                    reporter.ReportError("Wrong argument given at arg2", "", ast.position);
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
            ast.type = DataType.Regiment;
            return ast.type;
        }
        public Object VisitUnaryExpression(UnaryExpression ast, Object obj)
        {
            return null;
        }
        public Object VisitUnitStatVNameExpression(UnitStatVNameExpression ast, Object obj)
        {
            ast.type = (DataType) ast.usn.Visit(this, null);
            return ast.type;
        }
        #endregion

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

        #region Identifiers etc
        public Object VisitAttackType(AttackType ast, Object obj)
        {
            return DataType.AttackType;
        }
        public Object VisitBlockName(BlockName ast, Object obj)
        {
            return null;
        }
        public Object VisitIdentifier(Identifier ast, Object obj)
        {
            return null;
        }
        public Object VisitIntegerLiteral(IntegerLiteral ast, Object obj)
        {
            return null;
        }
        public Object VisitOperator(Operator ast, Object obj)
        {
            return null;
        }
        #endregion

        #region Misc
        public Object VisitBehaviourAssignment(BehaviourAssignment ast, Object obj)
        {
            return null;
        }
        #endregion

        #region Regiment assignment related
        //Regiment Assignment
        public Object VisitRegimentDeclaration(RegimentDeclaration ast, Object obj)
        {
            return null;
        }

        //Regiment Search
        public Object VisitBinaryParameter(BinaryParameter ast, Object obj)
        {
            return null;
        }
        public Object VisitParameter(Parameter ast, Object obj)
        {
            return null;
        }
        public Object VisitParameters(Parameters ast, Object obj)
        {
            return null;
        }
        public Object VisitRegimentSearch(RegimentSearch ast, Object obj)
        {
            return null;
        }

        //Unit function
        public Object VisitUnitFunction(UnitFunction ast, Object obj)
        {
            return null;
        }
        public Object VisitUnitFunctionName(UnitFunctionName ast, Object obj)
        {
            return null;
        }

        //Regiment stat
        public Object VisitRegimentStat(RegimentStat ast, Object obj)
        {
            return null;
        }
        public Object VisitUnitStatType(UnitStatType ast, Object obj)
        {
            return null;
        }
        #endregion

        #region Stats

        public Object VisitGridStatDeclaration(GridStatDeclaration ast, Object obj)
        {
            return null;
        }
        public Object VisitGridStatVName(GridStatVName ast, Object obj)
        {
            return null;
        }
        public Object VisitMaximumsStatDeclaration(MaximumsStatDeclaration ast, Object obj)
        {
            return null;
        }
        public Object VisitMaximumsStatVName(MaximumsStatVName ast, Object obj)
        {
            return null;
        }
        public Object VisitUnitStatDeclaration(UnitStatDeclaration ast, Object obj)
        {
            return null;
        }
        public Object VisitUnitStatPositionDeclaration(UnitStatPositionDeclaration ast, Object obj)
        {
            return null;
        }
        public Object VisitUnitStatTypeDeclaration(UnitStatTypeDeclaration ast, Object obj)
        {
            return null;
        }
        public Object VisitUnitStatVName(UnitStatVName ast, Object obj)
        {
            return null;
        }
        #endregion

        public BinaryOperatorDeclaration DeclareStandardBinaryOperator(DataType arg1,DataType arg2, string op,DataType result)
        {
            BinaryOperatorDeclaration declaration;
            declaration = new BinaryOperatorDeclaration(arg1, arg2, new Operator(op, null), result);
            idTable.EnterEntry(declaration, op);
            return declaration;
        }


        public Object VisitBinaryOperatorDeclaration(BinaryOperatorDeclaration ast, Object obj)
        {
            return null;
        }

        private void EstablishStandardEnviroment()
        {
            DeclareStandardBinaryOperator(DataType.Integer, DataType.Integer, ">", DataType.Boolean);
            DeclareStandardBinaryOperator(DataType.Integer, DataType.Integer, "<", DataType.Boolean);
            DeclareStandardBinaryOperator(DataType.Integer, DataType.Integer, ">=", DataType.Boolean);
            DeclareStandardBinaryOperator(DataType.Integer, DataType.Integer, "<=", DataType.Boolean);
            DeclareStandardBinaryOperator(DataType.Integer, DataType.Integer, "==", DataType.Boolean);
            DeclareStandardBinaryOperator(DataType.Boolean, DataType.Boolean, "||", DataType.Boolean);
            DeclareStandardBinaryOperator(DataType.Boolean, DataType.Boolean, "&&", DataType.Boolean);
        }
    }
}
