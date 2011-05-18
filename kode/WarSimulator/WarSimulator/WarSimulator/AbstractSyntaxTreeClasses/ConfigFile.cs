using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class ConfigFile:AST
    {
        public GridBlock gb;
        public RulesBlock rb;

        public ConfigFile(GridBlock gb, RulesBlock rb)
        {
            // TODO: Complete member initialization
            this.gb = gb;
            this.rb = rb;
        }
        public override Object Visit(Visitor v, Object arg)
        {
            return v.VisitConfigFile(this, arg);
        }
    }
}
