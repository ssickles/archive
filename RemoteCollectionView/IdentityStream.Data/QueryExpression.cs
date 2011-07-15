using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IdentityStream.Data
{
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

    [DataContract]
    [KnownType(typeof(NumericQueryExpression))]
    [KnownType(typeof(StringQueryExpression))]
    [KnownType(typeof(NullQueryExpression))]
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
    [DataContract]
    public class NumericQueryExpression : QueryExpression
    {
        public NumericQueryExpression(string FieldName, Operator Operator, int Value) : base(FieldName, Operator, Value) { }
        public NumericQueryExpression(string FieldName, Operator Operator, Guid Value) : base(FieldName, Operator, Value) { }
    }
    [DataContract]
    public class StringQueryExpression : QueryExpression
    {
        public StringQueryExpression(string FieldName, Operator Operator, string Value) : base(FieldName, Operator, Value) { }
    }
    [DataContract]
    public class NullQueryExpression : QueryExpression
    {
        public NullQueryExpression(string FieldName) : base(FieldName) { }
    }
}
