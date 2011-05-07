using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public interface Visitor
    {
        //Control Structures
        Object VisitIfCommand(IfCommand ast, Object obj);
        Object VisitElseIfCommand(ElseIfCommand ast, Object obj);
        Object VisitWhileCommand(WhileCommand ast, Object obj);

        //Expressions
        Object VisitBinaryExpression(BinaryExpression ast,Object obj);
        Object VisitIntegerExpression(IntegerExpression ast, Object obj);
        Object VisitRegimentStatExpression(RegimentStat ast, Object obj);
        Object VisitUnaryExpression(UnaryExpression ast, Object obj);
    }
}
