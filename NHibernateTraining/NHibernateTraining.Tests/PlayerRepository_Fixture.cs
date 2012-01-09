using NHibernate;
using NHibernate.Cfg;
using NHibernateTraining.Domain;
using NUnit.Framework;
using NHibernate.Tool.hbm2ddl;
using NHibernateTraining.Repositories;
using System.Collections.Generic;
using System;

namespace NHibernateTraining.Tests
{
    [TestFixture]
    public class PlayerRepository_Fixture
    {
        private ISessionFactory _sessionFactory;
        private Configuration _configuration;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _configuration = new Configuration();
            _configuration.Configure();
            _configuration.AddAssembly(typeof(Player).Assembly);
            _sessionFactory = _configuration.BuildSessionFactory();
        }

        private readonly Player[] _players = new[]
                 {
                     new Player() { Name = "Albert Pujols" },
                     new Player() { Name = "Mike Stanton" },
                     new Player() { Name = "Jose Reyes" },
                     new Player() { Name = "Prince Fielder" },
                     new Player() { Name = "Felix Hernandez" }
                 };

        private void CreateInitialData()
        {
            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var player in _players)
                    session.Save(player);
                transaction.Commit();
            }
        }

        [SetUp]
        public void SetupContext()
        {
            new SchemaExport(_configuration).Execute(false, true, false);
            CreateInitialData();
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
            var player = _players[0];
            player.Name = "Hanley Ramirez";
            IPlayerRepository repository = new PlayerRepository();
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
            var player = _players[0];
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
            var fromDb = repository.GetById(_players[1].Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_players[1], fromDb);
            Assert.AreEqual(_players[1].Name, fromDb.Name);
        }
    }
}
