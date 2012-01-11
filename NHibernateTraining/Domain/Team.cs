using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTraining.Domain
{
    public class Team
    {
        public virtual int? Id { get; set; }
        public virtual string City { get; set; }
        public virtual string Nickname { get; set; }
        public virtual int YearStarted { get; set; }
        public virtual int YearEnded { get; set; }
    }
}
