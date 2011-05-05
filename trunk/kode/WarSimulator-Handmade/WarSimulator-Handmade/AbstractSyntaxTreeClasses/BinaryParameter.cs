using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class BinaryParameter:Parameters
    {
        public Parameters p1, p2;

        public BinaryParameter(Parameters p1, Parameters p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
    }
}
