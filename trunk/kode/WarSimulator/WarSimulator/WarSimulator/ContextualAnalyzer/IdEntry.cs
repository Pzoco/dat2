using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class IdEntry
    {
        public Declaration declaration;
        public string id;
        public int level;

        public IdEntry(Declaration declaration,string id, int level)
        {
            this.declaration = declaration;
            this.id = id;
            this.level = level;
        }
    }
}
