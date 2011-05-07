using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class UnitStatTypeDeclaration:UnitStatDeclaration
    {
        public UnitStatNameVariable sn;
        public AttackType at;

        public UnitStatTypeDeclaration(UnitStatNameVariable sn, AttackType at)
        {
            this.sn = sn;
            this.at = at;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitUnitStatNameType(this, arg);
        }
    }
}
