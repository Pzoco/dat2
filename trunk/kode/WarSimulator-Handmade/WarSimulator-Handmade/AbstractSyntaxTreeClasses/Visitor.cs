using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public interface Visitor
    {
        #region Blocks
        Object VisitBehaviourBlock(Regiment ast, Object obj);
        Object VisitGridBlock(GridBlock ast, Object obj);
        Object VisitMaximaBlock(MaximaBlock ast, Object obj);
        Object VisitRegimentBlock(RegimentBlock ast, Object obj);
        Object VisitRulesBlock(RulesBlock ast, Object obj);
        Object VisitStandardsBlock(StandardsBlock ast, Object obj);
        #endregion

        #region Control Structures
        Object VisitIfCommand(IfCommand ast, Object obj);
        Object VisitElseIfCommand(ElseIfCommand ast, Object obj);
        Object VisitWhileCommand(WhileCommand ast, Object obj);
        #endregion

        #region Expressions
        Object VisitBinaryExpression(BinaryExpression ast, Object obj);
        Object VisitIntegerExpression(IntegerExpression ast, Object obj);
        Object VisitRegimentStatExpression(RegimentStatExpression ast, Object obj);
        Object VisitUnaryExpression(UnaryExpression ast, Object obj);
        Object VisitUnitStatVNameExpression(UnitStatVNameExpression ast, Object obj);
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
        Object VisitBehaviourAssignment(BehaviourAssignment ast, Object obj);
        #endregion

        #region Regiment assignment related
        //Regiment Assignment
        Object VisitRegimentDeclaration(RegimentDeclaration ast, Object obj);
        Object VisitRegimentDeclarationCommand(RegimentDeclarationCommand ast, Object obj);

        //Regiment Search
        Object VisitBinaryParameter(BinaryParameter ast, Object obj);
        Object VisitParameter(Parameter ast, Object obj);
        Object VisitRegimentSearch(RegimentSearch ast, Object obj);
        Object VisitRegimentSearchName(RegimentSearchName ast, Object obj);

        //Unit function
        Object VisitUnitFunction(UnitFunction ast, Object obj);
        Object VisitUnitFunctionName(UnitFunctionName ast, Object obj);

        //Regiment stat
        Object VisitRegimentStat(RegimentStat ast, Object obj);
        Object VisitUnitStatType(UnitStatType ast, Object obj);
        #endregion

        #region Stats
        Object VisitGridStatDeclaration(GridStatDeclaration ast, Object obj);
        Object VisitGridStatVName(GridStatVName ast, Object obj);
        Object VisitMaximaStatDeclaration(MaximaStatDeclaration ast, Object obj);
        Object VisitMaximaStatVName(MaximaStatVName ast, Object obj);
        Object VisitUnitStatIntegerDeclaration(UnitStatIntegerDeclaration ast, Object obj);
        Object VisitUnitStatPositionDeclaration(UnitStatPositionDeclaration ast, Object obj);
        Object VisitUnitStatTypeDeclaration(UnitStatTypeDeclaration ast, Object obj);
        Object VisitUnitStatVName(UnitStatVName ast, Object obj);
        #endregion

        Object VisitBinaryOperatorDeclaration(BinaryOperatorDeclaration ast, Object obj);
    }
}
