using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class UnitStatIntegerDeclaration:UnitStatDeclaration
    {
        public UnitStatVName sn;
        public IntegerLiteral il;

        public UnitStatIntegerDeclaration(UnitStatVName sn, IntegerLiteral il)
        {
            this.sn = sn;
            this.il = il;
        }
        public override Object Visit(Visitor v, Object arg)
        {
			return v.VisitUnitStatIntegerDeclaration(this, arg);
        }
    }
}
