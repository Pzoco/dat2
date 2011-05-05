using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class RegimentAssignment:SingleCommand
    {
        public Identifier i;
        public RegimentSearch rs;

        public RegimentAssignment(Identifier i,RegimentSearch rs)
        {
            this.i = i;
            this.rs = rs;
        }
    }
}
