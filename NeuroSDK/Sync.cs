using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Threading;

namespace NeuroSDK
{
    class Sync: ISynchronizeInvoke
    {
        private Dispatcher _dispatcher;

        public Sync()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        #region ISynchronizeInvoke Members

        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            throw new NotImplementedException();
        }

        public object EndInvoke(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public object Invoke(Delegate method, object[] args)
        {
            if (args == null)
            {
                return _dispatcher.Invoke(DispatcherPriority.Normal, method);
            }
            else if (args.Length == 1)
            {
                return _dispatcher.Invoke(DispatcherPriority.Normal, method, args[0]);
            }
            else
            {
                return _dispatcher.Invoke(DispatcherPriority.Normal, method, args[0], moreArgs(args));
            }
        }

        public bool InvokeRequired
        {
            get { return _dispatcher != Dispatcher.CurrentDispatcher; }
        }

        #endregion

        private object[] moreArgs(object[] args)
        {
            object[] tempArgs = new object[args.Length - 1];
            Array.Copy(args, 1, tempArgs, 0, tempArgs.Length);
            return tempArgs;
        }
    }
}
