﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class RegimentStatExpression:Expression
    {
        public RegimentStat rs;
        public RegimentStatExpression(RegimentStat rs)
        {
            this.rs = rs;
        }
    }
}