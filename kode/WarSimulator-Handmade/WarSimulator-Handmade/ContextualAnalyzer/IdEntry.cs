using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class IdEntry
    {
        public RegimentAssignment ra;
        public int level;

        public IdEntry(RegimentAssignment ra, int level)
        {
            this.ra = ra;
            this.level = level;
        }
    }
}
