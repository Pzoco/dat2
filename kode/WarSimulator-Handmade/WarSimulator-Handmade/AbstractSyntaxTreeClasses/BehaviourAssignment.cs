using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class BehaviourAssignment:BehaviourBlock
    {
        public BlockName bn;

        public BehaviourAssignment(BlockName bn):base(bn,null)
        {
            this.bn = bn;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitBehaviourAssignment(this, arg);
        }
    }
}
