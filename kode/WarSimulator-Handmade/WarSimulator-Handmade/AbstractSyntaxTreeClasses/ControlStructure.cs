using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class ControlStructure:SingleCommand
    {
        private Expression e;
        public ControlStructure(Expression e)
        {
            // TODO: Complete member initialization
            this.e = e;
        }
    }
}
