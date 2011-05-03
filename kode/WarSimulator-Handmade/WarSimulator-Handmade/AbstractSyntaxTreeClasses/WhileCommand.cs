using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class WhileCommand:ControlStructure
    {
        Expression e;
        SingleCommand sc;
        public WhileCommand(Expression e, SingleCommand sc)
        {
            this.e = e;
            this.sc = sc;
        }
    }
}
