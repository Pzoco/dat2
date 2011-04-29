using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class BinaryUnitStatName:UnitStat
    {
        public UnitStat usn1, usn2;

        public BinaryUnitStatName(UnitStat usn1, UnitStat usn2)
        {
            this.usn1 = usn1;
            this.usn2 = usn2;

        }
    }
}
