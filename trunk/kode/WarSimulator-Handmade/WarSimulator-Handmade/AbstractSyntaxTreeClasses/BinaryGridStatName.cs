using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class BinaryGridStatName:GridStat
    {
        public GridStat gsn1, gsn2;

        public BinaryGridStatName(GridStat gsn1, GridStat gsn2)
        {
            this.gsn1 = gsn1;
            this.gsn2 = gsn2;
        }
    }
}
