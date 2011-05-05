using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class Parameter:Parameters
    {
        public UnitStatType ust;
        public Operator o;
        public IntegerLiteral il;

        public Parameter(UnitStatType ust, Operator o, IntegerLiteral il)
        {
            // TODO: Complete member initialization
            this.ust = ust;
            this.o = o;
            this.il = il;
        }
    }
}
