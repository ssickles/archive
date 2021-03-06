﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTraining.Domain
{
    public interface IPlayerRepository: IRepository<Player, int?>
    {
        Player GetByName(string Name);
        Player GetByYahooId(int Id);
    }
}
