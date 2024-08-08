internal class Lexer {

    private string[] Source { get; set; }

    private List<Token> Tokens { get; set; } = new();

    private int Current { get; set; } = 0;

    public Lexer(string[] source) {
        Source = source;
    }

    public List<Token> Tokenize() {
        while (Current < Source.Length) {
            Tokens.Add(NextToken());
            Current++;
        }

        Tokens.Add(new Token("", TokenType.EOF));
        
        return Tokens;
    }

    private Token NextToken() {
        var command = Source[Current].Split(' ')[0].ToLower();

        return command switch {
            "push" => new(command, TokenType.PUSH, Args()),
            "pop"  => new(command,  TokenType.POP, Args()),

            "add"  => new(command,  TokenType.ADD),
            "sub"  => new(command,  TokenType.SUB),
            "mul"  => new(command,  TokenType.MUL),
            "div"  => new(command,  TokenType.DIV),

            ""     => new("",     TokenType.SPACE),
            _      => new("",       TokenType.BAD),
        };
    }

    private List<string> Args() {
        var tokens = Source[Current].Split(' ');

        var args = new List<string>();
        if (tokens.Length > 1) args.Add(tokens[1]);
        if (tokens.Length > 2) args.Add(tokens[2]);

        return args;
    }

    public void Print() {
        foreach (var token in Tokens) {
            Console.Write(token.ToString());
        }
    }
}