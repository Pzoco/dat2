using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class MaximumsBlock:AST
    {
        public List<MaximumsStatDeclaration> msds = new List<MaximumsStatDeclaration>();

        public MaximumsBlock(List<MaximumsStatDeclaration> msds)
        {
            // TODO: Complete member initialization
            this.msds = msds;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitMaximumsBlock(this, arg);
        }
    }
}
