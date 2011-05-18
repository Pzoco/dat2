using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class IfCommand : ControlStructure
    {
        public Expression e;
        public List<ElseIfCommand> eifc = new List<ElseIfCommand>();
        public SingleCommand sc1, sc2;
        public IfCommand(Expression e, SingleCommand sc1, SingleCommand sc2, List<ElseIfCommand> eifc, SourcePosition pos)
            : base(pos)
        {
            this.e = e;
            this.sc1 = sc1;
            this.sc2 = sc2;
            this.eifc.AddRange(eifc);
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitIfCommand(this, arg);
        }
    }
}
