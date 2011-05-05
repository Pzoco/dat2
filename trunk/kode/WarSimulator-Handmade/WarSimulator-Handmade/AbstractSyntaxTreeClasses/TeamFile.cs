using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class TeamFile:AST
    {
        public RegimentBlock rb;

        public TeamFile(RegimentBlock _rb, SourcePosition sp) :base(sp)
        {
            // TODO: Complete member initialization
            rb = _rb;
        }
    }
}
