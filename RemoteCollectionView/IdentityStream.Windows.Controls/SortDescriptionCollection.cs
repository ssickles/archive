using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using IdentityStream.Data;

namespace IdentityStream.Data
{
    public class SortDescriptionCollection : System.ComponentModel.SortDescriptionCollection
    {
        public new event NotifyCollectionChangedEventHandler CollectionChanged;

        protected override void ClearItems()
        {
            base.ClearItems();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void InsertItem(int index, SortDescription item)
        {
            base.InsertItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        protected override void RemoveItem(int index)
        {
            SortDescription item = this[index];
            base.RemoveItem(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        public List<SortObject> ToSortObjects()
        {
            List<SortObject> sorts = new List<SortObject>();
            foreach (SortDescription sd in this)
            {
                //find property name on the object
                sorts.Add(new SortObject(sd.PropertyName, sd.Direction == ListSortDirection.Ascending ? SortDirection.Ascending: SortDirection.Descending));
            }
            return sorts;
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler obj = CollectionChanged;
            if (obj != null)
            {
                CollectionChanged(this, e);
            }
        }
    }
}
