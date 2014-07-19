using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration();
            cfg.Configure();
            NHibernate.Tool.hbm2ddl.SchemaExport schema = new NHibernate.Tool.hbm2ddl.SchemaExport(cfg);
            schema.Create(false, true);
        }
    }
}
