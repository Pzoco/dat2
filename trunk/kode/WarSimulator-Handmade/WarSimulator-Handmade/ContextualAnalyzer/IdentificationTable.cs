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

        public void EnterEntry(RegimentAssignment raEntry)
        {
            bool duplicateFound = entries.Any(x => x.ra == raEntry);
            if (duplicateFound)
            {
                Console.WriteLine("Error: Duplicate found!");
            }
            else
            {
                entries.Add(new IdEntry(raEntry, level));
            }
        }
        public void RetrieveEntry(IdEntry entry)
        {
            entries.Add(entry);
        }
        
    }
}
