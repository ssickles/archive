using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SyncRepositorySynchronization
{
    public class DomainModelSync<T> where T : ISynchronize
    {
        private ObservableCollection<T> _items;
        private ISyncProvider<T> _provider;
        private Timer _t1;
        private object _locker = new object();
        private DateTime _lastUpdated = DateTime.MinValue;
        private SynchronizationContext _syncContext;

        public DomainModelSync(ISyncProvider<T> ItemProvider)
        {
            _syncContext = SynchronizationContext.Current;
            _provider = ItemProvider;
            _items = new ObservableCollection<T>();
            _t1 = new Timer(new TimerCallback(DoSync), null, Timeout.Infinite, Timeout.Infinite);
        }

        public ObservableCollection<T> Items { get { return _items; } }

        public void Activate(int Interval)
        {
            _t1.Change(0, Interval);
        }

        public void Deactivate()
        {
            _t1.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void DoSync(object State)
        {
            List<T> updates = _provider.Get(_lastUpdated);
            lock (_locker)
            {
                foreach (T item in _items)
                {
                    if (item.LastUpdated > _lastUpdated)
                        _provider.Save(item);
                }

                foreach (T update in updates)
                {
                    T item = _items.FirstOrDefault<T>(delegate(T it) { return update.Equals(it); });
                    if (item == null)
                    {
                        _syncContext.Send(new SendOrPostCallback(InsertItem), update);
                    }
                    else
                    {
                        _syncContext.Send(new SendOrPostCallback(UpdateItem), update);
                    }
                }
                _lastUpdated = DateTime.Now;
            }
        }

        private void InsertItem(object Item)
        {
            _items.Add((T)Item);
        }

        private void UpdateItem(object Item)
        {
            //id.FirstName = upd.FirstName;
            //id.LastName = upd.LastName;
            //item.LastUpdated = update.LastUpdated;
        }
    }
}
