using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace WickedSick.Data.Sql
{
    public class SqlObjectAdapter<T> : ObjectAdapter<T>
    {
        private SqlDataReader _reader;
        private SqlConnection _connection;
        
        public SqlCommand SelectCommand { get; set; }

        public SqlObjectAdapter() { }

        public SqlObjectAdapter(SqlConnection Connection)
        {
            _connection = Connection;
            this.SelectCommand = new SqlCommand(string.Format("SELECT * FROM {0}", _tableName), Connection);
        }

        public SqlObjectAdapter(SqlCommand SelectCommand)
        {
            this.SelectCommand = SelectCommand;
            _connection = SelectCommand.Connection;
        }

        public SqlObjectAdapter(string StoredProcedure, SqlConnection Connection)
            : this(new SqlCommand(StoredProcedure))
        {
            _connection = new SqlConnection();
            this.SelectCommand.CommandType = CommandType.StoredProcedure;
            this.SelectCommand.Connection = Connection;
        }

        #region ObjectAdapter<T> Members

        public override List<T> GetObjects()
        {
            List<T> data = new List<T>();

            if (SelectCommand == null) return default(List<T>);
            if (SelectCommand.Connection.State == ConnectionState.Closed)
                SelectCommand.Connection.Open();

            SqlDataReader reader = SelectCommand.ExecuteReader();

            while (reader.Read())
            {
                T o = (T)Activator.CreateInstance(typeof(T));
                for (int i = 0; i < _fields.Count; i++)
                {
                    SetProperty(o, _types[i], _methods[i], reader, _fields[i]);
                }
                data.Add(o);
            }
            reader.Dispose();
            return data;
        }
        
        public override T GetNextObject()
        {
            if (_reader == null || _reader.IsClosed)
            {
                if (SelectCommand == null) return default(T);
                if (SelectCommand.Connection.State == ConnectionState.Closed)
                    SelectCommand.Connection.Open();

                _reader = SelectCommand.ExecuteReader();
            }

            if (!_reader.Read())
                return default(T);

            T o = (T)Activator.CreateInstance(typeof(T));
            for (int i = 0; i < _fields.Count; i++)
            {
                SetProperty(o, _types[i], _methods[i], _reader, _fields[i]);
            }
            return o;
        }

        public override void Create(T obj)
        {
            SqlCommand cmd = new SqlCommand(BuildCreateCommandText(), _connection);
            cmd.Parameters.AddRange(BuildCreateCommandParameters(obj).ToArray());

            ExecuteNonQuery(cmd);
        }

        public override void Update(T obj)
        {
            SqlCommand cmd = new SqlCommand(BuildUpdateCommandText(), _connection);
            cmd.Parameters.AddRange(BuildUpdateCommandParameters(obj).ToArray());

            ExecuteNonQuery(cmd);
        }

        public override void Delete(T obj)
        {
            SqlCommand cmd = new SqlCommand(BuildDeleteCommandText(), _connection);
            cmd.Parameters.AddRange(BuildUpdateCommandParameters(obj).ToArray());

            ExecuteNonQuery(cmd);
        }

        private void ExecuteNonQuery(SqlCommand cmd)
        {
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();

            cmd.ExecuteNonQuery();
        }

        public override void Reset()
        {
            _reader.Close();
            _reader = null;
        }

        protected override void SetProperty(object Object, Type Type, MethodInfo SetMethod, IDataReader Reader, FieldAttribute Field)
        {
            object val = null;
            int index = -1;
            try
            {
                index = Reader.GetOrdinal(Field.FieldName);
            }
            catch (IndexOutOfRangeException ex)
            {
                if (!Field.IsOptional)
                {
                    throw ex;
                }
                return;
            }
            if (Type == typeof(string))
            {
                val = (Reader[Field.FieldName] != DBNull.Value) ? Reader.GetString(index) : string.Empty;
            }
            else if (Type == typeof(short))
            {
                val = (Reader[Field.FieldName] != DBNull.Value) ? Reader.GetInt16(index) : (short)-1;
            }
            else if (Type == typeof(int))
            {
                val = (Reader[Field.FieldName] != DBNull.Value) ? Reader.GetInt32(index) : -1;
            }
            else if (Type == typeof(long))
            {
                val = (Reader[Field.FieldName] != DBNull.Value) ? Reader.GetInt64(index) : (long)-1;
            }
            else if (Type == typeof(DateTime))
            {
                val = (Reader[Field.FieldName] != DBNull.Value) ? Reader.GetDateTime(index) : DateTime.MinValue;
            }
            else if (Type == typeof(TimeSpan))
            {
                val = (Reader[Field.FieldName] != DBNull.Value) ? TimeSpan.FromTicks(Reader.GetDateTime(index).Ticks) : TimeSpan.Zero;
            }
            else if (Type == typeof(bool))
            {
                val = Reader.GetBoolean(index);
            }
            else if (Type == typeof(byte[]))
            {
                val = (Reader[Field.FieldName] != DBNull.Value) ? (byte[])Reader[Field.FieldName] : null;
            }
            else if (Type == typeof(Guid))
            {
                val = (Reader[Field.FieldName] != DBNull.Value) ? Reader.GetGuid(index) : Guid.Empty;
            }

            SetMethod.Invoke(Object, new object[] { val });
        }

        #endregion

        private string BuildCreateCommandText()
        {
            string fieldNames = string.Empty;
            string paramNames = string.Empty;

            //Add fields if required access mode is met
            IEnumerable<FieldAttribute> fieldsUsed = _fields.Where(field => field.HasAccess(AccessModes.Write));
            foreach (FieldAttribute field in fieldsUsed)
            {
                fieldNames += string.Format("[{0}],", field.FieldName);
                paramNames += "@" + field.FieldName + ",";
            }
            fieldNames = fieldNames.TrimEnd(',');
            paramNames = paramNames.TrimEnd(',');

            return string.Format("INSERT INTO {0}({1}) VALUES ({2})", _tableName, fieldNames, paramNames);
        }

        private IEnumerable<SqlParameter> BuildCreateCommandParameters(T obj)
        {
            for (int i = 0; i < _fields.Count; i++)
            {
                if (_fields[i].HasAccess(AccessModes.Write))
                    yield return new SqlParameter("@" + _fields[i].FieldName, this.GetPropertyValue(obj, _getMethods[i]));
            }
        }

        private string BuildUpdateCommandText()
        {
            string setFields = string.Empty;
            string whereConditions = string.Empty;

            //Add fields if required access mode is met
            IEnumerable<FieldAttribute> fieldsUsed = _fields.Where(field => field.HasAccess(AccessModes.Write));
            foreach (FieldAttribute field in fieldsUsed)
            {
                setFields += string.Format("[{0}]=@{0},", field.FieldName);
            }
            setFields = setFields.TrimEnd(',');

            IEnumerable<FieldAttribute> whereFields = _fields.Where(field => field.IsKey);
            foreach (FieldAttribute field in whereFields)
            {
                whereConditions += string.Format("[{0}]=@{0},", field.FieldName);
            }
            whereConditions = whereConditions.TrimEnd(',');

            return string.Format("UPDATE {0} SET {1} WHERE {2}", _tableName, setFields, whereConditions);
        }

        private IEnumerable<SqlParameter> BuildUpdateCommandParameters(T obj)
        {
            for (int i = 0; i < _fields.Count; i++)
            {
                if (_fields[i].HasAccess(AccessModes.Write) || _fields[i].IsKey)
                    yield return new SqlParameter("@" + _fields[i].FieldName, this.GetPropertyValue(obj, _getMethods[i]));
            }
        }

        private string BuildDeleteCommandText()
        {
            string whereConditions = string.Empty;

            IEnumerable<FieldAttribute> whereFields = _fields.Where(field => field.IsKey);
            foreach (FieldAttribute field in whereFields)
            {
                whereConditions += string.Format("[{0}]=@{0},", field.FieldName);
            }
            whereConditions = whereConditions.TrimEnd(',');

            return string.Format("DELETE FROM {0} WHERE {1}", _tableName, whereConditions);
        }

        private IEnumerable<SqlParameter> BuildDeleteCommandParameters(T obj)
        {
            for (int i = 0; i < _fields.Count; i++)
            {
                if (_fields[i].IsKey)
                    yield return new SqlParameter("@" + _fields[i].FieldName, this.GetPropertyValue(obj, _getMethods[i]));
            }
        }
    }
}
