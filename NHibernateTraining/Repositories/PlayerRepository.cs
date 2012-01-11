using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernateTraining.Domain;
using NHibernate;

namespace NHibernateTraining.Repositories
{
    public class PlayerRepository: Repository<Player, int?>, IPlayerRepository
    {
        public Player GetByName(string Name)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var q = session.CreateQuery("from Player p where p.Name = :name");
                q.SetParameter<string>("name", Name);
                return q.UniqueResult<Player>();
            }
        }

        public Player GetByYahooId(int Id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var q = session.CreateQuery("from Player p where p.YahooId = :id");
                q.SetParameter<int>("id", Id);
                return q.UniqueResult<Player>();
            }
        }
    }
}
