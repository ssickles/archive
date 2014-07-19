using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;

namespace WickedSick.Data
{
    public abstract class ObjectAdapter<T> : IEnumerable<T>
    {
        protected static string _tableName;
        protected static List<FieldAttribute> _fields = new List<FieldAttribute>();
        protected static List<Type> _types = new List<Type>();
        protected static List<MethodInfo> _getMethods = new List<MethodInfo>();
        protected static List<MethodInfo> _methods = new List<MethodInfo>();

        static ObjectAdapter()
        {
            _tableName = TableAttribute.GetTableName(typeof(T));
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                FieldAttribute fa = FieldAttribute.GetTableField(typeof(T), pi.Name);
                if (fa == null)
                    continue;

                _fields.Add(fa);
                _types.Add(pi.PropertyType);
                _methods.Add(pi.GetSetMethod(false));
                _getMethods.Add(pi.GetGetMethod(false));
            }
        }

        public abstract List<T> GetObjects();
        public abstract T GetNextObject();
        public abstract void Create(T obj);
        public abstract void Update(T obj);
        public abstract void Delete(T obj);
        public abstract void Reset();
        protected abstract void SetProperty(object Object, Type Type, MethodInfo SetMethod, IDataReader Reader, FieldAttribute Field);
        protected object GetPropertyValue(object Object, MethodInfo GetMethod)
        {
            return GetMethod.Invoke(Object, new object[] { });
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return new ObjectAdapterEnumerator<T>(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ObjectAdapterEnumerator<T>(this);
        }

        #endregion
    }
}
