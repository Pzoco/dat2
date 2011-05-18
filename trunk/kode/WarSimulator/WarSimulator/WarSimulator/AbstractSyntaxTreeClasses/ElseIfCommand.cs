using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class ElseIfCommand:ControlStructure
    {
        public Expression e;
        public SingleCommand sc;
        public ElseIfCommand(Expression e, SingleCommand sc, SourcePosition pos)
            : base(pos)
        {
            this.e = e;
            this.sc = sc;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitElseIfCommand(this, arg);
        }
    }
}
