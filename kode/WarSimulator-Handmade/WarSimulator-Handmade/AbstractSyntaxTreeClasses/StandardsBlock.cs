using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class StandardsBlock:AST
    {
        public List<UnitStatDeclaration> usds;
        public BehaviourBlock bb;

        public StandardsBlock(List<UnitStatDeclaration> usds, BehaviourBlock bb)
        {
            this.usds = usds;
            this.bb = bb;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitStandardsBlock(this, arg);
        }
    }
}
