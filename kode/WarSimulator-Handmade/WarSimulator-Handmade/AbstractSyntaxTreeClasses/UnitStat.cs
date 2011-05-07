using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class UnitStat:AST
    {
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitUnitStat(this, arg);
        }
    }
}
