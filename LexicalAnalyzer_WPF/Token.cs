namespace Exp1_WPF;

public enum TokenType
{
    关键字= 1,
    分界符 = 2,
    算术运算符 = 3,
    关系运算符 = 4,
    常数 = 5,
    浮点数 = 6,
    标识符 = 7,
    注释 = 8,
    字符串 = 9,
    Error
}

public class Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; }
    public (int Line, int Column) Position { get; set; }

    public string TokenTypeStr
    {
        get
        {
            return $"{Type}";
        }
    }
    public string Pos
    {
        get
        {
            return $"({Position.Line},{Position.Column})";
        }
    }
    //将Token转化为要求的二元序列
    public string BinarySequence
    {
        get
        {
            if (Type == TokenType.Error)
            {
                return "Error";
            }
            else
            {
                if (Value.EndsWith("\n"))
                {
                    return $"({(int)Type},{Value.Substring(0, Value.Length - 1)})";
                }
                return $"({(int)Type},{Value})";
            }
        }
    }


    public override string ToString()
    {
        return $"{Value}\t({(int)Type},{Value})\t{Type}\t({Position.Line},{Position.Column})";
    }
}