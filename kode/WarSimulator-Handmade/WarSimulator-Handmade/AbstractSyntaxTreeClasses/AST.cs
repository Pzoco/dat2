using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public abstract class AST
    {

        public SourcePosition position;


        public SourcePosition GetPosition()
        {
            return position;
        }

        public AST(SourcePosition thePosition)
        {
            position = thePosition;
        }
    }
}
