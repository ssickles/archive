using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTraining.Domain
{
    public class Product
    {
        public virtual Guid Id { get; set; }
        public virtual string Category { get; set; }
        public virtual int Discontinued { get; set; }
        public virtual string Name { get; set; }
    }
}
