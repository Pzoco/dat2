using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class BehaviourBlock:AST
    {
        public BlockName bn;
        public List<SingleCommand> scs;

        public BehaviourBlock(BlockName bn, List<SingleCommand> scs)
        {
            // TODO: Complete member initialization
            this.bn = bn;
            this.scs = scs;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitBehaviourBlock(this, arg);
        }
    }
}
