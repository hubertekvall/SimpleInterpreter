namespace SimpleInterpreter;













public class ExpressionParser : Parser
{
    public ExpressionParser(string text) : base(text) { }

    public IExpression Expression()
    {
        return Equality();
    }


    IExpression Equality()
    {
        IExpression rootNode = Relational();

        while (Match(out Token matchedOperand, TokenType.NotEquals, TokenType.Equals))
        {
            Token operand = matchedOperand;
            IExpression rightNode = Relational();
            return new BinaryOperator(operand, rootNode, rightNode);
        }
        return rootNode;
    }

    IExpression Relational()
    {
        var rootNode = Term();

        while (Match(out Token matchedOperand, TokenType.GreaterOrEquals, TokenType.LesserOrEquals, TokenType.GreaterThan, TokenType.LesserThan))
        {
            var operand = matchedOperand;
            var rightNode = Term();
            return new BinaryOperator(operand, rootNode, rightNode);
        }

        return rootNode;
    }

    IExpression Term()
    {
        var rootNode = Factor();

        while (Match(out Token matchedOperand, TokenType.Add, TokenType.Subtract))
        {
            var operand = matchedOperand;
            var rightNode = Factor();
            return new BinaryOperator(operand, rootNode, rightNode);
        }

        return rootNode;
    }

    IExpression Factor()
    {
        var root = Unary();

        while (Match(out Token matchedOperand, TokenType.Divide, TokenType.Multiply))
        {
            var op = matchedOperand;
            var right = Unary();
            root = new BinaryOperator(op, root, right);
        }

        return root;
    }

    IExpression Unary()
    {
        if (Match(out Token matchedOperand, TokenType.Subtract, TokenType.Not))
        {
            var operand = matchedOperand;
            return new UnaryOperator(operand, Primary());
        }

        else return Primary();
    }


    IExpression Primary()
    {
        if (Match(out Token matchedNumber, TokenType.Number)) return new Literal(double.Parse(matchedNumber.content));
        else if (Match(out Token matchedString, TokenType.StringLiteral)) return new Literal(matchedString.content);
        else if (Match(out Token identifierMatch, TokenType.Identifier)) return new Load(identifierMatch.content);
        else if (Match(TokenType.Lparen)) return Parenthesis();
        else throw new Exception($"Expected an expression but got: {Advance().type.ToString()}");
    }


    IExpression Parenthesis()
    {
        var expression = Expression();
        if (!Match(TokenType.Rparen)) throw new Exception("Unclosed parenthesis");
        return expression;
    }
}