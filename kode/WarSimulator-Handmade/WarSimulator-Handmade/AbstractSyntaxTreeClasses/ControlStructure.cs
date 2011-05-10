using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public abstract class ControlStructure:SingleCommand
    {
        public ControlStructure(SourcePosition pos)
            : base(pos)
        {

        }

    }
}
