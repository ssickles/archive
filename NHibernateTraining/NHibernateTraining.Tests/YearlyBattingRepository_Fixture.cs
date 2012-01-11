using NHibernate;
using NHibernate.Cfg;
using NHibernateTraining.Domain;
using NUnit.Framework;
using NHibernate.Tool.hbm2ddl;
using NHibernateTraining.Repositories;
using System.Collections.Generic;
using System;
using NDbUnit.Core.SqlClient;
using NDbUnit.Core;

namespace NHibernateTraining.Tests
{
    [TestFixture]
    public class YearlyBattingRepository_Fixture
    {
        private ISessionFactory _sessionFactory;
        private Configuration _configuration;
        private INDbUnitTest _mySqlDatabase;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _configuration = new Configuration();
            _configuration.Configure();
            _configuration.AddAssembly(typeof(Player).Assembly);
            _sessionFactory = _configuration.BuildSessionFactory();

            _mySqlDatabase = new SqlDbUnitTest(Properties.Settings.Default.NHibernateTrainingConnectionString);

            _mySqlDatabase.ReadXmlSchema(@"..\..\NHibernateTrainingDataSet.xsd");
            _mySqlDatabase.ReadXml(@"..\..\YearlyBattingRepositoryTestData.xml");
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _mySqlDatabase.PerformDbOperation(NDbUnit.Core.DbOperationFlag.DeleteAll);
        }

        [SetUp]
        public void SetupContext()
        {
            new SchemaExport(_configuration).Execute(false, true, false);

            _mySqlDatabase.PerformDbOperation(DbOperationFlag.CleanInsertIdentity);
        }

        [Test]
        public void Can_add_new_yearly_batting()
        {
            var stat = new YearlyBatting()
            {
                Player = new Player { Name = "Albert Pujols", YahooId = 1 },
                Year = 2011,
                Team = new Team() { City = "Anaheim", Nickname = "Angels" }
            };
            IYearlyBattingRepository repository = new YearlyBattingRepository();
            repository.Save(stat);

            // use session to try to load the product
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<YearlyBatting>(stat.Id);
                // Test that the product was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(stat, fromDb);
                Assert.AreEqual(stat.Player.Name, fromDb.Player.Name);
                Assert.AreEqual(stat.Year, fromDb.Year);
                Assert.AreEqual(stat.Team.City, fromDb.Team.City);
            }
        }

        [Test]
        public void Can_update_existing_yearly_batting()
        {
            IYearlyBattingRepository repository = new YearlyBattingRepository();
            var stat = repository.GetById(2);
            stat.SB = 65;
            repository.Save(stat);

            // use session to try to load the product
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<YearlyBatting>(stat.Id);
                Assert.AreEqual(stat.SB, fromDb.SB);
            }
        }

        [Test]
        public void Can_remove_existing_yearly_batting()
        {
            IYearlyBattingRepository repository = new YearlyBattingRepository();
            var stat = repository.GetById(1);
            repository.Delete(stat);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<YearlyBatting>(stat.Id);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_player_batting_by_id()
        {
            IYearlyBattingRepository repository = new YearlyBattingRepository();
            var fromDb = repository.GetById(1);
            using (ISession session = _sessionFactory.OpenSession())
            {
                session.Update(fromDb.Player);
                session.Update(fromDb.Team);
                Assert.IsNotNull(fromDb);
                Assert.AreEqual("Prince Fielder", fromDb.Player.Name);
                Assert.AreEqual(2011, fromDb.Year);
                Assert.AreEqual("Washington", fromDb.Team.City);
            }
        }
    }
}
