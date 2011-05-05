using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class StandardsBlock:AST
    {
        public UnitStat us;
        public BehaviourBlock bb;

        public StandardsBlock(UnitStat us, BehaviourBlock bb)
        {
            // TODO: Complete member initialization
            this.us = us;
            this.bb = bb;
        }
    }
}
