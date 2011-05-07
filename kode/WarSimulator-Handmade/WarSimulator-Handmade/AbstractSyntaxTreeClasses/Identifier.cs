﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class Identifier:Terminal
    {
        DataType type;
        public Identifier(string spelling):base(spelling)
        {

        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitIdentifier(this, arg);
        }
    }
}