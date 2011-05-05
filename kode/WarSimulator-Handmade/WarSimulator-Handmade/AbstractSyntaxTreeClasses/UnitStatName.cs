using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class UnitStatName:UnitStat
    {
        public UnitStatNameVariable sn;
        public IntegerLiteral il;

        public UnitStatName(UnitStatNameVariable sn, IntegerLiteral il)
        {
            this.sn = sn;
            this.il = il;
        }
    }
}
