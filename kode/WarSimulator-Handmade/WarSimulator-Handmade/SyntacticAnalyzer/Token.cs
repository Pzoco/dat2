using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public enum TokenType
    {
        IDENTIFIER = 0,
        INTEGERLITERAL = 1,
        OPERATOR = 2,
        POSITION = 3,
        REGIMENTS = 4,

        //Units
        UNITFUNCTION = 5,
        UNITSTATNAME = 6,

        //Grid
        GRIDSTATNAME = 7,
        WIDTH = 8,
        HEIGHT = 9,

        //Maximums
        MAXIMUMSTATNAME = 10,
        TEAMS = 11,

        //UnitStatName
        SIZE = 12, 
        RANGE = 13,
        DAMAGE = 14,
        MOVEMENT = 15,
        ATTACKSPEED = 16,
        HEALTH = 17,
        REGIMENTPOSITION = 18,
        TYPE = 19,

        //Attack Type
        MELEE = 20,
        RANGED = 21,

        //Control Structures
        IF = 22,
        ELSE = 23,
        WHILE = 24,

        //Block Types
        TEAM = 25,
        REGIMENT = 26,
        BEHAVIOUR = 27,
        CONFIG = 28,
        GRID = 29,
        STANDARDS = 30,

        //Signs
        SLASH = 31,
        LEFTPAREN = 32,
        RIGHTPAREN = 33,
        LEFTBRACKET = 34,
        RIGHTBRACKET = 35,
        COMMA = 36,
        SEMICOLON = 37,
        ASSIGNMENT = 38,
    }
    class Token
    {
        public TokenType type;
        public string spelling;

        public Token(TokenType type, string spelling)
        {
            this.type = type;
            this.spelling = spelling;
        }
    }
}
