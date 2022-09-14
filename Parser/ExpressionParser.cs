using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SimpleInterpreter;



public class ExpressionParser : Parser
{
    public IExpression Expression()
    {
        return Equality();
    }


    IExpression Equality()
    {
        IExpression rootNode = Relational();

        while (Match(TokenType.NotEquals, TokenType.Equals))
        {
            Token operand = RetrieveToken();
            IExpression rightNode = Relational();
            return new BinaryOperator(operand, rootNode, rightNode);
        }
        return rootNode;
    }

    IExpression Relational()
    {
        var rootNode = Term();

        while (Match(TokenType.GreaterOrEquals, TokenType.LesserOrEquals, TokenType.GreaterThan, TokenType.LesserThan))
        {
            var operand = RetrieveToken();
            var rightNode = Term();
            return new BinaryOperator(operand, rootNode, rightNode);
        }

        return rootNode;
    }

    IExpression Term()
    {
        var rootNode = Factor();

        while (Match(TokenType.Add, TokenType.Subtract))
        {
            var operand = RetrieveToken();
            var rightNode = Factor();
            return new BinaryOperator(operand, rootNode, rightNode);
        }

        return rootNode;
    }

    IExpression Factor()
    {
        var root = Unary();

        while (Match(TokenType.Divide, TokenType.Multiply))
        {
            var op = RetrieveToken();
            var right = Unary();
            root = new BinaryOperator(op, root, right);
        }

        return root;
    }

    IExpression Unary()
    {
        if (Match(TokenType.Subtract, TokenType.Not))
        {
            var operand = RetrieveToken();
            return new UnaryOperator(operand, Primary());
        }

        else return Primary();
    }


    IExpression Primary() => Advance().type switch
    {
        TokenType.Number => new Literal(double.Parse(RetrieveToken().content)),
        TokenType.StringLiteral => new Literal(RetrieveToken().content),
        TokenType.Identifier => new Load(RetrieveToken().content),
        TokenType.Lparen => Parenthesis(),
        _ => throw new Exception($"Expected an expression but got: {RetrieveToken().type.ToString()}")
    };




    IExpression Parenthesis()
    {
        var expression = Expression();
        if (!Match(TokenType.Rparen)) throw new Exception("Unclosed parenthesis");
        return expression;
    }
}