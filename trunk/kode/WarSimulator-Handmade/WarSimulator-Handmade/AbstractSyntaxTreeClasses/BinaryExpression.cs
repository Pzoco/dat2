using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class BinaryExpression : Expression
    {
        public Expression e1, e2;
        public Operator o;

        public BinaryExpression(Expression e1, Expression e2, Operator o)
        {
            this.e1 = e1;
            this.e2 = e2;
            this.o = o;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitBinaryExpression(this, arg);
        }
    }
}
