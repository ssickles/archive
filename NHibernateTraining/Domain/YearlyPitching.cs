using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTraining.Domain
{
    public class YearlyPitching
    {
        public virtual int? Id { get; set; }
        public virtual Player Player { get; set; }
        public virtual int Year { get; set; }
        public virtual Team Team { get; set; }
        public virtual int W { get; set; }
        public virtual int L { get; set; }
        public virtual int G { get; set; }
        public virtual int GS { get; set; }
        public virtual int CG { get; set; }
        public virtual int SHO { get; set; }
        public virtual int HLD { get; set; }
        public virtual int SV { get; set; }
        public virtual int SVO { get; set; }
        public virtual int OUTS { get; set; }
        public virtual int H { get; set; }
        public virtual int R { get; set; }
        public virtual int ER { get; set; }
        public virtual int HR { get; set; }
        public virtual int HBP { get; set; }
        public virtual int BB { get; set; }
        public virtual int SO { get; set; }
        public virtual int TB { get; set; }
        public virtual int IBB { get; set; }
        public virtual int WP { get; set; }
        public virtual int BK { get; set; }
        public virtual int SB { get; set; }
        public virtual int CS { get; set; }
        public virtual int PK { get; set; }
        public virtual int GO { get; set; }
        public virtual int AO { get; set; }
    }
}
