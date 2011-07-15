using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedSick.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class TableAttribute : Attribute
    {
        public TableAttribute(string TableName)
        {
            this.TableName = TableName;
        }

        public string TableName { get; private set; }

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

    [Flags]
    public enum AccessModes
    {
        Read = 1,
        Write = 2,
        ReadWrite = 3
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class FieldAttribute : Attribute
    {
        public FieldAttribute(string FieldName)
            : this(FieldName, false) { }

        public FieldAttribute(string FieldName, bool IsKey)
            : this(FieldName, IsKey, AccessModes.ReadWrite) { }

        public FieldAttribute(string FieldName, AccessModes AccessMode)
            : this(FieldName, false, AccessMode) { }

        public FieldAttribute(string FieldName, bool IsKey, AccessModes AccessMode)
            : this(FieldName, IsKey, AccessMode, false) { }

        public FieldAttribute(string FieldName, AccessModes AccessMode, bool IsOptional)
            : this(FieldName, false, AccessMode, IsOptional) { }

        public FieldAttribute(string FieldName, bool IsKey, AccessModes AccessMode, bool IsOptional)
        {
            this.FieldName = FieldName;
            this.IsKey = IsKey;
            this.AccessMode = AccessMode;
            this.IsOptional = IsOptional;
        }

        public string FieldName { get; private set; }
        public bool IsKey { get; private set; }
        public AccessModes AccessMode { get; private set; }
        public bool IsOptional { get; private set; }

        public static FieldAttribute GetTableField(Type Type, string PropertyName)
        {
            object[] atts = Type.GetProperty(PropertyName).GetCustomAttributes(typeof(FieldAttribute), false);
            if (atts.Length == 1)
            {
                return (FieldAttribute)atts[0];
            }
            else
            {
                return null;
            }
        }

        public bool HasAccess(AccessModes mode)
        {
            return ((this.AccessMode & mode) == mode);
        }
    }
}
