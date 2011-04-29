using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class UnitFunction
    {
        public UnitFunctionName ufn;
        public Identifier i;

        public UnitFunction(UnitFunctionName ufn, Identifier i)
        {
            this.ufn = ufn;
            this.i = i;
        }
    }
}
