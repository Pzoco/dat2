﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class RegimentSearch:AST
    {
        public Parameters p;

        public RegimentSearch(Parameters p)
        {
            this.p = p;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitRegimentSearch(this, arg);
        }
    }
}
