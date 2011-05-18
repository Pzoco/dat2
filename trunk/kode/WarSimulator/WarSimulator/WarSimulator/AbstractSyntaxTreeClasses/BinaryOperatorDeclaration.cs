using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class BinaryOperatorDeclaration:Declaration
    {
        public DataType arg1, arg2,result;
        public Operator o;
        public BinaryOperatorDeclaration(DataType arg1, DataType arg2, Operator o, DataType result)
        {
            this.arg1 = arg1;
            this.arg1 = arg2;
            this.o = o;
            this.result = result;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitBinaryOperatorDeclaration(this,o);
        }
    }
}
