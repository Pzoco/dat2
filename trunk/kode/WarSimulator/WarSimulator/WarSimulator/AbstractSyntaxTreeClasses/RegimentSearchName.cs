using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class RegimentSearchName:Terminal
    {
        public RegimentSearchName(string spelling, SourcePosition position)
            : base(spelling, position)
        {

        }
        public override object Visit(Visitor v, object arg)
        {
            return v.VisitRegimentSearchName(this, arg);
        }
    }
}
