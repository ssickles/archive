using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTraining.Domain
{
    public class YearlyBatting
    {
        public virtual int? Id { get; set; }
        public virtual Player Player { get; set; }
        public virtual int Year { get; set; }
        public virtual Team Team { get; set; }
        public virtual int G { get; set; }
        public virtual int PA { get; set; }
        public virtual int AB { get; set; }
        public virtual int R { get; set; }
        public virtual int H { get; set; }
        public virtual int _2B { get; set; }
        public virtual int _3B { get; set; }
        public virtual int HR { get; set; }
        public virtual int RBI { get; set; }
        public virtual int BB { get; set; }
        public virtual int SO { get; set; }
        public virtual int SB { get; set; }
        public virtual int CS { get; set; }
        public virtual int SF { get; set; }
        public virtual int SH { get; set; }
        public virtual int HBP { get; set; }
        public virtual int IBB { get; set; }
        public virtual int GDP { get; set; }
        public virtual int NP { get; set; }
        public virtual int GO { get; set; }
        public virtual int AO { get; set; }
    }
}
