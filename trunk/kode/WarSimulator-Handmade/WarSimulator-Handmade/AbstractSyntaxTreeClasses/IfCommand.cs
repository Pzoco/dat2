﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    class IfCommand : ControlStructure
    {
        Expression e;
        SingleCommand sc1, sc2;
        public IfCommand(Expression e, SingleCommand sc1,SingleCommand sc2)
        {
            this.e = e;
            this.sc1 = sc1;
            this.sc2 = sc2;
        }
    }
}
