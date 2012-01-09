using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernateTraining.Domain;
using NHibernate;

namespace NHibernateTraining.Repositories
{
    public class PlayerRepository: IPlayerRepository
    {
        public Player GetById(int? key)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Get<Player>(key);
            }
        }

        public void Save(Player entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(entity);
                transaction.Commit();
            }
        }

        public void Delete(Player entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(entity);
                transaction.Commit();
            }
        }
    }
}
