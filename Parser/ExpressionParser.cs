namespace SimpleInterpreter;













public class ExpressionParser : BaseParser
{
    public ExpressionParser(string text) : base(text) { }

    public IExpression Expression()
    {
        return Equality();
    }



    public IExpression Assignment()
    {
        IExpression rootNode = Equality();

        if (Match(TokenType.Assignment))
        {
            IExpression assignmentValue = Assignment();

            if (rootNode is Variable assignmentVariable)
            {
                return new AssignmentStatement(assignmentVariable.Name, assignmentValue);
            }

            throw new Exception("Only variables can be assigned to")
        }

        return rootNode;
    }

    public IExpression Equality()
    {
        IExpression rootNode = Relational();

        while (Match(out Token matchedOperand, TokenType.NotEquals, TokenType.Equals))
        {

            IExpression rightNode = Relational();
            return new BinaryOperator(matchedOperand, rootNode, rightNode);
        }
        return rootNode;
    }

    public IExpression Relational()
    {
        var rootNode = Term();

        while (Match(out Token matchedOperand, TokenType.GreaterOrEquals, TokenType.LesserOrEquals, TokenType.GreaterThan, TokenType.LesserThan))
        {
            var rightNode = Term();
            return new BinaryOperator(matchedOperand, rootNode, rightNode);
        }

        return rootNode;
    }

    public IExpression Term()
    {
        var rootNode = Factor();

        while (Match(out Token matchedOperand, TokenType.Add, TokenType.Subtract))
        {
            var rightNode = Factor();
            return new BinaryOperator(matchedOperand, rootNode, rightNode);
        }

        return rootNode;
    }

    public IExpression Factor()
    {
        var root = Unary();

        while (Match(out Token matchedOperand, TokenType.Divide, TokenType.Multiply))
        {
            var right = Unary();
            root = new BinaryOperator(matchedOperand, root, right);
        }

        return root;
    }

    public IExpression Unary()
    {
        if (Match(out Token matchedOperand, TokenType.Subtract, TokenType.Not))
        {
            return new UnaryOperator(matchedOperand, Primary());
        }

        else return Primary();
    }


    public IExpression Primary()
    {
        if (Match(out Token matchedNumber, TokenType.Number)) return new Literal(double.Parse(matchedNumber.content));
        else if (Match(out Token matchedString, TokenType.StringLiteral)) return new Literal(matchedString.content);
        else if (Match(out Token identifierMatch, TokenType.Identifier)) return new Variable(identifierMatch.content);
        else if (Match(TokenType.Lparen)) return Parenthesis();
        else throw new Exception($"Expected an expression but got: {Advance().type.ToString()}");
    }


    public IExpression Parenthesis()
    {
        var expression = Expression();
        if (!Match(TokenType.Rparen)) throw new Exception("Unclosed parenthesis");
        return expression;
    }


    public IExpression ConditionalExpression()
    {
        Expect(TokenType.Lparen, "Conditional expression  must be initiated by a left-parenthesis");
        var expression = Parenthesis();
        Expect(TokenType.Then, "Expected 'then' following a conditional expression ");


        return expression;
    }
}