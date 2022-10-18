namespace SimpleInterpreter.Parser;
using SimpleInterpreter.Lexer;
using SimpleInterpreter.Runtime;

using System.Globalization;
using static SimpleInterpreter.Lexer.TokenType;



public record class ExpressionParser(TokenStream Tokens)
{

    public IExpression Expression()
    {
        return Assignment();
    }

    public IExpression Assignment()
    {
        IExpression root = Logical();

        if (Tokens.Match(out _, TokenType.Assignment))
        {
            if (root is Variable variable)
            {
                return new AssignmentExpression(variable.Name, Assignment());
            }
            throw new Exception("Can only assign to variables");
        }

        return root;
    }


    public IExpression Keyword()
    {
        if (Tokens.Match(out Token matchedOperand, TokenType.Sqrt)) return new UnaryOperatorExpression(matchedOperand, Keyword());
        return Equality();
    }


    public IExpression Logical()
    {
        var root = Equality();
        while (Tokens.Match(out Token matchedOperand, TokenType.And, TokenType.Or))
        {
            root = matchedOperand.Type switch
            {
                And => new AndExpression
                {
                    Left = root,
                    Right = Equality()
                },
                Or => new OrExpression
                {
                    Left = root,
                    Right = Equality()
                }
            };
        }
        return root;
    }

    public IExpression Equality() => MatchBinaryOperator(Relational, TokenType.Equals, TokenType.NotEquals);
    public IExpression Relational() => MatchBinaryOperator(Term, TokenType.GreaterOrEquals, TokenType.LesserOrEquals, TokenType.LesserThan, TokenType.GreaterThan);
    public IExpression Term() => MatchBinaryOperator(Factor, TokenType.Add, TokenType.Subtract);
    public IExpression Factor() => MatchBinaryOperator(Unary, TokenType.Multiply, TokenType.Divide);



    public IExpression Unary()
    {
        if (Tokens.Match(out Token matchedOperand, TokenType.Not, TokenType.Subtract, TokenType.Sqrt, TokenType.Print)) return new UnaryOperatorExpression(matchedOperand, Unary());
        return Primary();
    }


    public IExpression Primary()
    {

        if (Tokens.Match(out Token matchedNumber, TokenType.Number)) return new Literal(Double.Parse(matchedNumber.Content, CultureInfo.InvariantCulture));
        else if (Tokens.Match(out Token matchedString, TokenType.StringLiteral)) return new Literal(matchedString.Content);
        else if (Tokens.Match(out Token identifierMatch, TokenType.Identifier)) return new Variable(identifierMatch.Content);
        else if (Tokens.Match(out _, TokenType.Lparen)) return Parenthesis();
        else throw new Exception($"Expected an expression but got: {Tokens.Advance()}");

    }


    public IExpression Parenthesis()
    {
        var expression = Expression();
        if (!Tokens.Match(out _, TokenType.Rparen)) throw new Exception("Unclosed parenthesis");
        return expression;
    }



    public IExpression MatchBinaryOperator(Func<IExpression> higherPrecedenceFunction, params TokenType[] types)
    {
        var root = higherPrecedenceFunction();
        while (Tokens.Match(out Token matchedOperand, types)) root = new BinaryOperatorExpression(matchedOperand, root, higherPrecedenceFunction());
        return root;
    }


}
