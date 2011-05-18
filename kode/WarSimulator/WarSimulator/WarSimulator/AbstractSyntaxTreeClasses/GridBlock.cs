using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class GridBlock:AST
    {
        public BlockName bn;
        public List<GridStatDeclaration> gss = new List<GridStatDeclaration>();

        public GridBlock(BlockName bn, List<GridStatDeclaration> gss)
        {
            this.bn = bn;
            this.gss = gss;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitGridBlock(this, arg);
        }
    }
}
