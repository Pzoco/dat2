using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class MaximumsStatName:MaximumsStat
    {
        public MaximumsStatNameVariable msv;
        public IntegerLiteral il;

        public MaximumsStatName(MaximumsStatNameVariable msv, IntegerLiteral il)
        {
            this.msv = msv;
            this.il = il;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitMaximumsStatName(this, arg);
        }
    }
}
