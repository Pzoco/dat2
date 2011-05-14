/*
 * TestRegexpParser.cs
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public License
 * as published by the Free Software Foundation; either version 3
 * of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free
 * Software Foundation, Inc., 59 Temple Place, Suite 330, Boston,
 * MA 02111-1307, USA.
 *
 * Copyright (c) 2003-2005 Per Cederberg. All rights reserved.
 */

using System.IO;
using PerCederberg.Grammatica.Runtime;

namespace PerCederberg.Grammatica.Test {

    /**
     * A test case for the RegexpParser class.
     *
     * @author   Per Cederberg, <per at percederberg dot net>
     * @version  1.0
     */
    public class TestRegexpParser : ParserTestCase {

        /**
         * The valid input string.
         */
        private const string VALID_INPUT =
            "[a-z.]+(a|b).?";

        /**
         * The parse tree for the valid input string.
         */
        private const string VALID_OUTPUT =
            "Expr(2001)\n" +
            "  Term(2002)\n" +
            "    Fact(2003)\n" +
            "      Atom(2004)\n" +
            "        LEFT_BRACKET(1003): \"[\", line: 1, col: 1\n" +
            "        CharacterSet(2006)\n" +
            "          Character(2007)\n" +
            "            CHAR(1014): \"a\", line: 1, col: 2\n" +
            "          Character(2007)\n" +
            "            CHAR(1014): \"-\", line: 1, col: 3\n" +
            "          Character(2007)\n" +
            "            CHAR(1014): \"z\", line: 1, col: 4\n" +
            "          Character(2007)\n" +
            "            DOT(1011): \".\", line: 1, col: 5\n" +
            "        RIGHT_BRACKET(1004): \"]\", line: 1, col: 6\n" +
            "      AtomModifier(2005)\n" +
            "        PLUS(1009): \"+\", line: 1, col: 7\n" +
            "    Fact(2003)\n" +
            "      Atom(2004)\n" +
            "        LEFT_PAREN(1001): \"(\", line: 1, col: 8\n" +
            "        Expr(2001)\n" +
            "          Term(2002)\n" +
            "            Fact(2003)\n" +
            "              Atom(2004)\n" +
            "                CHAR(1014): \"a\", line: 1, col: 9\n" +
            "          VERTICAL_BAR(1010): \"|\", line: 1, col: 10\n" +
            "          Expr(2001)\n" +
            "            Term(2002)\n" +
            "              Fact(2003)\n" +
            "                Atom(2004)\n" +
            "                  CHAR(1014): \"b\", line: 1, col: 11\n" +
            "        RIGHT_PAREN(1002): \")\", line: 1, col: 12\n" +
            "    Fact(2003)\n" +
            "      Atom(2004)\n" +
            "        DOT(1011): \".\", line: 1, col: 13\n" +
            "      AtomModifier(2005)\n" +
            "        QUESTION(1007): \"?\", line: 1, col: 14\n";

        /**
         * The unexpected EOF input string.
         */
        private const string UNEXPECTED_EOF_INPUT = "(abc";

        /**
         * The unexpected character input string.
         */
        private const string UNEXPECTED_CHAR_INPUT = "a\nb";

        /**
         * The unexpected token input string.
         */
        private const string UNEXPECTED_TOKEN_INPUT = "abc)";

        /**
         * Tests parsing a valid input string.
         */
        public void TestValidInput() {
            Parse(CreateParser(VALID_INPUT), VALID_OUTPUT);
        }

        /**
         * Tests parsing with an unexpected EOF error.
         */
        public void TestUnexpectedEOF() {
            FailParse(CreateParser(UNEXPECTED_EOF_INPUT),
                      ParseException.ErrorType.UNEXPECTED_EOF,
                      1,
                      5);
        }

        /**
         * Tests parsing with an unexpected character error.
         */
        public void TestUnexpectedChar() {
            FailParse(CreateParser(UNEXPECTED_CHAR_INPUT),
                      ParseException.ErrorType.UNEXPECTED_CHAR,
                      1,
                      2);
        }

        /**
         * Tests parsing with an unexpected token error.
         */
        public void TestUnexpectedToken() {
            FailParse(CreateParser(UNEXPECTED_TOKEN_INPUT),
                      ParseException.ErrorType.UNEXPECTED_TOKEN,
                      1,
                      4);
        }

        /**
         * Creates a new parser.
         *
         * @param input          the input to parse
         *
         * @return the parser created
         */
        private Parser CreateParser(string input) {
            Parser  parser = null;

            try {
                parser = new RegexpParser(new StringReader(input));
                parser.Prepare();
            } catch (ParserCreationException e) {
                Fail(e.Message);
            }
            return parser;
        }
    }
}