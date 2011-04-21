using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class BehaviourBlock
    {
        private BlockName bn;
        private SingleCommand sc;

        public BehaviourBlock(BlockName bn, SingleCommand sc)
        {
            // TODO: Complete member initialization
            this.bn = bn;
            this.sc = sc;
        }
    }
}
