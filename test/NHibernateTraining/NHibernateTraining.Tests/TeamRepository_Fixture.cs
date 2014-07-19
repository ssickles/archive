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
    public class TeamRepository_Fixture
    {
        private ISessionFactory _sessionFactory;
        private Configuration _configuration;
        private INDbUnitTest _mySqlDatabase;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _configuration = new Configuration();
            _configuration.Configure();
            _configuration.AddAssembly(typeof(Team).Assembly);
            _sessionFactory = _configuration.BuildSessionFactory();

            _mySqlDatabase = new SqlDbUnitTest(Properties.Settings.Default.NHibernateTrainingConnectionString);

            _mySqlDatabase.ReadXmlSchema(@"..\..\NHibernateTrainingDataSet.xsd");
            _mySqlDatabase.ReadXml(@"..\..\TeamRepositoryTestData.xml");
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
        public void Can_add_new_Team()
        {
            var Team = new Team { City = "Pittsburgh" };
            ITeamRepository repository = new TeamRepository();
            repository.Save(Team);

            // use session to try to load the product
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Team>(Team.Id);
                // Test that the product was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(Team, fromDb);
                Assert.AreEqual(Team.City, fromDb.City);
            }
        }

        [Test]
        public void Can_update_existing_Team()
        {
            ITeamRepository repository = new TeamRepository();
            var Team = new Team() { Id = 1, City = "Atlanta" };
            repository.Save(Team);

            // use session to try to load the product
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Team>(Team.Id);
                Assert.AreEqual(Team.City, fromDb.City);
            }
        }

        [Test]
        public void Can_remove_existing_Team()
        {
            var Team = new Team() { Id = 1 };
            ITeamRepository repository = new TeamRepository();
            repository.Delete(Team);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Team>(Team.Id);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_Team_by_id()
        {
            ITeamRepository repository = new TeamRepository();
            var fromDb = repository.GetById(1);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual("Miami", fromDb.City);
        }
    }
}
