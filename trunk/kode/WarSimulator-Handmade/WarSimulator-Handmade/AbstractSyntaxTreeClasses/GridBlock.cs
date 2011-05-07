using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class GridBlock:AST
    {
        public BlockName bn;
        public GridStat gs;

        public GridBlock(BlockName bn,GridStat gs)
        {
            this.bn = bn;
            this.gs = gs;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitGridBlock(this, arg);
        }
    }
}
