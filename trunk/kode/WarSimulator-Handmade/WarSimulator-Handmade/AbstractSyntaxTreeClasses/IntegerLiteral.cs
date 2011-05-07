using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class IntegerLiteral:Terminal
    {
        public IntegerLiteral(string spelling):base(spelling)
        {

        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitIntegerLiteral(this, arg);
        }
    }
}
