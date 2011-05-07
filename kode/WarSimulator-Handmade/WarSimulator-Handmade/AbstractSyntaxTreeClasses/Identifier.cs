using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class Identifier:Terminal
    {
        DataType type;
        public Identifier(String spelling, SourcePosition position):base(spelling,position)
        {
            this.spelling = spelling;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitIdentifier(this, arg);
        }
    }
}
