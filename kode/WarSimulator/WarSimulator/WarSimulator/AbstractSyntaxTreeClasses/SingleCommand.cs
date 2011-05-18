using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public abstract class SingleCommand:AST
    {
        public SingleCommand(SourcePosition pos)
            : base(pos)
        {

        }
        
    }
}
