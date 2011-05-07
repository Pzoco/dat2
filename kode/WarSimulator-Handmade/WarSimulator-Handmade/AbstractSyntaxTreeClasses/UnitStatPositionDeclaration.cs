using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class UnitStatPositionDeclaration:UnitStatDeclaration
    {
        public UnitStatVName sn;
        public IntegerLiteral x,y;

        public UnitStatPositionDeclaration(UnitStatVName sn, IntegerLiteral x, IntegerLiteral y)
        {
            this.sn = sn;
            this.x = x;
            this.y = y;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitUnitStatPositionDeclaration(this, arg);
        }
    }
}
