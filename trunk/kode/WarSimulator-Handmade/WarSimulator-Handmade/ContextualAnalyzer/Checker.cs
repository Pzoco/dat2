using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class Checker:Visitor
    {
        IdentificationTable idTable = new IdentificationTable();

        #region Blocks
        public Object VisitBehaviourBlock(BehaviourBlock ast, Object obj) 
        { 
            return null; 
        }
        Object VisitGridBlock(GridBlock ast, Object obj);
        Object VisitMaximumsBlock(MaximumsBlock ast, Object obj);
        Object VisitRegimentBlock(RegimentBlock ast, Object obj);
        Object VisitRulesBlock(RulesBlock ast, Object obj);
        Object VisitStandardsBlock(StandardsBlock ast, Object obj);
        #endregion

        #region Control Structures
        Object VisitControlStructure(ControlStructure ast, Object obj);
        Object VisitIfCommand(IfCommand ast, Object obj);
        Object VisitElseIfCommand(ElseIfCommand ast, Object obj);
        Object VisitWhileCommand(WhileCommand ast, Object obj);
        #endregion

        #region Expressions
        Object VisitBinaryExpression(BinaryExpression ast, Object obj);
        Object VisitExpression(Expression ast, Object obj);
        Object VisitIntegerExpression(IntegerExpression ast, Object obj);
        Object VisitRegimentStatExpression(RegimentStat ast, Object obj);
        Object VisitUnaryExpression(UnaryExpression ast, Object obj);
        #endregion

        #region Files
        Object VisitTeamFile(TeamFile ast, Object obj);
        Object VisitConfigFile(ConfigFile ast, Object obj);
        #endregion

        #region Identifiers etc
        Object VisitAttackType(AttackType ast, Object obj);
        Object VisitBlockName(BlockName ast, Object obj);
        Object VisitIdentifier(Identifier ast, Object obj);
        Object VisitIntegerLiteral(IntegerLiteral ast, Object obj);
        Object VisitOperator(Operator ast, Object obj);
        #endregion

        #region Misc
        Object VisitSingleCommand(SingleCommand ast, Object obj);
        Object VisitBehaviourAssignment(BehaviourAssignment ast, Object obj);
        #endregion

        #region Regiment assignment related
        //Regiment Assignment
        Object VisitRegimentAssignment(RegimentAssignment ast, Object obj);

        //Regiment Search
        Object VisitBinaryParameter(BinaryParameter ast, Object obj);
        Object VisitParameter(Parameter ast, Object obj);
        Object VisitParameters(Parameters ast, Object obj);
        Object VisitRegimentSearch(RegimentSearch ast, Object obj);

        //Unit function
        Object VisitUnitFunction(UnitFunction ast, Object obj);
        Object VisitUnitFunctionName(UnitFunctionName ast, Object obj);

        //Regiment stat
        Object VisitRegimentStat(RegimentStat ast, Object obj);
        Object VisitUnitStatType(UnitStatType ast, Object obj);
        #endregion

        #region Stats
        Object VisitBinaryGridStatName(BinaryGridStatName ast, Object obj);
        Object VisitBinaryMaximumsStatName(BinaryMaximumsStatName ast, Object obj);
        Object VisitBinaryUnitStatName(BinaryUnitStatName ast, Object obj);
        Object VisitGridStat(GridStat ast, Object obj);
        Object VisitGridStatName(GridStatName ast, Object obj);
        Object VisitGridStatNameVariable(GridStatNameVariable ast, Object obj);
        Object VisitMaximumsStat(MaximumsStat ast, Object obj);
        Object VisitMaximumsStatName(MaximumsStatName ast, Object obj);
        Object VisitMaximumsStatNameVariable(MaximumsStatNameVariable ast, Object obj);
        Object VisitUnitStat(UnitStat ast, Object obj);
        Object VisitUnitStatName(UnitStatName ast, Object obj);
        Object VisitUnitStatNamePosition(UnitStatNamePosition ast, Object obj);
        Object VisitUnitStatNameType(UnitStatNameType ast, Object obj);
        Object VisitUnitStatNameVariable(UnitStatNameVariable ast, Object obj);
        #endregion
    }
}
