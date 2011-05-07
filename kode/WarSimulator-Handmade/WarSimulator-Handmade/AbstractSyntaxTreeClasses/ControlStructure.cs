using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class ControlStructure:SingleCommand
    {
        public ControlStructure()
        {
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitControlStructure(this, arg);
        }
    }
}
