using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class UnaryExpression : Expression
    {
        public Expression e;
        public Operator o;

        public UnaryExpression(Operator o, Expression e)
        {
            this.o = o;
            this.e = e;
        }
    }
}
