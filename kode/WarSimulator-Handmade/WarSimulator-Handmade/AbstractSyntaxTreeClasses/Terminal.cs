using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class Terminal:AST
    {
        public string spelling;

        public Terminal(string spelling)
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
