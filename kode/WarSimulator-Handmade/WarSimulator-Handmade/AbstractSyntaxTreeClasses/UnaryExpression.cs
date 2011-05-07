using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class UnaryExpression : Expression
    {
        public Expression e;
        public Operator o;

        public UnaryExpression(Operator o, Expression e)
        {
            this.o = o;
            this.e = e;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitUnaryExpression(this, arg);
        }
    }
}
