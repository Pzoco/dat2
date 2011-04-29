using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class TeamFile:AST
    {
        public RegimentBlock rb;

        public TeamFile(RegimentBlock rb)
        {
            // TODO: Complete member initialization
            this.rb = rb;
        }
    }
}
