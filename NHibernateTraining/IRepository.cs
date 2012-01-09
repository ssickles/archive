using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTraining
{
    public interface IRepository<Entity, Key> where Entity : class
    {
        Entity GetById(Key key);
        void Save(Entity entity);
        void Delete(Entity entity);
    }
}
