using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class Parser
    {
        #region Constructor
        public Parser()
        {

        }
        #endregion

        private void Accept()

        #region Basic Parse Methods
        private void ParseBlockName();
        private void ParseIdentifier();
        private void ParseIntegerLiteral();
        private void ParseComment();
        private void ParseGraphicLiteral();
        #endregion

        #region Team File Parse Methods
        private void ParseTeamFile();
        private void ParseRegimentBlock();
        private void ParseSingleCommand();
        private void ParseControlStructure();
        private void ParseExpression();
        private void ParsePrimaryExpression();
        private void ParseOperator();
        private void ParseUnitStat();
        private void ParseUnitStatName();
        private void ParseAttackType();
        #endregion

        #region Config File Parse Methods
        private void ParseConfigFile();
        private void ParseGridBlock();
        private void ParseGridStat();
        private void ParseGridStatName();
        private void ParseRulesBlock();
        private void ParseMaximumsBlock();
        private void ParseMaximumsStat();
        private void ParseMaximumsStatName();
        private void ParseStandardsBlock();
    }
}
