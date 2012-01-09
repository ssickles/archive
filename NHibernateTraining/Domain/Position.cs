using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTraining.Domain
{
    public class Position
    {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual PositionType Type { get; set; }
    }

    public enum PositionType
    {
        B,
        P
    }
}
