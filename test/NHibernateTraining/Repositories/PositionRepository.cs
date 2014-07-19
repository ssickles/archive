using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernateTraining.Domain;

namespace NHibernateTraining.Repositories
{
    public class PositionRepository: Repository<Position, string>, IPositionRepository
    {
    }
}
