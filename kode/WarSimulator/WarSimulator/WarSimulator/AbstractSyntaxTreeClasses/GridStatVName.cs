﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class GridStatVName : Terminal
    {
        public GridStatVName(String spelling, SourcePosition position)
            : base(spelling, position)
        {

        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitGridStatVName(this, arg);
        }
    }
}
