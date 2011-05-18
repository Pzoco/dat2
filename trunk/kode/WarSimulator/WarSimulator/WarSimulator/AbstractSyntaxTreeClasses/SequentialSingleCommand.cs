using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class SequentialSingleCommand:SingleCommand
    {
        public SingleCommand S1, S2;
        public SequentialSingleCommand(SingleCommand s1, SingleCommand s2, SourcePosition pos):base(pos)
        {
            S1 = s1;
            S2 = s2;
        }
        public override Object Visit(Visitor v, Object arg)
        {
			return v.VisitSequentialSingleCommand(this, arg);
        }
    }
}
