using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace WickedSick.Data
{
    public class ObjectAdapterEnumerator<T> : IEnumerator, IEnumerator<T>
    {
        private object _CurrentNotTyped;
        private T _Current;
        private ObjectAdapter<T> _Instance;

        internal ObjectAdapterEnumerator(ObjectAdapter<T> adapter)
        {
            _Instance = adapter;
        }

        #region IEnumerator Members

        object IEnumerator.Current
        {
            get { return _CurrentNotTyped; }
        }

        bool IEnumerator.MoveNext()
        {
            _Current = _Instance.GetNextObject();
            _CurrentNotTyped = _Current;
            return (_CurrentNotTyped != null);
        }

        void IEnumerator.Reset()
        {
            _Instance.Reset();
        }

        #endregion

        #region IEnumerator<T> Members

        T IEnumerator<T>.Current
        {
            get { return _Current; }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            _Current = default(T);
            _CurrentNotTyped = null;
        }

        #endregion
    }
}
