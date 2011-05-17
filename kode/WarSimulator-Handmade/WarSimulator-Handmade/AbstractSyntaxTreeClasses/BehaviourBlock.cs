using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class Regiment:AST
    {
        public BlockName bn;
        public SingleCommand sc;

        public Regiment(BlockName bn, SingleCommand sc)
        {
            // TODO: Complete member initialization
            this.bn = bn;
            this.sc = sc;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitBehaviourBlock(this, arg);
        }
    }
}
