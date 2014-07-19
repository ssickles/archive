using NHibernate;
using NHibernate.Cfg;
using NHibernateTraining.Domain;
using NUnit.Framework;
using NHibernate.Tool.hbm2ddl;
using NHibernateTraining.Repositories;
using System.Collections.Generic;
using System;
using NDbUnit.Core;
using NDbUnit.Core.SqlClient;

namespace NHibernateTraining.Tests
{
    [TestFixture]
    public class PlayerRepository_Fixture
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
            _mySqlDatabase.ReadXml(@"..\..\PlayerRepositoryTestData.xml");
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
        public void Can_add_new_player()
        {
            var player = new Player { Name = "Joe Nathan" };
            IPlayerRepository repository = new PlayerRepository();
            repository.Save(player);

            // use session to try to load the product
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Player>(player.Id);
                // Test that the product was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(player, fromDb);
                Assert.AreEqual(player.Name, fromDb.Name);
            }
        }

        [Test]
        public void Can_update_existing_player()
        {
            IPlayerRepository repository = new PlayerRepository();
            var player = new Player() { Id = 1, Name = "Hanley Ramirez", YahooId = 1 };
            repository.Save(player);

            // use session to try to load the product
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Player>(player.Id);
                Assert.AreEqual(player.Name, fromDb.Name);
            }
        }

        [Test]
        public void Can_remove_existing_player()
        {
            var player = new Player() { Id = 1 };
            IPlayerRepository repository = new PlayerRepository();
            repository.Delete(player);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Player>(player.Id);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_player_by_id()
        {
            IPlayerRepository repository = new PlayerRepository();
            var fromDb = repository.GetById(1);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual("Prince Fielder", fromDb.Name);
        }

        [Test]
        public void Can_get_existing_player_by_name()
        {
            IPlayerRepository repository = new PlayerRepository();
            var fromDb = repository.GetByName("Prince Fielder");
            Assert.IsNotNull(fromDb);
            Assert.AreEqual("Prince Fielder", fromDb.Name);
        }

        [Test]
        public void Can_get_existing_player_by_yahoo_id()
        {
            IPlayerRepository repository = new PlayerRepository();
            var fromDb = repository.GetByYahooId(1);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(1, fromDb.YahooId);
        }
    }
}
