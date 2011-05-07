using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class AttackType:Terminal
    {
        public AttackType(string spelling): base(spelling)
        {
        
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitAttackType(this,arg);
        }
    }
}
