using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class IdentificationTable
    {
        private int level = 0;
        private List<IdEntry> entries = new List<IdEntry>();

        public void Open()
        {
            level++;
        }
        public void Close()
        {
            entries.RemoveAll(x => x.level == level);
            level--;
        }

        //returns true for success and false for failure
        public bool EnterEntry(Declaration declaration,string id)
        {
            bool duplicateFound = entries.Any(x => x.id == id);
            if (!duplicateFound)
            {
                entries.Add(new IdEntry(declaration, id,level));
                return true;
            }
            return false;
        }
        public Declaration RetrieveEntry(string id)
        {
            foreach(IdEntry entry in entries)
			{
				if (entry.id == id)
				{
					return entry.declaration;
				}
            }
            return null;
        }
    }
}
