using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class IntegerExpression:Expression
    {
        public IntegerLiteral il;

        public IntegerExpression(IntegerLiteral il)
        {
            this.il = il;
        }

    }
}
