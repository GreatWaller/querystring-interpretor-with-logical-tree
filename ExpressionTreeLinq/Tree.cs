using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ExpressionTreeLinq
{
    public class TreeNode<T>
    {
        public string Query { get; set; }
        public string LogicalOperator { get; set; }
        public ICriterion<T> Data { get; set; }
        public TreeNode<T> Left { get; set; }
        public TreeNode<T> Right { get; set; }

    }
    public abstract class Tree<T>
    {
        //string queryString="(Name.Contains(\"A\") AND (Name.Contains(\"T\")) & (Age==5) & (Age >1)";
        private static string AND = "AND";
        private static string AND_OR = "AND|OR";
        private static Regex REGEX_AND = new Regex("&");
        private static Regex REGEX_AND_OR = new Regex("AND|OR");
        private static Regex REGEX_OPERATOR = new Regex("(=|>(?:=){0,1}|<(?:=|>)?|(<>)|!=|!<|!>)|(LIKE)");
        private static string[] OPERATORS = { "=", ">", "<", ">=", "<=", "<>", "!=", "!<", "!>" };
        public void CreateTree(TreeNode<T> node)
        {
            //node.Query = queryString;
            string logic = AND;
            node.Query= REGEX_AND.Replace(node.Query, "#", 1, 0);
            var list = node.Query.Split("#");
            if (list.Length==1)
            {
                //20200303 优先处理括号
                //第一步：去除两端的最外层括号
                //第二步：调用BracketStack.IsValid()
                int endAt = 0;
                if (list[0][0]=='(')
                {
                    node.Query = list[0].Substring(1, list[0].Length - 2);
                    BracketStack.IsValid(node.Query, out endAt);
                }
                // 逻辑运算符 只有 and 和 or; 
                // todo: and 优先级比or 高，因此 应先从 OR 拆分条件。补充优先级可要求传入时加括号处理
                // todo: not较特殊，等想通后再处理。补充：！为一元运算符，构建逻辑树时不应处理，
                //       可并入叶子节点一并处理
                logic = Regex.Match(node.Query.Substring(endAt,node.Query.Length-1- endAt), AND_OR).Value;
                node.Query = REGEX_AND_OR.Replace(node.Query, "#", 1, endAt);
                list = node.Query.Split("#");
                if (list.Length==1)
                {
                    // 思路：在此处生成叶子节点的Data,在添加完叶子后反向生成父节点Data
                    var data = REGEX_OPERATOR.Split(node.Query);
                    var cond = new UserCondition { Key = data[0], Value = data[2],Operator=data[1] };
                    node.Data = GetCriterion(cond);
                    return;
                }
            }
            //生成逻辑节点
            node.LogicalOperator = logic;
            //node.Data = new AndCriterion<T>();
            //从左向右 构建一个 二叉树结构
            var left = new TreeNode<T>() { Query = list[0] };
            node.Left = left;
            CreateTree(node.Left);
            var right = new TreeNode<T>() { Query = list[1] };
            node.Right = right;
            CreateTree(node.Right);
            //思路：依据logical类型生成Criterion
            if (node.LogicalOperator=="AND")
            {
                node.Data = new AndCriterion<T>(left.Data, right.Data);
            }
            else
            {
                node.Data = new OrCriterion<T>(left.Data, right.Data);
            }
            
        }

        protected abstract ICriterion<T> GetCriterion(UserCondition cond);
    }

    public class CompanyTree<Company> : Tree<Company>
    {
        protected override ICriterion<Company> GetCriterion(UserCondition cond)
        {
            ICriterion<Company> criterion;
            //通过UserCondition.Key对应不同的字段
            switch (cond.Key.ToLower())
            {
                case "name":
                    criterion = new NameCriterion(cond) as ICriterion<Company>;
                    break;
                case "age":
                    criterion = new AgeCriterion(cond) as ICriterion<Company>;
                    break;
                default:
                    criterion = null;
                    break;
            }
            return criterion;
        }
    }

}
