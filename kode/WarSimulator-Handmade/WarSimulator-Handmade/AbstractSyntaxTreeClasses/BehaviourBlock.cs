using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class BehaviourBlock:AST
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
