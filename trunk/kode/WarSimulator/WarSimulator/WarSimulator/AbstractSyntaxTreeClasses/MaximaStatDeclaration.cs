using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class MaximaStatDeclaration:Declaration
    {
        public MaximaStatVName msv;
        public IntegerLiteral il;

        public MaximaStatDeclaration(MaximaStatVName msv, IntegerLiteral il)
        {
            this.msv = msv;
            this.il = il;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitMaximaStatDeclaration(this, arg);
        }
    }
}
