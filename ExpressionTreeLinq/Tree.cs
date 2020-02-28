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
        
        public void CreateTree(TreeNode<T> node)
        {
            //node.Query = queryString;
            string logic = AND;
            node.Query= REGEX_AND.Replace(node.Query, "#", 1, 0);
            var list = node.Query.Split("#");
            if (list.Length==1)
            {
                // 逻辑运算符 只有 and 和 or; 
                // todo: and 优先级比or 高，在不考虑括号的情况下 应先从 OR 拆分条件
                // todo: not较特殊，等想通后再处理
                // todo: 括号提升优先级
                logic = Regex.Match(node.Query, AND_OR).Value;
                node.Query = REGEX_AND_OR.Replace(node.Query, "#", 1, 0);
                list = node.Query.Split("#");
                if (list.Length==1)
                {
                    // 第一种思路：显得有点多余
                    //生成页节点：先记录节点的逻辑类型，后通过遍历算法，反向生成节点的Data;
                    //todo: 遍历-生成算法：先生成所有叶子节点的Data,再反向生成非叶节点的Data;
                    // 第二种思路：在此处生成叶子节点的Data,在结尾添加非叶子Data
                    var cond1 = new UserCondition { Key = "Name", Value = "A" };
                    node.Data = (ICriterion<T>)GetCriterion(cond1);
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
            //此处为第二种思路：依据logical类型生成Criterion
            node.Data = new AndCriterion<T>(left.Data, right.Data);
            Console.WriteLine("loop");
        }

        protected abstract ICriterion<T> GetCriterion(UserCondition cond);
    }

    public class CompanyTree<Company> : Tree<Company>
    {
        protected override ICriterion<Company> GetCriterion(UserCondition cond)
        {
            return new NameCriterion(cond) as ICriterion<Company>;
        }
    }

}
