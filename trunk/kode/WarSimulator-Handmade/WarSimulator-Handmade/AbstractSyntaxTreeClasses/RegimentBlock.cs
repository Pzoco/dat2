using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class RegimentBlock:AST
    {
        public RegimentBlock(SourcePosition pos)
            : base(pos)
        {

        }
         /*
          * Hvad er meningen med de her?
          * */
        public BlockName bn;
        public List<UnitStatDeclaration> usds;
        public BehaviourBlock bb;

        public RegimentBlock(BlockName bn, List<UnitStatDeclaration> usds, BehaviourBlock bb)
        {
            // TODO: Complete member initialization
            this.bn = bn;
            this.usds = usds;
            this.bb = bb;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitRegimentBlock(this, arg);
        }
    }
}
