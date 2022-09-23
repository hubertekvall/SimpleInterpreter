using System.Collections;

namespace SimpleInterpreter
{
    public enum TokenType
    {

        EndOfText,
        Identifier,
        Lparen,
        Rparen,
        Add,
        Subtract,
        Multiply,
        Divide,
        Mod,
        Number,
        StringLiteral,
        VariableDeclaration,
        Assignment,
        Equals,
        NotEquals,
        GreaterOrEquals,
        LesserOrEquals,
        LesserThan,
        GreaterThan,
        Not,
        If,
        ElseIf,
        Else,
        End,
        Then,
        While,
        For,
        NewLine,
        WhiteSpace
    }


    public class ReservedKeywords
    {
        public static Dictionary<string, TokenType> names = new Dictionary<string, TokenType>
        {
            {"IF", TokenType.If },
            {"ELSEIF", TokenType.ElseIf},
            {"ELSE", TokenType.Else },
            {"VAR", TokenType.VariableDeclaration},
            {"THEN", TokenType.Then},
            {"END", TokenType.End }
        };


        public static TokenType GetTokenType(string content)
        {


            return names.TryGetValue(content, out TokenType identifiedType) ? identifiedType : TokenType.Identifier;
        }
    }

    public record struct Token(TokenType type, string content = "");











    public struct Lexer
    {

        string text;
        int startPointer = 0;
        int offset = 0;



        public Lexer(string text) => this.text = text;
        bool Empty() => offset >= text.Length;


        char Advance()
        {
            if (!Empty()) offset++;
            return text[offset - 1];
        }

        char Peek()
        {
            if (!Empty()) return text[offset];
            else return '\0';
        }



        void StartNewToken()
        {
            startPointer = offset;
        }
        string Finalize()
        {
            var content = text.Substring(startPointer, offset - startPointer);
            return content;
        }



        public bool Chomp(Func<Char, bool> matchFunction)
        {
            if (matchFunction(Peek()))
            {
                Advance();
                return true;
            }

            return false;
        }


        public bool ChompWhile(Func<Char, bool> matchFunction)
        {
            bool status = false;
            while (matchFunction(Peek()))
            {
                Advance();
                status = true;
            }

            return status;
        }



        public Token Identifier()
        {
            ChompWhile(Char.IsLetterOrDigit);
            return new Token(TokenType.Identifier, Finalize());
        }




        public Token Comparison()
        {
            return Peek() switch
            {
                '>' => new Token(TokenType.GreaterOrEquals),
                '<' => new Token(TokenType.LesserOrEquals),
                '=' => new Token(TokenType.Equals),
                _ => new Token(TokenType.Assignment)
            };
        }



        public Token Number()
        {
            ChompWhile(Char.IsDigit);
            if (Chomp((char c) => c == '.') && !ChompWhile(Char.IsDigit))
            {
                throw new Exception("Ill-formed number literal");
            }

            return new Token(TokenType.Number, Finalize());
        }


        public Token StringLiteral(char quoteType)
        {
            if (quoteType == '\'')
            {
                ChompWhile((char c) => c != '\'');
            }

            else
            {
                ChompWhile((char c) => c != '"');
            }

            Advance();
            var content = Finalize();
            return new Token(TokenType.StringLiteral, content.Substring(1, content.Length - 1));
        }






        public IEnumerable<Token> Tokenize()
        {

            if (!Empty())
            {

                StartNewToken();
                var ch = Advance();

                var foundToken = ch switch
                {
                    // WHITESPACE

                    // char wsChar when (skipNewline && wsChar == '\n' || wsChar == '\r') => GetNextToken(skipNewline),
                    char wsChar when (wsChar == '\n' || wsChar == '\r') => new Token(TokenType.NewLine),

                    // Arithmetic operators
                    '+' => new Token(TokenType.Add),
                    '-' => new Token(TokenType.Subtract),
                    '*' => new Token(TokenType.Multiply),
                    '/' => new Token(TokenType.Divide),
                    '%' => new Token(TokenType.Mod),

                    // If blank a space is encountered just recursively find the next token
                    ' ' => new Token(TokenType.WhiteSpace),

                    '=' => new Token(TokenType.Assignment),

                    // IDENTIFIERS AND KEYWORDS
                    char ident when char.IsLetter(ident) => Identifier(),

                    // NUMBER LITERALS
                    char number when char.IsDigit(number) => Number(),

                    // STRING LITERALS
                    char quote when (quote == '"' || quote == '\'') => StringLiteral(quote),

                    // Dividers and spacers
                    '(' => new Token(TokenType.Lparen),
                    ')' => new Token(TokenType.Rparen),
                    _ => throw new Exception($"Invalid character or token: {ch}")
                };


                yield return foundToken;
            }
        }
    }







}

