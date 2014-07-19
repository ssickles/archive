using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernateTraining.Domain;
using NHibernate;

namespace NHibernateTraining.Repositories
{
    public class YearlyBattingRepository: Repository<YearlyBatting, int?>, IYearlyBattingRepository
    {
    }
}
