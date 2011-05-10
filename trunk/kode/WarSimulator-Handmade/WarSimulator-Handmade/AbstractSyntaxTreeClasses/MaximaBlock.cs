using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class MaximaBlock:AST
    {
        public List<MaximaStatDeclaration> msds = new List<MaximaStatDeclaration>();

        public MaximaBlock(List<MaximaStatDeclaration> msds)
        {
            // TODO: Complete member initialization
            this.msds = msds;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitMaximaBlock(this, arg);
        }
    }
}
