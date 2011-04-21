using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class IfCommand : ControlStructure
    {
        public IfCommand(Expression e, SingleCommand sc1, SingleCommand sc2): base(e, sc1, sc2)
        {

        }
    }
}
