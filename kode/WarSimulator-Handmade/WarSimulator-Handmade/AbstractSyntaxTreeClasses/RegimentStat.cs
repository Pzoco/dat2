using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class RegimentStat:AST
    {
        Identifier i;
        UnitStatType ust;
        public RegimentStat(Identifier i, UnitStatType ust)
        {
            this.i = i;
            this.ust = ust;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitRegimentStat(this, arg);
        }
    }
}
