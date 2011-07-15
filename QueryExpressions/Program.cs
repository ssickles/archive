using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Runtime.Serialization;

namespace QueryExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            QueryExpression exp1 = new StringQueryExpression("identity_code", Operator.Equals, "C");
            QueryExpression exp2 = new StringQueryExpression("identity_code", Operator.NotEqual, "CE");
            QueryExpression exp3 = new StringQueryExpression("last_name", Operator.BeginsWith, "Sick");
            QueryExpression exp4 = new NumericQueryExpression("number_of_logins", Operator.GreaterThan, 5);
            NullQueryExpression exp5 = new NullQueryExpression("identity_code");

            QueryObject q = new QueryObject("identities");
            q.Conditions = (!exp1 | !exp2 & exp3 | exp4) & (!exp3 & exp4) | !exp5;
            q.Sorts.Add(new SortObject("identity_code", SortDirection.Descending));
            q.Sorts.Add(new SortObject("last_name"));
            q.SetPaging(50, 0);

            Console.WriteLine(MySqlQueryObjectParser.ToCommand(new MySqlConnection(), "identities", q).CommandText);
            Console.ReadLine();
        }
    }

    public enum ExpressionType
    {
        String,
        Numeric,
        Null,
        Complex
    }
    public enum Operator
    {
        Equals,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        Contains,
        EndsWith,
        BeginsWith,
        In
    }
    public enum QueryOperand
    {
        AND,
        OR
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }

    [DataContract]
    public class QueryExpression
    {
        [DataMember]
        public bool Negated { get; private set; }
        [DataMember]
        public ExpressionType Type { get; private set; }
        [DataMember]
        public string FieldName { get; private set; }
        [DataMember]
        public Operator? Operator { get; private set; }
        [DataMember]
        public string Value { get; private set; }

        [DataMember]
        public QueryExpression LeftExpression { get; private set; }
        [DataMember]
        public QueryOperand Operand { get; private set; }
        [DataMember]
        public QueryExpression RightExpression { get; private set; }

        internal QueryExpression(string FieldName, Operator Operator, string Value)
        {
            this.Type = ExpressionType.String;
            this.FieldName = FieldName;
            this.Operator = Operator;
            this.Value = Value;
        }
        internal QueryExpression(string FieldName, Operator Operator, int Value)
        {
            this.Type = ExpressionType.Numeric;
            this.FieldName = FieldName;
            this.Operator = Operator;
            this.Value = Value.ToString();
        }
        internal QueryExpression(string FieldName, Operator Operator, Guid Value)
        {
            this.Type = ExpressionType.String;
            this.FieldName = FieldName;
            this.Operator = Operator;
            this.Value = Value.ToString();
        }
        internal QueryExpression(string FieldName)
        {
            this.Type = ExpressionType.Null;
            this.FieldName = FieldName;
        }
        internal QueryExpression(bool Negated, ExpressionType Type, string FieldName, Operator? Operator, string Value, QueryExpression LeftExpression, QueryOperand Operand, QueryExpression RightExpression)
        {
            this.Negated = Negated;
            this.Type = Type;
            this.FieldName = FieldName;
            this.Operator = Operator;
            this.Value = Value;
            this.LeftExpression = LeftExpression;
            this.Operand = Operand;
            this.RightExpression = RightExpression;
        }

        internal QueryExpression(QueryExpression FirstExpression, QueryOperand Operand, QueryExpression SecondExpression)
        {
            this.Type = ExpressionType.Complex;
            this.LeftExpression = FirstExpression;
            this.Operand = Operand;
            this.RightExpression = SecondExpression;
        }

        public static QueryExpression operator |(QueryExpression exp1, QueryExpression exp2)
        {
            return new QueryExpression(exp1, QueryOperand.OR, exp2);
        }
        public static QueryExpression operator &(QueryExpression exp1, QueryExpression exp2)
        {
            return new QueryExpression(exp1, QueryOperand.AND, exp2);
        }
        public static QueryExpression operator !(QueryExpression exp1)
        {
            QueryExpression exp = exp1.Copy();
            exp.Negated = !exp.Negated;
            return exp;
        }

        private QueryExpression Copy()
        {
            return new QueryExpression(this.Negated, this.Type, this.FieldName, this.Operator, this.Value, this.LeftExpression, this.Operand, this.RightExpression);
        }
        public void Negate()
        {
            this.Negated = !this.Negated;
        }
    }
    public class NumericQueryExpression : QueryExpression
    {
        public NumericQueryExpression(string FieldName, Operator Operator, int Value) : base(FieldName, Operator, Value) { }
        public NumericQueryExpression(string FieldName, Operator Operator, Guid Value) : base(FieldName, Operator, Value) { }
    }
    public class StringQueryExpression : QueryExpression
    {
        public StringQueryExpression(string FieldName, Operator Operator, string Value) : base(FieldName, Operator, Value) { }
    }
    public class NullQueryExpression : QueryExpression
    {
        public NullQueryExpression(string FieldName) : base(FieldName) { }
    }

    [DataContract]
    public class SortObject
    {
        public string FieldName { get; private set; }
        public SortDirection Direction { get; private set; }

        public SortObject(string FieldName)
        {
            this.FieldName = FieldName;
            this.Direction = SortDirection.Ascending;
        }
        public SortObject(string FieldName, SortDirection SortDirection)
        {
            this.FieldName = FieldName;
            this.Direction = SortDirection;
        }
    }

    [DataContract]
    public class QueryObject
    {
        public QueryObject(string TableName)
        {
            this.TableName = TableName;
            this.Sorts = new List<SortObject>();
        }

        public QueryObject(string TableName, QueryExpression Filters, List<SortObject> Sorts, int PageSize, int Offset)
        {

        }

        [DataMember]
        public string TableName { get; private set; }
        [DataMember]
        public QueryExpression Conditions { get; set; }
        [DataMember]
        public List<SortObject> Sorts { get; private set; }
        [DataMember]
        public int Offset { get; private set; }
        [DataMember]
        public int PageSize { get; private set; }

        public void SetPaging(int PageSize, int Offset)
        {
            this.PageSize = PageSize;
            this.Offset = Offset;
        }
    }

    public static class MySqlQueryObjectParser
    {
        public static MySqlCommand ToCommand(MySqlConnection Connection, string TableName, QueryObject QueryObject)
        {
            Dictionary<string, object> parameters = new Dictionary<string,object>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM ");
            sb.Append(TableName);
            if (QueryObject.Conditions != null)
            {
                sb.Append(" WHERE ");
                sb.Append(ToCommand(QueryObject.Conditions, parameters));
            }

            if (QueryObject.Sorts.Count > 0)
            {
                sb.Append(" ORDER BY ");
                foreach (SortObject s in QueryObject.Sorts)
                {
                    sb.Append(string.Format("{0} {1}", s.FieldName, s.Direction));
                    sb.Append(", ");
                }
                sb.Remove(sb.Length - 2, 2);
            }

            if (QueryObject.Offset >= 0 && QueryObject.PageSize > 0)
            {
                sb.Append(" LIMIT ");
                sb.Append(QueryObject.Offset);
                sb.Append(", ");
                sb.Append(QueryObject.PageSize);
            }

            sb.Append(";");
            MySqlCommand com = new MySqlCommand(sb.ToString(), Connection);
            foreach (string field in parameters.Keys)
            {
                com.Parameters.AddWithValue(field, parameters[field]);
            }
            return com;
        }

        private static string ToCommand(QueryExpression exp, Dictionary<string, object> parameters)
        {
            StringBuilder sb = new StringBuilder();
            if (exp.Negated)
                sb.Append("NOT ");
            switch (exp.Type)
            {
                case ExpressionType.Complex:
                    if (exp.LeftExpression == null || exp.RightExpression == null)
                        throw new ArgumentException("A complex expression must have a LeftExpression, RightExpression and Operand.");
                    
                    sb.Append("(");
                    sb.Append(ToCommand(exp.LeftExpression, parameters));
                    sb.Append(" ");
                    sb.Append(exp.Operand.ToString());
                    sb.Append(" ");
                    sb.Append(ToCommand(exp.RightExpression, parameters));
                    sb.Append(")");
                    break;
                case ExpressionType.Null:
                    if (exp.FieldName == null)
                        throw new ArgumentException("The FieldName must be set in order to do a null comparison.");
                    
                    sb.Append(exp.FieldName);
                    sb.Append(" IS NULL");
                    break;
                case ExpressionType.String:
                case ExpressionType.Numeric:
                    if (string.IsNullOrEmpty(exp.FieldName) || !exp.Operator.HasValue || string.IsNullOrEmpty(exp.Value))
                        throw new ArgumentException("The FieldName, Operator, and Value must be set in order to evaluate the expression.");
                    if (exp.Type == ExpressionType.Numeric)
                    {
                        float test;
                        if (!float.TryParse(exp.Value, out test))
                            throw new ArgumentException("The value for the Numeric Expression is not numeric.");
                    }

                    string paramName = UniqueParameterName(exp.FieldName, parameters);
                    string value = GetValueSql(exp.Value, exp.Operator.Value);
                    sb.Append(exp.FieldName);
                    sb.Append(" ");
                    sb.Append(GetOperatorSql(exp.Operator.Value));
                    sb.Append(" ");
                    sb.Append(paramName);
                    parameters.Add(paramName, exp.Value);
                    break;
            }
            return sb.ToString();
        }

        private static string UniqueParameterName(string FieldName, Dictionary<string, object> Parameters)
        {
            int fieldNum = 0;
            string result = string.Format("@{0}_{1}", FieldName, fieldNum);
            while (Parameters.Keys.Where(f => f.Equals(result)).Count() > 0)
            {
                fieldNum++;
                result = string.Format("@{0}_{1}", FieldName, fieldNum);
            }
            return result;
        }

        private static string GetOperatorSql(Operator Operator)
        {
            switch (Operator)
            {
                case Operator.BeginsWith:
                case Operator.EndsWith:
                case Operator.Contains:
                    return "LIKE";
                case Operator.Equals:
                    return "=";
                case Operator.GreaterThan:
                    return ">";
                case Operator.GreaterThanOrEqual:
                    return ">=";
                case Operator.In:
                    return "IN";
                case Operator.LessThan:
                    return "<";
                case Operator.LessThanOrEqual:
                    return "<=";
                case Operator.NotEqual:
                    return "!=";
                default:
                    throw new ArgumentException("The Operator has not been provided an associated sql command.");
            }
        }

        private static string GetValueSql(string Value, Operator Operator)
        {
            const string beginsWithFormat = "{0}%";
            const string endsWithFormat = "%{0}";
            const string containsFormat = "%{0}%";
            switch (Operator)
            {
                case Operator.BeginsWith:
                    return string.Format(beginsWithFormat, Value);
                case Operator.EndsWith:
                    return string.Format(endsWithFormat, Value);
                case Operator.Contains:
                    return string.Format(containsFormat, Value);
            }
            return Value;
        }
    }
}
