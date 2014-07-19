using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IdentityStream.Data
{
    [DataContract]
    public class QueryObject
    {
        public QueryObject()
        {
            this.Sorts = new List<SortObject>();
        }

        public QueryObject(QueryExpression Conditions, List<SortObject> Sorts, int PageSize, int Offset)
        {
            this.Conditions = Conditions;
            this.Sorts = Sorts;
            this.PageSize = PageSize;
            this.Offset = Offset;
        }

        [DataMember]
        public QueryExpression Conditions { get; set; }
        [DataMember]
        public List<SortObject> Sorts { get; private set; }
        [DataMember]
        public int Offset { get; private set; }
        [DataMember]
        public int PageSize { get; private set; }

        public void SetPaging(int PageSize, int Offset)
        {
            this.PageSize = PageSize;
            this.Offset = Offset;
        }
    }
}
