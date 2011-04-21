using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class ControlStructure:SingleCommand
    {
        private Expression e;
        private SingleCommand sc1;
        private SingleCommand sc2;

        public ControlStructure(Expression e, SingleCommand sc1, SingleCommand sc2)
        {
            // TODO: Complete member initialization
            this.e = e;
            this.sc1 = sc1;
            this.sc2 = sc2;
        }
    }
}
