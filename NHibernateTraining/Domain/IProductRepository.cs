using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTraining.Domain
{
    public interface IProductRepository: IRepository<Product, Guid>
    {
        Product GetByName(string name);
        ICollection<Product> GetByCategory(string category);
    }
}
