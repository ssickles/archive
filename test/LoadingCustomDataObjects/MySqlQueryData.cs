using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoadingCustomDataObjects
{
    [Serializable]
    public class MySqlQueryData
    {
        public ExpressionData Filters { get; set; }
        public List<SortData> Sorts { get; set; }
        public int Offset { get; set; }
        public int PageSize { get; set; }
        public string ToSql()
        {
            StringBuilder sb = new StringBuilder();
            if (Filters != null)
            {
                sb.Append(" WHERE ");
                sb.Append(Filters.ToSql());
            }

            if (Sorts != null && Sorts.Count > 0)
            {
                sb.Append(" ORDER BY ");
                foreach (SortData s in Sorts)
                {
                    sb.Append(s.ToSql());
                    sb.Append(", ");
                }
                sb.Remove(sb.Length - 2, 2);
            }

            if (Offset >= 0 && PageSize > 0)
            {
                sb.Append(" LIMIT ");
                sb.Append(Offset);
                sb.Append(", ");
                sb.Append(PageSize);
            }

            sb.Append(";");
            return sb.ToString();
        }
    }

    [Serializable]
    public class SortData
    {
        public string FieldName { get; set; }
        public SortDirection Direction { get; set; }
        public string ToSql()
        {
            return string.Format("{0} {1}", FieldName, Direction.ToString());
        }
    }

    [Serializable]
    public class ExpressionData
    {
        public bool IsGroupExpression { get; set; }
        public ExpressionData FirstSubExpression { get; set; }
        public Dictionary<ExpressionData, QueryOperand> SubExpressions { get; set; }
        public bool Negated { get; set; }
        public string FieldName { get; set; }
        public string Operation { get; set; }
        public FieldType ValueType { get; set; }
        public string Value { get; set; }
        public string ToSql()
        {
            StringBuilder sb = new StringBuilder();
            if (IsGroupExpression)
            {
                sb.Append("(");
                sb.Append(FirstSubExpression.ToSql());

                foreach (KeyValuePair<ExpressionData, QueryOperand> exp in SubExpressions)
                {
                    sb.Append(" ");
                    sb.Append(exp.Value.ToString());
                    sb.Append(" ");
                    sb.Append(exp.Key.ToSql());
                }
                sb.Append(")");
            }
            else
            {
                switch (ValueType)
                {
                    case FieldType.String:
                        //TODO: check for sql injection
                        break;
                    case FieldType.Numeric:
                        float test;
                        if (!float.TryParse(Value, out test))
                            return string.Empty;
                        break;
                }
                if (Negated) sb.Append("NOT ");
                sb.Append(FieldName);
                sb.Append(" ");
                sb.Append(Operation);
                sb.Append(" ");
                sb.Append(Value.ToString());
            }
            return sb.ToString();
        }
    }

    public enum QueryOperand
    {
        AND,
        OR
    }

    public enum Operator
    {
        Equals,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        In,
        Like
    }

    public enum FieldType
    {
        String,
        Numeric
    }

    public enum SortDirection
    {
        ASC,
        DESC
    }
}
