namespace SimpleInterpreter;













public class ExpressionParser : BaseParser
{
    public ExpressionParser(string text) : base(text) { }

    public IExpression Expression()
    {
        return Assignment();
    }



    public IExpression Assignment()
    {
        IExpression rootNode = Equality();

        if (Match(out _,  TokenType.Assignment))
        {
            IExpression assignmentValue = Assignment();

            if (rootNode is Variable assignmentVariable)
            {
                return new Assignment(assignmentVariable.Name, assignmentValue);
            }

            throw new Exception("Only variables can be assigned to");
        }

        return rootNode;
    }

    public IExpression Equality()
    {
        IExpression rootNode = Relational();

        while (Match(out Token matchedOperand, TokenType.NotEquals, TokenType.Equals))
        {

            IExpression rightNode = Relational();
            rootNode = new BinaryOperator(matchedOperand, rootNode, rightNode);
        }
        return rootNode;
    }

    public IExpression Relational()
    {
        var rootNode = Term();

        while (Match(out Token matchedOperand, TokenType.GreaterOrEquals, TokenType.LesserOrEquals, TokenType.GreaterThan, TokenType.LesserThan))
        {
            var rightNode = Term();
            rootNode = new BinaryOperator(matchedOperand, rootNode, rightNode);
        }

        return rootNode;
    }

    public IExpression Term()
    {
        var rootNode = Factor();

        while (Match(out Token matchedOperand, TokenType.Add, TokenType.Subtract))
        {
            var rightNode = Factor();
            rootNode = new BinaryOperator(matchedOperand, rootNode, rightNode);
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
        if (Match(out Token matchedNumber, TokenType.Number)) return new Literal(double.Parse(matchedNumber.Content));
        else if (Match(out Token matchedString, TokenType.StringLiteral)) return new Literal(matchedString.Content);
        else if (Match(out Token identifierMatch, TokenType.Identifier)) return new Variable(identifierMatch.Content);
        else if (Match(out _ , TokenType.Lparen)) return Parenthesis();
        else throw new Exception($"Expected an expression but got: {Advance()}");
    }


    public IExpression Parenthesis()
    {
        var expression = Expression();
        if (!Match(out _, TokenType.Rparen)) throw new Exception("Unclosed parenthesis");
        return expression;
    }



}