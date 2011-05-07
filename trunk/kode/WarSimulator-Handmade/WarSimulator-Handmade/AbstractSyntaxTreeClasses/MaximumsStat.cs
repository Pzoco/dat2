using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class MaximumsStat:AST
    {
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitMaximumsStat(this, arg); 
        }
    }
}
