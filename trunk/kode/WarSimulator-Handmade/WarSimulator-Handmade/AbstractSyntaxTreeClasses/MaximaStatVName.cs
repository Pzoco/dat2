using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class MaximaStatVName:Terminal
    {
        public MaximaStatVName(String spelling, SourcePosition position)
            : base(spelling, position)
        {

        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitMaximaStatVName(this, arg);
        }
    }
}
