using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class TeamFile:AST
    {
        public List<RegimentBlock> rbs;

        public TeamFile(List<RegimentBlock> rbs, SourcePosition sp) :base(sp)
        {
            this.rbs = rbs;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitTeamFile(this, arg);
        }
    }
}
