using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class UnitStatVNameExpression:Expression
    {
        public UnitStatType ust;

        public UnitStatVNameExpression(UnitStatType ust)
        {
            this.ust = ust;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitUnitStatVNameExpression(this, arg);
        }
    }
}
