﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class GridStatName:GridStat
    {
        public GridStatNameVariable gsnv;
        public IntegerLiteral il;

        public GridStatName(GridStatNameVariable gsnv, IntegerLiteral il)
        {
            // TODO: Complete member initialization
            this.gsnv = gsnv;
            this.il = il;
        }
    }
}
