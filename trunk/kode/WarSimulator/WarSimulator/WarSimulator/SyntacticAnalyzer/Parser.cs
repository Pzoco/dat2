using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSimulator_Handmade
{
    public class Parser
    {
        private Scanner scanner;
        public ErrorReporter errorReporter;
        private Token currentToken;
        private SourcePosition previousTokenPosition;

        #region Constructor
        public Parser(string file,ErrorReporter reporter)
        {
            scanner = new Scanner(new Source(file));
            errorReporter = reporter;
            previousTokenPosition = new SourcePosition();
        }
        #endregion

        #region Accept Methods
        //Accepts a token if it matches the expected type
        private void Accept(Token.TokenType expectedType)
        {
            if (currentToken.type == expectedType)
            {
                currentToken = scanner.Scan();
            }
            else
            {
                errorReporter.ReportParserError("Expected " + expectedType + ", but got " + currentToken.type, currentToken);
            }
        }

        //Accepts a token
        private void AcceptIt()
        {
            currentToken = scanner.Scan();
        }

        void Start(SourcePosition position)
        {
            position.start = currentToken.position.start;
        }

        void Finish(SourcePosition position)
        {
            position.finish = previousTokenPosition.finish;
        }

        void SyntaxError(String errorMessage, Token tok)
        {
            errorReporter.ReportParserError(errorMessage, tok);
        }
        #endregion

        #region Basic Parse Methods
        private BlockName ParseBlockName()
        {
            Identifier i = ParseIdentifier();
            return new BlockName(i);
        }
        private Identifier ParseIdentifier()
        {
            Identifier id = null;

            if (currentToken.type == Token.TokenType.Identifier)
            {
                previousTokenPosition = currentToken.position;
                String spelling = currentToken.spelling;
                id = new Identifier(spelling, previousTokenPosition);
                currentToken = scanner.Scan();
            }
            else
            {
                SyntaxError("Error parsing Identifier", currentToken);
            }
            return id;
        }
        private IntegerLiteral ParseIntegerLiteral()
        {
            IntegerLiteral il= null;
            if (currentToken.type==Token.TokenType.IntegerLiteral)
            {

                previousTokenPosition = currentToken.position;
                string spelling = currentToken.spelling;
                AcceptIt();
                il = new IntegerLiteral(spelling, previousTokenPosition);
            }
            else
            {
                SyntaxError("Error parsing Integer Literal", currentToken);
            }
            return il;
        }
        private BehaviourBlock ParseBehaviourBlock()
        {
            BehaviourBlock bb = null;
            Accept(Token.TokenType.Behaviour);
            if (currentToken.type == Token.TokenType.Operator)
            {
                AcceptIt();
                BlockName bn = ParseBlockName();
                Accept(Token.TokenType.SemiColon);
                bb = new BehaviourAssignment(bn);
            }
            else
            {
                BlockName bn = ParseBlockName();
                Accept(Token.TokenType.LeftBracket);
                SingleCommand sc = ParseSingleCommand();
                Accept(Token.TokenType.RightBracket);
				bb = new BehaviourBlock(bn, sc);
            }
            return bb;
        }
        #endregion

        #region Team File Parse Methods
        public TeamFile ParseTeamFile()
        {
            TeamFile tf = null;
            currentToken = scanner.Scan();

            Accept(Token.TokenType.Team);
            Identifier id = ParseIdentifier();
            try
            {
                SourcePosition regimentBlockPosition = new SourcePosition();
                Start(regimentBlockPosition);
                RegimentBlock rb = ParseRegimentBlock();
                while (currentToken.type==Token.TokenType.RightBracket)
                {
                    AcceptIt();
                    RegimentBlock rb2 = ParseRegimentBlock();
                    Finish(regimentBlockPosition);
                    rb = new SequentialRegimentBlock(rb, rb2, regimentBlockPosition);
                }
                tf = new TeamFile(rb, previousTokenPosition);
                if (currentToken.type != Token.TokenType.EndOfText)
                {
                    SyntaxError("File did not end when expected!", currentToken);
                }
            }
            catch (Exception ex) { return null; }
            return tf;
        }

        private RegimentBlock ParseRegimentBlock()
        {
            Accept(Token.TokenType.Regiment);
            BlockName bn = ParseBlockName();
            Accept(Token.TokenType.LeftBracket);
            List<UnitStatDeclaration> usds = new List<UnitStatDeclaration>();
            bool declarationFound = true;
            while (declarationFound)
            {
                declarationFound = false;
                switch (currentToken.type)
                {
                    case Token.TokenType.Size:
                    case Token.TokenType.Range:
                    case Token.TokenType.Damage:
                    case Token.TokenType.Movement:
                    case Token.TokenType.AttackSpeed:
                    case Token.TokenType.Health:
                    case Token.TokenType.RegimentPosition:
                    case Token.TokenType.Type:
                        usds.Add(ParseUnitStatDeclaration()); declarationFound = true; break;
                    default:
                        break;
                }
            }
			BehaviourBlock bb = ParseBehaviourBlock();
            Accept(Token.TokenType.RightBracket);
            return new RegimentBlock(bn, usds, bb);
        }
        private SingleCommand ParseSingleCommand()
        {
            SingleCommand sc = null;
            switch (currentToken.type)
            {
                case Token.TokenType.If:
                case Token.TokenType.While:
                    sc = ParseControlStructure();
                    break;
                case Token.TokenType.Regiment:
                    sc = ParseRegimentDeclaration();
                    break;
                case Token.TokenType.Attack:
                case Token.TokenType.MoveAway:
                case Token.TokenType.MoveTowards:
                    sc = ParseUnitFunction();
                    break;
                default:
                    SyntaxError("Error parsing Single Command!", currentToken);
                    break;
            }
            while (currentToken.type == Token.TokenType.If || currentToken.type == Token.TokenType.While || 
                currentToken.type == Token.TokenType.Regiment || currentToken.type == Token.TokenType.Attack || 
                currentToken.type == Token.TokenType.MoveAway || currentToken.type == Token.TokenType.MoveTowards)
            {
                SingleCommand sc2 = null;
                switch (currentToken.type)
                {
                    case Token.TokenType.If:
                    case Token.TokenType.While:
                        sc2 = ParseControlStructure();
                        break;
                    case Token.TokenType.Regiment:
                        sc2 = ParseRegimentDeclaration();
                        break;
                    case Token.TokenType.Attack:
                    case Token.TokenType.MoveAway:
                    case Token.TokenType.MoveTowards:
                        sc2 = ParseUnitFunction();
                        break;
                    default:
                        SyntaxError("Error parsing Single Command!", currentToken);
                        break;
                }
                sc = new SequentialSingleCommand(sc, sc2, previousTokenPosition);
            }
            return sc;
        }
        private SingleCommand ParseControlStructure()
        {
            if (currentToken.type == Token.TokenType.If)
            {
                Expression e = null;
                SingleCommand sc1 = null;
                SingleCommand sc2 = null;
                List<ElseIfCommand> eifc = new List<ElseIfCommand>();
                AcceptIt();
                Accept(Token.TokenType.LeftParen);
                e = ParseExpression();
                Accept(Token.TokenType.RightParen);
                Accept(Token.TokenType.LeftBracket);
                sc1 = ParseSingleCommand();
                Accept(Token.TokenType.RightBracket);
                while (currentToken.type == Token.TokenType.Else)
                {
                    AcceptIt();
                    if (currentToken.type == Token.TokenType.If)
                    {
                        AcceptIt();
                        Accept(Token.TokenType.LeftParen);
                        Expression eife = ParseExpression();
                        Accept(Token.TokenType.RightParen);
                        Accept(Token.TokenType.LeftBracket);
                        SingleCommand eifsc = ParseSingleCommand();
                        Accept(Token.TokenType.RightBracket);
                        eifc.Add(new ElseIfCommand(eife, eifsc, previousTokenPosition));
                    }
                    else
                    {
                        Accept(Token.TokenType.LeftBracket);
                        sc2 = ParseSingleCommand();
                        Accept(Token.TokenType.RightBracket);
                        break;
                    }
                }
                return new IfCommand(e, sc1, sc2, eifc, previousTokenPosition);
            }
            else if (currentToken.type == Token.TokenType.While)
            {
                Expression e = null;
                SingleCommand sc = null;
                AcceptIt();
                Accept(Token.TokenType.LeftParen);
                e = ParseExpression();
                Accept(Token.TokenType.RightParen);
                Accept(Token.TokenType.LeftBracket);
                sc = ParseSingleCommand();
                Accept(Token.TokenType.RightBracket);
                return new WhileCommand(e, sc, previousTokenPosition);
            }
            return null;
        }
        private Expression ParseExpression()
        {
            Expression e1 = ParsePrimaryExpression();
            while (currentToken.type == Token.TokenType.Operator)
            {
                Operator o = ParseOperator();
                Expression e2 = ParsePrimaryExpression();
                e1 = new BinaryExpression(e1, e2, o);
            }
            return e1;
        }
        private Expression ParsePrimaryExpression()
        {
            Expression e = null;
            switch (currentToken.type)
            {
                case Token.TokenType.IntegerLiteral:
                    IntegerLiteral il = ParseIntegerLiteral();
                    e = new IntegerExpression(il);
                    break;
                case Token.TokenType.Operator:
                    Operator o = ParseOperator();
                    Expression pe = ParsePrimaryExpression();
                    e = new UnaryExpression(o, pe);
                    break;
                case Token.TokenType.Size:
                case Token.TokenType.Range:
                case Token.TokenType.Damage:
                case Token.TokenType.Movement:
                case Token.TokenType.AttackSpeed:
                case Token.TokenType.Health:
                case Token.TokenType.RegimentPosition:
                case Token.TokenType.Type:
                    UnitStatType ust = ParseUnitStatType();
                    e = new UnitStatVNameExpression(ust);
                    break;
                case Token.TokenType.LeftParen:
                    AcceptIt();
                    e = ParseExpression();
                    Accept(Token.TokenType.RightParen);
                    break;
                case Token.TokenType.Identifier:
                    e = ParseRegimentStat();
                    break;
                default:
                    SyntaxError("Error parsing Primary Expression", currentToken);
                    break;
            }
            return e;
        }
        private SingleCommand ParseUnitFunction()
        {
            previousTokenPosition = currentToken.position;
            UnitFunctionName ufn = new UnitFunctionName(currentToken.spelling,previousTokenPosition);
            AcceptIt();
            Accept(Token.TokenType.LeftParen);
            Identifier i = ParseIdentifier();
            Accept(Token.TokenType.RightParen);
            Accept(Token.TokenType.SemiColon);
            return new UnitFunction(ufn, i,previousTokenPosition);
        }
        private Operator ParseOperator()
        {
            previousTokenPosition = currentToken.position;
            string spelling = currentToken.spelling;
            Accept(Token.TokenType.Operator);
            return new Operator(spelling,previousTokenPosition);
        }
        private SingleCommand ParseRegimentDeclaration()
        {
            Accept(Token.TokenType.Regiment);
            Identifier i = ParseIdentifier();
            Accept(Token.TokenType.Assignment);
            RegimentSearch rs = ParseRegimentSearch();
            Accept(Token.TokenType.SemiColon);
            RegimentDeclaration rd = new RegimentDeclaration(i, rs);
            return new RegimentDeclarationCommand(rd,previousTokenPosition);
        }
        private RegimentStatExpression ParseRegimentStat()
        {
            RegimentStat e = null;
            UnitStatType ust = null;
            Identifier id = ParseIdentifier();
            Accept(Token.TokenType.Dot);
            switch (currentToken.type)
            {
                case Token.TokenType.Range:
                case Token.TokenType.Health:
                case Token.TokenType.Size:
                case Token.TokenType.Movement:
                case Token.TokenType.AttackSpeed:
                case Token.TokenType.Distance:
                case Token.TokenType.Damage:
                    ust = ParseUnitStatType();
                    e = new RegimentStat(id,ust); break;
                default:
                    SyntaxError("Error parsing Regiment Stat", currentToken);
                    break;
            }
            return new RegimentStatExpression(e); ;
        }
        private RegimentSearch ParseRegimentSearch()
        {
            RegimentSearchName rsn = null;
            if (currentToken.type == Token.TokenType.SearchForEnemies ||
                currentToken.type == Token.TokenType.SearchForFriends)
            {
                rsn = new RegimentSearchName(currentToken.spelling, currentToken.position);
                AcceptIt();
            }
            Accept(Token.TokenType.LeftParen);
            List<Parameter> p = ParseParameters();
            Accept(Token.TokenType.RightParen);

            return new RegimentSearch(p, rsn);
        }
        private List<Parameter> ParseParameters()
        {
            UnitStatType ust = ParseUnitStatType();
            Operator o = ParseOperator();
            IntegerLiteral il = ParseIntegerLiteral();
			List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter(ust, o, il));

            while (currentToken.type == Token.TokenType.Comma)
            {
                UnitStatType ust2 = ParseUnitStatType();
                Operator o2 = ParseOperator();
                IntegerLiteral il2 = ParseIntegerLiteral();
				parameters.Add(new Parameter(ust2, o2, il2));

            }
			return parameters;
        }
        private UnitStatType ParseUnitStatType()
        {
            previousTokenPosition = currentToken.position;
            string spelling = currentToken.spelling;
            AcceptIt();
            return new UnitStatType(spelling,previousTokenPosition);
        }
        private UnitStatDeclaration ParseUnitStatDeclaration()
        {
            UnitStatDeclaration usn = null;
            previousTokenPosition = currentToken.position;
            switch (currentToken.type)
            {
                case Token.TokenType.Size:
                case Token.TokenType.Range:
                case Token.TokenType.Damage:
                case Token.TokenType.Movement:
                case Token.TokenType.AttackSpeed:
                case Token.TokenType.Health:
                    UnitStatVName sn = new UnitStatVName(currentToken.spelling,previousTokenPosition);
                    AcceptIt();
                    Accept(Token.TokenType.Assignment);
                    IntegerLiteral il = ParseIntegerLiteral();
                    Accept(Token.TokenType.SemiColon);
                    usn = new UnitStatIntegerDeclaration(sn, il);
                    break;
                case Token.TokenType.RegimentPosition:
                    sn = new UnitStatVName(currentToken.spelling,previousTokenPosition);
                    AcceptIt();
                    Accept(Token.TokenType.Assignment);
                    Accept(Token.TokenType.Position);
                    Accept(Token.TokenType.LeftParen);
                    IntegerLiteral ilx = ParseIntegerLiteral();
                    Accept(Token.TokenType.Comma);
                    IntegerLiteral ily = ParseIntegerLiteral();
                    Accept(Token.TokenType.RightParen);
                    Accept(Token.TokenType.SemiColon);
                    usn = new UnitStatPositionDeclaration(sn, ilx, ily);
                    break;
                case Token.TokenType.Type:
                    sn = new UnitStatVName(currentToken.spelling,previousTokenPosition);
                    AcceptIt();
                    Accept(Token.TokenType.Assignment);
                    AttackType at = ParseAttackType();
                    Accept(Token.TokenType.SemiColon);
                    usn = new UnitStatTypeDeclaration(sn, at);
                    break;
                default:
                    SyntaxError("Error parsing Unit Stat Declaration", currentToken);
                    break;
            }
            return usn;
        }
        private AttackType ParseAttackType()
        {
            if (currentToken.type == Token.TokenType.Melee || currentToken.type == Token.TokenType.Ranged)
            {
                previousTokenPosition = currentToken.position;
                string spelling = currentToken.spelling;
                AcceptIt();
                return new AttackType(spelling,previousTokenPosition);
            }
            return null;
        }
        #endregion

        #region Config File Parse Methods
        public ConfigFile ParseConfigFile()
        {
            currentToken = scanner.Scan();
            Accept(Token.TokenType.Config);
            GridBlock gb = ParseGridBlock();
            RulesBlock rb = ParseRulesBlock();
            return new ConfigFile(gb, rb);
        }
        private GridBlock ParseGridBlock()
        {
            Accept(Token.TokenType.Grid);
            BlockName bn = ParseBlockName();
            Accept(Token.TokenType.LeftBracket);
            List<GridStatDeclaration> gsd = new List<GridStatDeclaration>();
            while (currentToken.type == Token.TokenType.Width || currentToken.type == Token.TokenType.Height)
            {
                gsd.Add(ParseGridStatDeclaration());
            }
            Accept(Token.TokenType.RightBracket);
            return new GridBlock(bn, gsd);
        }
        private GridStatDeclaration ParseGridStatDeclaration()
        {
            if (currentToken.type == Token.TokenType.Width || currentToken.type == Token.TokenType.Height)
            {
                previousTokenPosition = currentToken.position;
                GridStatVName gsnv = new GridStatVName(currentToken.spelling,previousTokenPosition);
                AcceptIt();
                Accept(Token.TokenType.Assignment);
                IntegerLiteral il = ParseIntegerLiteral();
                Accept(Token.TokenType.SemiColon);
                return new GridStatDeclaration(gsnv, il);
            }
            return null;
        }

        private RulesBlock ParseRulesBlock()
        {
			Accept(Token.TokenType.Rules);
			Accept(Token.TokenType.LeftBracket);
            StandardsBlock sb = ParseStandardsBlock();
            MaximaBlock mb = ParseMaximaBlock();
			Accept(Token.TokenType.RightBracket);
            return new RulesBlock(mb, sb);
        }
        private MaximaBlock ParseMaximaBlock()
        {
            Accept(Token.TokenType.Maxima);
            Accept(Token.TokenType.LeftBracket);
            List<MaximaStatDeclaration> msds = new List<MaximaStatDeclaration>();
            bool declarationFound = true;
            while (declarationFound)
            {
                declarationFound = false;
                switch (currentToken.type)
                {
                    case Token.TokenType.Size:
                    case Token.TokenType.Range:
                    case Token.TokenType.Damage:
                    case Token.TokenType.Movement:
                    case Token.TokenType.AttackSpeed:
                    case Token.TokenType.Health:
                    case Token.TokenType.Regiments:
                    case Token.TokenType.Teams:
                        msds.Add(ParseMaximaStatDeclaration());
                        declarationFound = true; break;
                    default:
                        break;
                }
            }
            Accept(Token.TokenType.RightBracket);

            return new MaximaBlock(msds);
        }
        private MaximaStatDeclaration ParseMaximaStatDeclaration()
        {
            previousTokenPosition = currentToken.position;
            MaximaStatVName msv = new MaximaStatVName(currentToken.spelling, previousTokenPosition);
            AcceptIt();
            Accept(Token.TokenType.Assignment);
            IntegerLiteral il = ParseIntegerLiteral();
            Accept(Token.TokenType.SemiColon);
            return new MaximaStatDeclaration(msv, il);
        }
        private StandardsBlock ParseStandardsBlock()
        {
            Accept(Token.TokenType.Standards);
            Accept(Token.TokenType.LeftBracket);
            List<UnitStatDeclaration> usds = new List<UnitStatDeclaration>();
            bool declarationFound = true;
            while (declarationFound)
            {
                declarationFound = false;
                switch (currentToken.type)
                {
                    case Token.TokenType.Size:
                    case Token.TokenType.Range:
                    case Token.TokenType.Damage:
                    case Token.TokenType.Movement:
                    case Token.TokenType.AttackSpeed:
                    case Token.TokenType.Health:
                    case Token.TokenType.RegimentPosition:
                    case Token.TokenType.Type:
                        usds.Add(ParseUnitStatDeclaration()); declarationFound = true; break;
                    default:
                        break;
                }
            }
			BehaviourBlock bb = ParseBehaviourBlock();
            Accept(Token.TokenType.RightBracket);
            return new StandardsBlock(usds, bb);
        }
        #endregion

    }
}
