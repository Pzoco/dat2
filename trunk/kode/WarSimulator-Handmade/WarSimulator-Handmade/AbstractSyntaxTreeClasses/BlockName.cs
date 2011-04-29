using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class BlockName:AST
    {
        public Identifier i;

        public BlockName(Identifier i)
        {
            // TODO: Complete member initialization
            this.i = i;
        }
    }
}
