using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class MaximumsStatDeclaration:Declaration
    {
        public MaximumsStatVName msv;
        public IntegerLiteral il;

        public MaximumsStatDeclaration(MaximumsStatVName msv, IntegerLiteral il)
        {
            this.msv = msv;
            this.il = il;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitMaximumsStatDeclaration(this, arg);
        }
    }
}
