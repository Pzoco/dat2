using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class GridStatDeclaration:Declaration
    {
        public GridStatVName gsnv;
        public IntegerLiteral il;

        public GridStatDeclaration(GridStatVName gsnv, IntegerLiteral il)
        {
            // TODO: Complete member initialization
            this.gsnv = gsnv;
            this.il = il;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitGridStatDeclaration(this, arg);
        }
    }
}
