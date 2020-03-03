using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionTreeLinq
{
    public class UserCondition
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string Operator { get; set; }
    }

    public enum OperatorEnum
    {
        Equal,
        Contains
    }
}
