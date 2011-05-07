using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    //ER IKKE KORREKT BLIVER FIXED!
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

        public void EnterEntry(Declaration declaration,string id)
        {
            bool duplicateFound = entries.Any(x => x.id == id);
            if (duplicateFound)
            {
                Console.WriteLine("Error: Duplicate found!");
            }
            else
            {
                entries.Add(new IdEntry(declaration, id,level));
            }
        }
        public Declaration RetrieveEntry(string id)
        {
            List<IdEntry> entries = entries.GetRange(x => x.id == id);
            if (entries[0} != null)
            {
                return entries[0];
            }
            else
            {
                entries.Add(new IdEntry(declaration, id, level));
            }
        }
    }
}
