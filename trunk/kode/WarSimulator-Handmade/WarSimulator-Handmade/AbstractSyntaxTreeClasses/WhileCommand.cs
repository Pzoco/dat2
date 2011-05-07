using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class WhileCommand:ControlStructure
    {
        public Expression e;
        public SingleCommand sc;
        public WhileCommand(Expression e, SingleCommand sc)
        {
            this.e = e;
            this.sc = sc;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitWhileCommand(this, arg);
        }
    }
}
