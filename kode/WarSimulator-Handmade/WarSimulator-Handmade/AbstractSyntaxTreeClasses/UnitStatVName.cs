using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class UnitStatVName:Terminal
    {
        public UnitStatVName(String spelling, SourcePosition position)
            : base(spelling, position)
        {

        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitUnitStatVName(this, arg);
        }
    }
}
