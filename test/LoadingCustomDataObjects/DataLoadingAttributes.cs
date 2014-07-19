using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoadingCustomDataObjects
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class TableAttribute: Attribute
    {
        private string _tableName;

        public TableAttribute(string TableName)
        {
            _tableName = TableName;
        }

        public string TableName { get { return _tableName; } }

        public static string GetTableName(Type Type)
        {
            object[] atts = Type.GetCustomAttributes(typeof(TableAttribute), false);
            if (atts.Length == 1)
            {
                return ((TableAttribute)atts[0]).TableName;
            }
            else
            {
                throw new Exception();
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class FieldAttribute: Attribute
    {
        private string _fieldName;
        private bool _isKey;

        public FieldAttribute(string FieldName)
        {
            _fieldName = FieldName;
            _isKey = false;
        }
        public FieldAttribute(string FieldName, bool IsKey)
        {
            _fieldName = FieldName;
            _isKey = IsKey;
        }

        public string FieldName { get { return _fieldName; } }
        public bool IsKey { get { return _isKey; } }

        public static string GetTableField(Type Type, string PropertyName)
        {
            object[] atts = Type.GetProperty(PropertyName).GetCustomAttributes(typeof(FieldAttribute), false);
            if (atts.Length == 1)
            {
                return ((FieldAttribute)atts[0]).FieldName;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
