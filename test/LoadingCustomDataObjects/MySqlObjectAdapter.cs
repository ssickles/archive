using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using LoadingCustomDataObjects.Properties;
using System.Data;
using System.Reflection;

namespace LoadingCustomDataObjects
{
    public class MySqlObjectAdapter<T>
    {
        private string _tableName;
        private List<string> _fieldNames = new List<string>();
        private List<Type> _types = new List<Type>();
        private List<MethodInfo> _methods = new List<MethodInfo>();
        private MySqlDataReader _reader;

        public MySqlConnection Connection { get; set; }
        public MySqlQueryData Query { get; set; }

        public MySqlObjectAdapter()
        {
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                _tableName = TableAttribute.GetTableName(typeof(T));
                _fieldNames.Add(FieldAttribute.GetTableField(typeof(T), pi.Name));
                _types.Add(pi.PropertyType);
                _methods.Add(pi.GetSetMethod(false));
            }
        }
        public MySqlObjectAdapter(MySqlConnection Connection)
            : this()
        {
            this.Connection = Connection;
        }
        public MySqlObjectAdapter(MySqlConnection Connection, MySqlQueryData Query)
            : this()
        {
            this.Connection = Connection;
            this.Query = Query;
        }

        public List<T> GetObjects()
        {
            List<T> data = new List<T>();
            StringBuilder sb = new StringBuilder("SELECT * FROM ");
            sb.Append(_tableName);
            if (Query != null)
            {
                sb.Append(Query.ToSql());
            }
            MySqlCommand com = new MySqlCommand(sb.ToString(), Connection);

            if (Connection.State == ConnectionState.Closed)
                Connection.Open();

            MySqlDataReader reader = com.ExecuteReader();

            while (reader.Read())
            {
                T o = (T)Activator.CreateInstance(typeof(T));
                for (int i = 0; i < _fieldNames.Count; i++)
                {
                    SetProperty(o, _types[i], _methods[i], reader, _fieldNames[i]);
                }
                data.Add(o);
            }
            reader.Dispose();
            return data;
        }
        public T GetNextObject()
        {
            if (_reader == null || _reader.IsClosed)
            {
                StringBuilder sb = new StringBuilder("SELECT * FROM ");
                sb.Append(_tableName);
                if (Query != null)
                {
                    sb.Append(Query.ToSql());
                }
                MySqlCommand com = new MySqlCommand(sb.ToString(), Connection);

                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();

                _reader = com.ExecuteReader();
            }

            if (!_reader.Read())
                return default(T);

            T o = (T)Activator.CreateInstance(typeof(T));
            for (int i = 0; i < _fieldNames.Count; i++)
            {
                SetProperty(o, _types[i], _methods[i], _reader, _fieldNames[i]);
            }
            return o;
        }

        private void SetProperty(object Object, Type Type, MethodInfo SetMethod, MySqlDataReader Reader, string FieldName)
        {
            if (Type == typeof(string))
            {
                string value = (Reader[FieldName] != DBNull.Value) ? Reader.GetString(FieldName) : string.Empty;
                SetMethod.Invoke(Object, new object[] { value });
                return;
            }
            if (Type == typeof(int))
            {
                int value = (Reader[FieldName] != DBNull.Value) ? Reader.GetInt32(FieldName) : -1;
                SetMethod.Invoke(Object, new object[] { value });
                return;
            }
            if (Type == typeof(DateTime))
            {
                DateTime value = (Reader[FieldName] != DBNull.Value) ? Reader.GetDateTime(FieldName) : DateTime.MinValue;
                SetMethod.Invoke(Object, new object[] { value });
                return;
            }
            if (Type == typeof(bool))
            {
                SetMethod.Invoke(Object, new object[] { Reader.GetBoolean(FieldName) });
                return;
            }
            if (Type == typeof(byte[]))
            {
                byte[] value = (Reader[FieldName] != DBNull.Value) ? (byte[])Reader[FieldName] : null;
                SetMethod.Invoke(Object, new object[] { value });
                return;
            }
        }
    }
}
