namespace Exp1_WPF
{
    public class Lexer
    {
        //关键字集合
        private static HashSet<string> _keywords = new HashSet<string>
        {
            "for", "if", "then", "while","end","printf","scanf"
        };
        //关系运算符集合
        private static HashSet<string> _relationOperators = new HashSet<string> { "==", "<=", ">=", "<", ">", "!=" };
        //算数运算符集合
        private static HashSet<string> _arithmeticOperators = new HashSet<string> { "+", "-", "*", "/" };
        //分隔符集合
        private static HashSet<char> _separators = new HashSet<char> { ';', '(', ')', '{', '}', '[', ']', '<', '>','"',','};

        public HashSet<string> Keywords
        {
            get { return _keywords; }
            set { _keywords = value; }
        }

        public HashSet<string> RelationOperators
        {
            get { return _relationOperators; }
            set { _relationOperators = value; }
        }

        public HashSet<string> ArithmeticOperators
        {
            get { return _arithmeticOperators; }
            set { _arithmeticOperators = value; }
        }

        public HashSet<char> Separators
        {
            get { return _separators; }
            set { _separators = value; }
        }

        /// <summary>
        /// 判断一个字符是不是合理的非运算符、分隔符等字符
        /// a-z A-Z _
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private bool IsValidLetter(char a)
        {
            if ((a >= 'a' && a <= 'z') || (a >= 'A' && a <= 'Z') || (a == '_'))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 对一段代码进行词法分析
        /// </summary>
        /// <param name="code">要分析的代码</param>
        /// <returns>分析结果Token</returns>
        public List<Token> Analyze(string code)
        {
            //存放识别出的语法最 小单元
            var tokens = new List<Token>();
            //通过换行符分行，进行分析
            var lines = code.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                //表示语法单元的列数
                int columnNum = 1;

                for (int j = 0; j < line.Length;)
                {
                    char currentChar = line[j];
                    //如果当前的字符是空格，那么分析下一个
                    if (char.IsWhiteSpace(currentChar))
                    {
                        j++;
                        continue;
                    }
                    //Position表示行和列
                    var position = (Line: i + 1, Column: columnNum);

                    // 连续两个/ 表示是注释
                    if (j + 1 < line.Length && currentChar == '/' && line[j + 1] == '/')
                    {
                        tokens.Add(new Token { Type = TokenType.注释, Value = line.Substring(j), Position = position });
                        break; // 跳过剩下的内容，直接跳转到下一行
                    }
                    //如果当前字符是合法的字母 a-z/A-Z/_
                    if (IsValidLetter(currentChar))
                    {
                        int start = j;
                        while (j < line.Length && IsValidLetter(line[j]))
                        {
                            j++;
                        }
                        //拼出完整的单词
                        string word = line.Substring(start, j - start);
                        TokenType type = _keywords.Contains(word) ? TokenType.关键字 : TokenType.标识符;
                        tokens.Add(new Token { Type = type, Value = word, Position = position });
                        columnNum++;
                    }
                    // 当前首字母是数字
                    else if (char.IsDigit(currentChar))
                    {
                        int start = j;
                        TokenType type;
                        //数字后面跟着字符，表示错误的标识符
                        if (j + 1 < line.Length && IsValidLetter(line[j + 1]))
                        {
                            j++;
                            while (j < line.Length && IsValidLetter(line[j]))
                            {
                                j++;
                            }
                            type = TokenType.Error;
                            tokens.Add(new Token { Type = type, Value = line.Substring(start, j - start), Position = position });
                        }
                        else
                        {
                            bool isFloat = false;
                            while (j < line.Length && (char.IsDigit(line[j]) || line[j] == '.'))
                            {
                                //数字串中间包含. 说明为浮点数
                                if (line[j] == '.')
                                {
                                    isFloat = true;
                                }
                                j++;
                            }
                            //拼出完整的数
                            string number = line.Substring(start, j - start);
                            type = isFloat ? TokenType.浮点数 : TokenType.常数;
                            tokens.Add(new Token { Type = type, Value = number, Position = position });
                        }
                        columnNum++;
                    }
                    //当前的字符是关系运算符或算术运算符
                    else if (_relationOperators.Contains(currentChar.ToString()) || _arithmeticOperators.Contains(currentChar.ToString()))
                    {
                        string op = currentChar.ToString();
                        TokenType type;
                        //两个加号或者减号的情况，这里判断为错误，实际上++和--也应该算到算数运算符中
                        if (j + 1 < line.Length && _arithmeticOperators.Contains(line[j + 1].ToString()))
                        {
                            type = TokenType.Error;
                            op += line[j + 1];
                            Console.WriteLine($"op:{op}");
                            j++;
                        }
                        else
                        {
                            type = _relationOperators.Contains(op) ? TokenType.关系运算符 : TokenType.算术运算符;
                        }
                        tokens.Add(new Token { Type = type, Value = op, Position = position });
                        j++;
                        columnNum++;
                    }
                    //当前字符是分隔符
                    else if (_separators.Contains(currentChar))
                    {
                        // 判断是否是字符串的开始符号 '"'
                        if (currentChar == '"' || currentChar == '\'')
                        {
                            char quoteType = currentChar;
                            int startPos = j;
                            j++;
                            // 读取字符串内容
                            while (j < line.Length)
                            {
                                currentChar = line[j];
                                j++;
                                // 处理转义字符
                                if (currentChar == '\\' && j < line.Length && (line[j] == '"' || line[j] == '\''))
                                {
                                    j++;
                                    continue;
                                }
                                // 找到匹配的结束引号
                                if (currentChar == quoteType)
                                {
                                    break;
                                }
                            }
                            tokens.Add(new Token { Type = TokenType.字符串, Value = line.Substring(startPos,j-startPos), Position = position});
                        }
                        else
                        {
                            tokens.Add(new Token { Type = TokenType.分界符, Value = currentChar.ToString(), Position = position});
                            j++;
                        }
                        columnNum++;
                    }
                    //不属于上面的任何一种符号，则说明错误
                    else
                    {
                        tokens.Add(new Token { Type = TokenType.Error, Value = currentChar.ToString(), Position = position });
                        j++;
                        columnNum++;
                    }
                }
            }
            return tokens;
        }
    }
}
