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
    public class PositionRepository_Fixture
    {
        private ISessionFactory _sessionFactory;
        private Configuration _configuration;
        private INDbUnitTest _mySqlDatabase;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _configuration = new Configuration();
            _configuration.Configure();
            _configuration.AddAssembly(typeof(Position).Assembly);
            _sessionFactory = _configuration.BuildSessionFactory();

            _mySqlDatabase = new SqlDbUnitTest(Properties.Settings.Default.NHibernateTrainingConnectionString);

            _mySqlDatabase.ReadXmlSchema(@"..\..\NHibernateTrainingDataSet.xsd");
            _mySqlDatabase.ReadXml(@"..\..\PositionRepositoryTestData.xml");
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
        public void Can_add_new_Position()
        {
            var Position = new Position { Id = "3B", Name = "Third Base" };
            IPositionRepository repository = new PositionRepository();
            repository.Save(Position);

            // use session to try to load the product
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Position>(Position.Id);
                // Test that the product was successfully inserted
                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(Position, fromDb);
                Assert.AreEqual(Position.Name, fromDb.Name);
            }
        }

        [Test]
        public void Can_update_existing_Position()
        {
            IPositionRepository repository = new PositionRepository();
            var Position = new Position() { Id = "1B", Type = PositionType.P };
            repository.Save(Position);

            // use session to try to load the product
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Position>(Position.Id);
                Assert.AreEqual(Position.Type, fromDb.Type);
            }
        }

        [Test]
        public void Can_remove_existing_Position()
        {
            var Position = new Position() { Id = "1B" };
            IPositionRepository repository = new PositionRepository();
            repository.Delete(Position);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Position>(Position.Id);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void Can_get_existing_Position_by_id()
        {
            IPositionRepository repository = new PositionRepository();
            var fromDb = repository.GetById("1B");
            Assert.IsNotNull(fromDb);
            Assert.AreEqual("First Base", fromDb.Name);
        }
    }
}
