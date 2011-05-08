using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class RegimentDeclaration:SingleCommand
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
            return v.VisitRegimentDeclaration(this, arg);
        }
    }
}
