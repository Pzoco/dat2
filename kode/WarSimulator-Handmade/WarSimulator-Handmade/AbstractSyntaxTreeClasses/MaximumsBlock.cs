using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class MaximumsBlock:AST
    {
        public MaximumsStat ms;

        public MaximumsBlock(MaximumsStat ms)
        {
            // TODO: Complete member initialization
            this.ms = ms;
        }
    }
}
