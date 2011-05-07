using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class RegimentDeclaration:Declaration
    {
        public Identifier i;
        public RegimentSearch rs;

        public RegimentDeclaration(Identifier i,RegimentSearch rs)
        {
            this.i = i;
            this.rs = rs;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitRegimentAssignment(this, arg);
        }
    }
}
