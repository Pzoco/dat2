using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class UnitStatNameExpression:Expression
    {
        public UnitStatName usn;

        public UnitStatNameExpression(UnitStatName usn)
        {
            this.usn = usn;
        }

    }
}
