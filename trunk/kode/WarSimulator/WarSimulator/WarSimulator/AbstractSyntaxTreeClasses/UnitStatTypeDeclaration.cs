using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class UnitStatTypeDeclaration:UnitStatDeclaration
    {
        public UnitStatVName sn;
        public AttackType at;

        public UnitStatTypeDeclaration(UnitStatVName sn, AttackType at)
        {
            this.sn = sn;
            this.at = at;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitUnitStatTypeDeclaration(this, arg);
        }
    }
}
