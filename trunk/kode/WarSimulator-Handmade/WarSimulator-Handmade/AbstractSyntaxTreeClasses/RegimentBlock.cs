using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class RegimentBlock
    {
        private BlockName bn;
        private UnitStat us;
        private BehaviourBlock bb;

        public RegimentBlock(BlockName bn, UnitStat us, BehaviourBlock bb)
        {
            // TODO: Complete member initialization
            this.bn = bn;
            this.us = us;
            this.bb = bb;
        }
    }
}
