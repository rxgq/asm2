internal class Interpreter {
    private List<Token> Tokens { get; set; }
    private int Current { get; set; }
    private Stack<int> Program { get; set; } = new();
    private Dictionary<string, int> Defs { get; set; } = new();

    public Interpreter(List<Token> tokens) {
        Tokens = tokens;
        MapDefs();
    }

    public void Interpret() {
        for (; Tokens[Current].Type != TokenType.EOF; Current++) {
            Execute();
        }
    }

    private void Execute() {
        switch (Tokens[Current].Type) {
            case TokenType.PUSH: OnPush(); break;
            case TokenType.DROP: OnDrop(); break;
            case TokenType.DUPE: OnDupe(); break;
            case TokenType.SWAP: OnSwap(); break;
            case TokenType.FREE: OnFree(); break;
            case TokenType.ROTATE: OnRotate(); break;
            case TokenType.SIZE: OnSize(); break;

            case TokenType.ADD: OnOp(); break;
            case TokenType.SUB: OnOp(); break;
            case TokenType.MUL: OnOp(); break;
            case TokenType.DIV: OnOp(); break;
            case TokenType.MOD: OnOp(); break;
            case TokenType.ABS: OnAbs(); break;
            case TokenType.NEG: OnNeg(); break;

            case TokenType.OUT: OnOut(); break;
            case TokenType.READ: OnRead(); break;
            case TokenType.GOTO: OnGoto(); break;
            case TokenType.HALT: OnHalt(); break;
        }
    }

    private void OnPush() {
        Program.Push(int.Parse(Tokens[Current].Args[0]));
    }

    private void OnDrop() {
        if (Program.Count < 1) return;
        Program.Pop();
    }

    private void OnDupe() {
        if (Program.Count < 1) return;

        var a = Program.Peek();
        Program.Push(a);
    }

    private void OnSwap() {
        if (Program.Count < 2) return;

        var a = Program.Pop();
        var b = Program.Pop();

        Program.Push(a);
        Program.Push(b);
    }

    private void OnFree() {
        Program.Clear();
    }

    private void OnRotate() {
        var rev = new Stack<int>();

       while (Program.Count != 0)
            rev.Push(Program.Pop());

        Program = rev;
    }

    private void OnSize() {
        Program.Push(Program.Count); 
    }

    private void OnOut() {
        if (Program.Count < 1) return;

        Thread.Sleep(400);
        Console.Write(Program.Peek() + "\n");
    }

    private void OnRead() {
        var a = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(a)) 
            return;

        Program.Push(int.Parse(a));
    }

    private void OnOp() {
        if (Program.Count < 2) return;

        var b = Program.Pop();
        var a = Program.Pop();

        switch (Tokens[Current].Type) {
            case TokenType.ADD: Program.Push(a + b); break;
            case TokenType.SUB: Program.Push(a - b); break;
            case TokenType.MUL: Program.Push(a * b); break;
            case TokenType.DIV: Program.Push(a / b); break;
            case TokenType.MOD: Program.Push(a % b); break;
        }
    }

    private void OnAbs() {
        if (Program.Count < 1) return;

        var a = Program.Pop();
        Program.Push(Math.Abs(a));
    }

    private void OnNeg() {
        if (Program.Count < 1) return;

        var a = Program.Pop();
        Program.Push(-a);
    }

    private void OnGoto() {
        var def = Defs.TryGetValue(Tokens[Current].Args[0], out int idx) ? idx : -1;
        if (def != -1) Current = idx;
    }

    private void OnHalt() {
        Environment.Exit(0);
    }

    private void MapDefs() {
        for (int i = 0; i < Tokens.Count; i++) {
            if (Tokens[i].Type == TokenType.DEF)
                Defs[Tokens[i].Args[0]] = i;
        }
    }

    public void Print() {
        Console.Write("\n======= PROGRAM =======");
        foreach (var def in Defs)
            Console.Write($"\nDEF: {def.Key} => index: {def.Value}");

        Console.WriteLine("\n");
    }
}