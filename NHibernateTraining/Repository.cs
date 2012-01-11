using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernateTraining.Repositories;

namespace NHibernateTraining
{
    public class Repository<Entity, Key>: IRepository<Entity, Key> where Entity: class
    {
        public Entity GetById(Key key)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Get<Entity>(key);
            }
        }

        public void Save(Entity entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(entity);
                transaction.Commit();
            }
        }

        public void Delete(Entity entity)
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
