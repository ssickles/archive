using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncRepositorySynchronization
{
    public interface ISyncProvider<T> where T : ISynchronize
    {
        List<T> Get(DateTime LastUpdated);
        void Save(T Item);
        void Delete(T Item);
    }
}
