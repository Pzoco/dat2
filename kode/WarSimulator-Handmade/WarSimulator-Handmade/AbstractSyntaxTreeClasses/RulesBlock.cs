using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class RulesBlock:AST
    {
        public MaximumsBlock mb;
        public StandardsBlock sb;

        public RulesBlock(MaximumsBlock mb, StandardsBlock sb)
        {
            // TODO: Complete member initialization
            this.mb = mb;
            this.sb = sb;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitRulesBlock(this, arg);
        }
    }
}
