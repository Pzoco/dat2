using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class SequentialRegimentBlock:RegimentBlock
    {
        public RegimentBlock RB1, RB2;
        public SequentialRegimentBlock(RegimentBlock rb1, RegimentBlock rb2, SourcePosition pos)
            : base(pos)
        {
            RB1 = rb1;
            RB2 = rb2;
        }
    }
}
