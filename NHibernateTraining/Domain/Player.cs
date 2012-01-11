using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTraining.Domain
{
    public class Player
    {
        public virtual int? Id { get; set; }
        public virtual int MlbComId { get; set; }
        private DateTime _mlbComLastUpdated;
        public virtual DateTime MlbComLastUpdated
        {
            get
            {
                if (_mlbComLastUpdated == DateTime.MinValue) _mlbComLastUpdated = DateTime.Now;
                return _mlbComLastUpdated;
            }
            set
            {
                _mlbComLastUpdated = value;
            }
        }
        public virtual int EspnId { get; set; }
        public virtual int YahooId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Pronounced { get; set; }
        private DateTime _birthdate;
        public virtual DateTime Birthdate
        {
            get
            {
                if (_birthdate == DateTime.MinValue) _birthdate = DateTime.Now;
                return _birthdate;
            }
            set
            {
                _birthdate = value;
            }
        }
        public virtual string Birthplace { get; set; }
        public virtual string Bats { get; set; }
        public virtual string Throws { get; set; }
        public virtual string Height { get; set; }
        public virtual string Wieght { get; set; }
        public virtual string Position { get; set; }
        public virtual int Experience { get; set; }
        public virtual string Salary { get; set; }
        public virtual string College { get; set; }
        private DateTime _debut;
        public virtual DateTime Debut
        {
            get
            {
                if (_debut == DateTime.MinValue) _debut = DateTime.Now;
                return _mlbComLastUpdated;
            }
            set
            {
                _debut = value;
            }
        }

        public virtual IList<Position> Eligibility { get; set; }
    }
}
