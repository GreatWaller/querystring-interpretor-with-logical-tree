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
        /// 1��ȥ��������,��һ����Ҫ��CreateTree�����ȴ���
        /// 2��ȡ������������ַ�
        /// 3��ȡ��������֮�����Ƿ�Ϊ AND/OR����������Tree��֡�
        ///     �˲���Ϊ���CreateTree�Ĺ��ܣ����ڴ�ע��˵�����Ը�����⡣
        /// 4���������ڵݹ�ȡ���������ż��ɣ�������CreateTree
        /// </summary>
        /// <param name="query"></param>
        public BracketStack(string query)
        {
            //ȥ�ո�Ҳ��Ҫ�ڴ���֮ǰ�����˴������á�
            _query = query.Replace(" ", "");
        }
        /// <summary>
        /// �ж������Ƿ�Ϸ���ͬʱ���������������ŵ�������λ��
        /// Ҫ����ʼ�ַ�Ϊ'(',��ɷ���true.��ʵ������ʱ������ȥ�������������ţ��վ�����false.
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