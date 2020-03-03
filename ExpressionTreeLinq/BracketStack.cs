using System.Collections;
using System.Collections.Generic;

namespace ExpressionTreeLinq{
    public class Bracket
    {
        public char LeftBracket { get; set; }
        public int StartAt { get; set; }
        public int EndAt { get; set; }
    }
    public class BracketStack{
        private readonly string _query;

        /// <summary>
        /// "(((Age>=5) OR (Age<=30)) AND Age>60)"
        /// 1：去两端括号,这一步需要在CreateTree中事先处理
        /// 2：取最左侧括号内字符
        /// 3：取完左括号之后检查是否为 AND/OR，进而辅助Tree拆分。
        ///     此部分为外层CreateTree的功能，仅在此注释说明，以辅助理解。
        /// 4：本方法在递归取最外左括号即可，具体在CreateTree
        /// </summary>
        /// <param name="query"></param>
        public BracketStack(string query)
        {
            //去空格也需要在传入之前处理，此处测试用。
            _query = query.Replace(" ", "");
        }
        /// <summary>
        /// 判断括号是否合法，同时返回左侧最外层括号的右括号位置
        /// 要求起始字符为'(',则可返回true.在实际运用时因首先去除了最外层的括号，刚均返回false.
        /// </summary>
        /// <returns></returns>
        public static bool IsValid(string query, out int endAt)
        {
            endAt = 0;
            Stack<char> stack = new Stack<char>();
            for (int i = 0; i < query.Length; i++)
            {
                if (query[i]=='(')
                {
                    stack.Push(query[i]);
                }
                else
                {
                    if (stack.Count==0)
                    {
                        endAt = i==0?0:i-1;
                        return false;
                    }
                    if (query[i]==')' && stack.Pop()!='(')
                    {
                        return false;
                    }
                }
            }
            return stack.Count == 0;
        }

    }
}