/*
 * War2Parser.cs
 *
 * THIS FILE HAS BEEN GENERATED AUTOMATICALLY. DO NOT EDIT!
 */

using System.IO;

using PerCederberg.Grammatica.Runtime;

/**
 * <remarks>A token stream parser.</remarks>
 */
internal class War2Parser : RecursiveDescentParser {

    /**
     * <summary>An enumeration with the generated production node
     * identity constants.</summary>
     */
    private enum SynteticPatterns {
    }

    /**
     * <summary>Creates a new parser with a default analyzer.</summary>
     *
     * <param name='input'>the input stream to read from</param>
     *
     * <exception cref='ParserCreationException'>if the parser
     * couldn't be initialized correctly</exception>
     */
    public War2Parser(TextReader input)
        : base(input) {

        CreatePatterns();
    }

    /**
     * <summary>Creates a new parser.</summary>
     *
     * <param name='input'>the input stream to read from</param>
     *
     * <param name='analyzer'>the analyzer to parse with</param>
     *
     * <exception cref='ParserCreationException'>if the parser
     * couldn't be initialized correctly</exception>
     */
    public War2Parser(TextReader input, War2Analyzer analyzer)
        : base(input, analyzer) {

        CreatePatterns();
    }

    /**
     * <summary>Creates a new tokenizer for this parser. Can be overridden
     * by a subclass to provide a custom implementation.</summary>
     *
     * <param name='input'>the input stream to read from</param>
     *
     * <returns>the tokenizer created</returns>
     *
     * <exception cref='ParserCreationException'>if the tokenizer
     * couldn't be initialized correctly</exception>
     */
    protected override Tokenizer NewTokenizer(TextReader input) {
        return new War2Tokenizer(input);
    }

    /**
     * <summary>Initializes the parser by creating all the production
     * patterns.</summary>
     *
     * <exception cref='ParserCreationException'>if the parser
     * couldn't be initialized correctly</exception>
     */
    private void CreatePatterns() {
        ProductionPattern             pattern;
        ProductionPatternAlternative  alt;

        pattern = new ProductionPattern((int) War2Constants.EXPRESSION,
                                        "Expression");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) War2Constants.TERM, 1, 1);
        alt.AddProduction((int) War2Constants.EXPRESSION_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) War2Constants.EXPRESSION_TAIL,
                                        "ExpressionTail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) War2Constants.ADD, 1, 1);
        alt.AddProduction((int) War2Constants.EXPRESSION, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) War2Constants.SUB, 1, 1);
        alt.AddProduction((int) War2Constants.EXPRESSION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) War2Constants.TERM,
                                        "Term");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) War2Constants.FACTOR, 1, 1);
        alt.AddProduction((int) War2Constants.TERM_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) War2Constants.TERM_TAIL,
                                        "TermTail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) War2Constants.MUL, 1, 1);
        alt.AddProduction((int) War2Constants.TERM, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) War2Constants.DIV, 1, 1);
        alt.AddProduction((int) War2Constants.TERM, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) War2Constants.FACTOR,
                                        "Factor");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) War2Constants.ATOM, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) War2Constants.LEFT_PAREN, 1, 1);
        alt.AddProduction((int) War2Constants.EXPRESSION, 1, 1);
        alt.AddToken((int) War2Constants.RIGHT_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) War2Constants.ATOM,
                                        "Atom");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) War2Constants.NUMBER, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) War2Constants.IDENTIFIER, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);
    }
}
