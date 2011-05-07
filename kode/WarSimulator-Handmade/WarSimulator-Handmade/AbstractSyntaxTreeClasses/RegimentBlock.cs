using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class RegimentBlock:AST
    {
        public BlockName bn;
        public UnitStat us;
        public BehaviourBlock bb;

        public RegimentBlock(BlockName bn, UnitStat us, BehaviourBlock bb)
        {
            // TODO: Complete member initialization
            this.bn = bn;
            this.us = us;
            this.bb = bb;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitRegimentBlock(this, arg);
        }
    }
}
