using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ExpressionTreeLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            // Add a using directive for System.Linq.Expressions. 
            string[] companies = { "Consolidated Messenger", "Alpine Ski House", "Southridge Video", "City Power & Light",
                   "Coho Winery", "Wide World Importers", "Graphic Design Institute", "Adventure Works",
                   "Humongous Insurance", "Woodgrove Bank", "Margie's Travel", "Northwind Traders",
                   "Blue Yonder Airlines", "Trey Research", "The Phone Company",
                   "Wingtip Toys", "Lucerne Publishing", "Fourth Coffee" };

            // The IQueryable data to query.  
            IQueryable<String> queryableData = companies.AsQueryable<string>();

            //test criterion
            List<Company> companyList = new List<Company>();
            foreach (var company in companies)
            {
                companyList.Add(new Company { Name = company, Age = 5 });
            }
            IQueryable<Company> data = companyList.AsQueryable<Company>();
            //Console.WriteLine(data.Where(p=>p.Name.Contains("A")).Expression.Type.ToString());

            // Compose the expression tree that represents the parameter to the predicate.  
            ParameterExpression p = Expression.Parameter(typeof(Company), "company");

            var cond1 = new UserCondition { Key = "Name", Value = "A" };
            var cond2 = new UserCondition { Key = "Age", Value = "5" };

            var c1 = new NameCriterion(cond1);
            var c2 = new AgeCriterion(cond2);
            var a = new AndCriterion<Company>(c1, c2);
            var exp = a.HandleExpression(p);
            var lambda = Expression.Lambda<Func<Company, bool>>(exp, p);
            var r = data.Where(lambda);
            foreach (var company in r)
                Console.WriteLine(company.Name);

            //var res = a.HandleQueryable(data);
            //foreach (var company in res)
            //    Console.WriteLine(company.Name);
            Console.WriteLine("================================");
            string queryString =
                "((Age>=5 OR Age<=30) AND (((Age>60) AND (Age<10)) OR (Age =1))) " +
                "& (((Age!=5) AND (Age<>30)) OR (Age!>60)) & (Age !<1)";


            //test here
            queryString = "(((Name LIKE A) OR (Name LIKE i)) AND (Name LIKE i))&(Age =5)";
            //remove space
            queryString=queryString.Replace(" ","");
            System.Console.WriteLine(queryString);

            //test logic tree
            TreeNode<Company> head = new TreeNode<Company>();
            head.Query = queryString;

            new CompanyTree<Company>().CreateTree(head);
            var res= head.Data.HandleQueryable(data);
            foreach (var company in res)
                Console.WriteLine(company.Name);

            Console.WriteLine("=================================");

            var bx = 1;
            var by = 10;
            var ba = bx == 1 && bx < 0 || by > 0;
            var bb = bx == 1 && (bx < 0 || by > 0);

            Console.WriteLine(ba.ToString());
            Console.WriteLine(bb.ToString());
            Console.WriteLine("====================================");
            string bracketString = 
                "(Age>=5 OR Age<=30) AND (((Age>60) OR (Age<10)) OR (Age =1))";
            bracketString = bracketString.Replace(" ", "");
            int endAt;
            bool isValid=BracketStack.IsValid(bracketString,out endAt);
            Console.WriteLine($"{bracketString} is {isValid}, and End at {endAt}");
            Console.WriteLine("=======================================");

            string fullString = "(((Name LIKE A) OR (Name LIKE i)) AND (Name LIKE i))&(Age =5)" +
                "&(MaxNumRecordReturn=200) &( PageRecordNum =25) &( RecordStartNo =1) " +
                "& (Sort = Person.AgeUpLimit)";
            fullString = fullString.Replace(" ", "");
            Regex REGEX_COMMON_FIELDS = new Regex("((?:MaxNumRecordReturn|PageRecordNum|RecordStartNo|Sort)=)");
            //var arr = REGEX_COMMON_FIELDS.Split(fullString,2);
            //foreach (var item in arr)
            //{
            //    Console.WriteLine(item);
            //}
            //var m = REGEX_COMMON_FIELDS.Match(fullString)?.Value;
            var list = fullString.Split("&");
            string qString = "";
            string maxNumRecordReturn;
            string pageRecordNum;
            string recordStartNo;
            string sort;
            foreach (var item in list)
            {
                var s = item.Substring(1, item.Length - 2);
                var m = REGEX_COMMON_FIELDS.Match(s);
                var l = REGEX_COMMON_FIELDS.Split(s);
                switch (m.Value)
                {
                    case "MaxNumRecordReturn=":
                        maxNumRecordReturn = l[2];
                        break;
                    case "PageRecordNum=":
                        pageRecordNum = l[2];
                        break;
                    case "RecordStartNo=":
                        recordStartNo = l[2];
                        break;
                    case "Sort=":
                        sort = l[2];
                        break;
                    default:
                        qString += (string.IsNullOrEmpty(qString) ? "" : "&") + item;
                        break;
                }
            }
        }
    }
}
