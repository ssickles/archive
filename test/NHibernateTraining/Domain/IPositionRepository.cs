using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTraining.Domain
{
    public interface IPositionRepository: IRepository<Position, string>
    {
    }
}
