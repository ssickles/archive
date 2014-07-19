using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IdentityStream.Data
{
    public enum SortDirection
    {
        Ascending,
        Descending
    }

    [DataContract]
    public class SortObject
    {
        [DataMember]
        public string FieldName { get; private set; }
        [DataMember]
        public SortDirection Direction { get; private set; }

        public SortObject(string FieldName)
        {
            this.FieldName = FieldName;
            this.Direction = SortDirection.Ascending;
        }
        public SortObject(string FieldName, SortDirection SortDirection)
        {
            this.FieldName = FieldName;
            this.Direction = SortDirection;
        }
    }
}
