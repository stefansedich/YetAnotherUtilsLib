using System;
using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;
using YetAnotherUtilsLib.Core.NHibernate;

namespace YetAnotherUtilsLib.Core.Tests.NHibernate
{
    [TestFixture]
    public class UnitOfWorkFactoryFixture
    {
        private ISessionFactory _sessionFactory;
        private ISession _session;
        private UnitOfWorkFactory _factory;

        [SetUp]
        public void SetUp()
        {
            var factoryBuilder = MockRepository.GenerateStub<INHibernateSessionFactoryBuilder>();
            _sessionFactory = MockRepository.GenerateStub<ISessionFactory>();
            _session = MockRepository.GenerateStub<ISession>();

            factoryBuilder.Stub(builder => builder.BuildSessionFactory())
                .Return(_sessionFactory);

            _sessionFactory.Stub(fac => fac.OpenSession())
                .Return(_session);

            _factory = new UnitOfWorkFactory(factoryBuilder);
        }

        [Test]
        public void Create_Creates_New_UnitOfWork_Opening_Session_And_Starting_Transaction()
        {
            // Arrange

            // Act
            using (var uow = _factory.Create())
            {
                // Assert
                _session.AssertWasCalled(sess => sess.BeginTransaction());

                Assert.IsNotNull(uow);
                Assert.AreEqual(FlushMode.Commit, _session.FlushMode);
                Assert.AreEqual(uow.CurrentSession, _session);
            }
        }
        
        [Test]
        public void Create_Sets_CurrentUnitOfWork_To_Started_UnitOfWork()
        {
            // Arrange

            // Act
            using (var uow = _factory.Create())
            {
                // Assert
                Assert.AreEqual(uow, UnitOfWorkFactory.CurrentUnitOfWork);
            }
        }

        [Test]
        public void Create_Throws_If_UnitOfWork_Already_Running()
        {
            // Arrange
            
            // Act
            try
            {
                using (_factory.Create())
                {
                    _factory.Create();
                }

                Assert.Fail();
            } 
            catch(InvalidOperationException)
            {
                // Assert
            }
        }

        [Test]
        public void CurrentUnitOfWork_Exceptions_If_UnitOfWork_Not_Started()
        {
            // Arrange
            
            try
            {
                // Act
                var uow = UnitOfWorkFactory.CurrentUnitOfWork;
                Assert.IsNotNull(uow);
            }
            catch(InvalidOperationException)
            {
                // Assert
                // Should exception as CurrentUnitOfWork should be empty
            }
        }
    }
}