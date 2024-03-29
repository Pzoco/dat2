﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class RegimentSearch:AST
    {
        public List<Parameter> p;
        public RegimentSearchName rsn;
        public RegimentSearch(List<Parameter> p, RegimentSearchName rsn)
        {
            this.p = p;
            this.rsn = rsn;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitRegimentSearch(this, arg);
        }
    }
}
