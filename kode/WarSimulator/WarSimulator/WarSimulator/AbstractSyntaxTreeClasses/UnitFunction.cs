using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class UnitFunction:SingleCommand
    {
        public UnitFunctionName ufn;
        public Identifier i;

        public UnitFunction(UnitFunctionName ufn, Identifier i, SourcePosition pos)
            : base(pos)
        {
            this.ufn = ufn;
            this.i = i;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitUnitFunction(this, arg);
        }
    }
}
