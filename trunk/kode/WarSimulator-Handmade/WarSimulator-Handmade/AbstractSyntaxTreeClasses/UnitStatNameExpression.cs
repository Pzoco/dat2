﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class UnitStatNameExpression:Expression
    {
        public UnitStat usn;

        public UnitStatNameExpression(UnitStat usn)
        {
            this.usn = usn;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitUnitStatNameExpression(this, arg);
        }
    }
}
