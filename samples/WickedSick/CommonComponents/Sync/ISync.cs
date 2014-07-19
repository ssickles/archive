using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonComponents.Sync
{
    public enum SyncResult
    {
        Success,
        Failure
    }

    public interface ISync
    {
        string Name { get; set; }
        SyncResult Run();
    }
}
