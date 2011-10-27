using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTraining.Domain
{
    public interface IProductRepository
    {
        void Add(Product product);
        void Update(Product product);
        void Remove(Product product);
        Product GetById(Guid productId);
        Product GetByName(string name);
        ICollection<Product> GetByCategory(string category);
    }
}
