using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class Terminal:AST
    {
        public string spelling;

        public Terminal(String spelling, SourcePosition position)
            :base(position)
        {
            // TODO: Complete member initialization
            this.spelling = spelling;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return null;
        }
    }
}
