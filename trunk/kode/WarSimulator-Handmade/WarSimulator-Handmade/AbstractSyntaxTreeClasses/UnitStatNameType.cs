using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class UnitStatNameType:UnitStat
    {
        public UnitStatNameVariable sn;
        public AttackType at;

        public UnitStatNameType(UnitStatNameVariable sn, AttackType at)
        {
            this.sn = sn;
            this.at = at;
        }
    }
}
