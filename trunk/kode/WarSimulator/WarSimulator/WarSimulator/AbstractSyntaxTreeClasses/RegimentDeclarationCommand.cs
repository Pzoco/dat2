using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class RegimentDeclarationCommand:SingleCommand
    {
        public RegimentDeclaration rd;
        public RegimentDeclarationCommand(RegimentDeclaration rd, SourcePosition pos)
            : base(pos)
        {
            this.rd = rd;
        }
        public override object Visit(Visitor v, object arg)
        {
            return v.VisitRegimentDeclarationCommand(this,arg);
        }
    }
}
