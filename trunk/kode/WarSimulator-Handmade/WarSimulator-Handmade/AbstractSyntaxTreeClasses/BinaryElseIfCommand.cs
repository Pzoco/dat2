using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class BinaryElseIfCommand:ControlStructure
    {
        ElseIfCommand eif1, eif2;

        public BinaryElseIfCommand(ElseIfCommand eif1, ElseIfCommand eif2)
        {
            this.eif1 = eif1;
            this.eif2 = eif2;
        }
    }
}
