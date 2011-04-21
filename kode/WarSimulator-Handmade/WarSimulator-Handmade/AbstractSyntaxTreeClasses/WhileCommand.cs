using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class WhileCommand:ControlStructure
    {
        public WhileCommand(Expression e, SingleCommand sc):base(e,sc,null)
        {
        }
    }
}
