namespace SimpleInterpreter;









using static TokenType;



public class ExpParser : BaseParser
{
    public ExpParser(string text) : base(text) { }

    public OperatorNode Expression()
    {
        return Equality();
    }



    public OperatorNode Assignment()
    {
        OperatorNode rootNode = Equality();

        if (Match(out Token match, TokenType.Assignment))
        {
            OperatorNode assignmentValue = Assignment();

            if (rootNode.token == TokenType.Identifier)
            {
                return new OperatorNode(match, rootNode, assignmentValue);
            }

            throw new Exception("Only variables can be assigned to")
        }

        return rootNode;
    }

    public OperatorNode Equality()
    {
        OperatorNode rootNode = Relational();

        while (Match(out Token matchedOperand, TokenType.NotEquals, TokenType.Equals))
        {
            OperatorNode rightNode = Relational();
            return new OperatorNode(matchedOperand, rootNode, rightNode);
        }
        return rootNode;
    }

    public OperatorNode Relational()
    {
        var rootNode = Term();

        while (Match(out Token matchedOperand, TokenType.GreaterOrEquals, TokenType.LesserOrEquals, TokenType.GreaterThan, TokenType.LesserThan))
        {
            var rightNode = Term();
            return new OperatorNode(matchedOperand, rootNode, rightNode);
        }

        return rootNode;
    }

    public OperatorNode Term()
    {
        var rootNode = Factor();

        while (Match(out Token matchedOperand, TokenType.Add, TokenType.Subtract))
        {
            var rightNode = Factor();
            return new OperatorNode(matchedOperand, rootNode, rightNode);
        }

        return rootNode;
    }

    public OperatorNode Factor()
    {
        var root = Unary();

        while (Match(out Token matchedOperand, TokenType.Divide, TokenType.Multiply))
        {
            var right = Unary();
            root = new OperatorNode(matchedOperand, root, right);
        }

        return root;
    }

    public OperatorNode Unary()
    {
        if (Match(out Token matchedOperand, TokenType.Subtract, TokenType.Not))
        {
            return new OperatorNode(matchedOperand, Primary(), null);
        }

        else return Primary();
    }


    public OperatorNode Primary()
    {
        if (Match(out Token match, Number, StringLiteral, Identifier)) return OperatorNode.Literal(match);
        else throw new Exception($"Expected an expression but got: {Peek().type.ToString()}");
    }


    public OperatorNode Parenthesis()
    {
        var expression = Expression();
        if (!Match(TokenType.Rparen)) throw new Exception("Unclosed parenthesis");
        return expression;
    }



}