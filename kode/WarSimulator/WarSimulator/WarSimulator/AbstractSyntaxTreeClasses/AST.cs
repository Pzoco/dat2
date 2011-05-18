using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public abstract class AST
    {

        public SourcePosition position;

        /* Hvad er meningen med denne funktion? position er public
        public SourcePosition GetPosition()
        {
            return position;
        }
        */
        public AST(SourcePosition inputpos)
        {
            position = inputpos;
        }
        public AST()
        {

        }

        public abstract Object Visit(Visitor v, Object arg);
    }
}
