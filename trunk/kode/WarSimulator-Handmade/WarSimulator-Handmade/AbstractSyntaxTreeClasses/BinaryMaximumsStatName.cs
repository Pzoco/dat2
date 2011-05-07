using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class BinaryMaximumsStatName:MaximumsStat
    {
        MaximumsStat ms1, ms2;
        public BinaryMaximumsStatName(MaximumsStat ms1, MaximumsStat ms2)
        {
            this.ms1 = ms1;
            this.ms2 = ms2;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitBinaryMaximumsStatName(this, arg);
        }
    }
}
