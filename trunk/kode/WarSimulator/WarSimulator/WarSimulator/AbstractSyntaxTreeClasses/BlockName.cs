using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class BlockName:AST
    {
        public Identifier i;

        public BlockName(Identifier i)
        {
            this.i = i;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitBlockName(this, arg);
        }
    }
}
