using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarSimulator_Handmade.Simulation
{
    class Team
    {
		public string name;
		public int number;
		public Color color;
		public List<Regiment> regiments = new List<Regiment>();
    }
}
