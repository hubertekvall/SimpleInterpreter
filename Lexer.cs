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
        EndIf,
        End,
        NewLine
    }


    public class ReservedKeywords
    {
        public static Dictionary<string, TokenType> names = new Dictionary<string, TokenType>
        {
            {"IF", TokenType.If },
            {"ELSEIF", TokenType.ElseIf},
            {"ELSE", TokenType.Else },
            {"ENDIF", TokenType.EndIf},
            {"VAR", TokenType.VariableDeclaration},
            {"END", TokenType.End }
        };


        public static TokenType GetTokenType(string content)
        {
            TokenType identifiedType;

            return names.TryGetValue(content, out identifiedType) ? identifiedType : TokenType.Identifier;
        }
    }

    public record struct Token(TokenType type, string content = "");











    public struct Lexer
    {

        string text;
        int startPointer = 0;
        int offset = 0;



        public Lexer(string text)
        {
            this.text = text;
        }

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


        void BeginNewToken()
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



        public Token GetNextToken(bool skipNewline = true)
        {

            if (!Empty())
            {

                BeginNewToken();

                var ch = Advance();

                var token = ch switch
                {
                    // WHITESPACE

                    char wsChar when (skipNewline && wsChar == '\n' || wsChar == '\r') => GetNextToken(skipNewline),
                    char wsChar when (wsChar == '\n' || wsChar == '\r') => new Token(TokenType.NewLine),

                    // Arithmetic operators
                    '+' => new Token(TokenType.Add),
                    '-' => new Token(TokenType.Subtract),
                    '*' => new Token(TokenType.Multiply),
                    '/' => new Token(TokenType.Divide),
                    '%' => new Token(TokenType.Mod),

                    // If blank a space is encountered just recursively find the next token
                    ' ' => GetNextToken(skipNewline),
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


                return token;
            }

            return new Token(TokenType.EndOfText);
        }

    }






    public struct Tokenizer : IEnumerable<Token>
    {
        LexemeReader reader;

        public IEnumerator<Token> GetEnumerator()
        {
            return reader;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }


    }



    public struct LexemeReader : IEnumerator<Token>
    {
        string text;

        int startPointer = 0;
        int offset = 0;

        (int offsetCache, int startPointerCache) cachedPointers;


        public LexemeReader(string text)
        {
            this.text = text;
            this.cachedPointers = (0, 0);
        }

        public Token Current => throw new NotImplementedException();

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}

