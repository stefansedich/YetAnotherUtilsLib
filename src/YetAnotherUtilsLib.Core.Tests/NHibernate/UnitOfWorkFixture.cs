using Castle.Windsor;
using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;
using YetAnotherUtilsLib.Core.Container;
using YetAnotherUtilsLib.Core.NHibernate;

namespace YetAnotherUtilsLib.Core.Tests.NHibernate
{
    [TestFixture]
    public class UnitOfWorkFixture
    {
        private ISessionFactory _factory;
        private INHibernateSessionFactoryBuilder _factoryBuilder;
        private ISession _session;

        [SetUp]
        public void SetUp()
        {
            var container = MockRepository.GenerateStub<IWindsorContainer>();

            _factoryBuilder = MockRepository.GenerateStub<INHibernateSessionFactoryBuilder>();
            _factory = MockRepository.GenerateStub<ISessionFactory>();
            _session = MockRepository.GenerateStub<ISession>();

            IoC.InitializeWith(container);

            _factoryBuilder.Stub(builder => builder.BuildSessionFactory())
                .Return(_factory);

            _factory.Stub(fac => fac.OpenSession())
                .Return(_session);

            container.Stub(res => res.Resolve<INHibernateSessionFactoryBuilder>())
                .Return(_factoryBuilder);
        }

        [Test]
        public void Start_Creates_New_UnitOfWork_Opening_Session_And_Starting_Transaction()
        {
            // Arrange

            // Act
            var uow = UnitOfWork.Start();

            // Assert
            _session.AssertWasCalled(sess => sess.BeginTransaction());

            Assert.IsNotNull(uow);
            Assert.AreEqual(FlushMode.Commit, _session.FlushMode);
            Assert.AreEqual(uow.CurrentSession, _session);
        }

        [Test]
        public void Start_Sets_CurrentUnitOfWork_To_Started_UnitOfWork()
        {
            // Arrange

            // Act
            var uow = UnitOfWork.Start();

            // Assert
            Assert.AreEqual(uow, UnitOfWork.CurrentUnitOfWork);
        }

        [Test]
        public void Commit_Commits_Transaction_If_Transaction_Active()
        {
            // Arrange
            var transaction = MockRepository.GenerateStub<ITransaction>();

            transaction.Stub(trans => trans.IsActive)
                .Return(true);

            _session.Stub(sess => sess.BeginTransaction())
                .Return(transaction);

            var uow = UnitOfWork.Start();

            // Act
            uow.Commit();

            // Assert
            transaction.AssertWasCalled(trans => trans.Commit());
        }

        [Test]
        public void Commit_Does_Not_Commit_Transaction_If_Transaction_InActive()
        {
            // Arrange
            var transaction = MockRepository.GenerateStub<ITransaction>();

            _session.Stub(sess => sess.BeginTransaction())
                .Return(transaction);

            var uow = UnitOfWork.Start();

            // Act
            uow.Commit();

            // Assert
        }

        [Test]
        public void RollBack_Rolls_Back_Transaction_If_Transaction_Active()
        {
            // Arrange
            var transaction = MockRepository.GenerateStub<ITransaction>();

            transaction.Stub(trans => trans.IsActive)
                .Return(true);

            _session.Stub(sess => sess.BeginTransaction())
                .Return(transaction);

            var uow = UnitOfWork.Start();

            // Act
            uow.Rollback();

            // Assert
            transaction.AssertWasCalled(trans => trans.Rollback());
        }

        [Test]
        public void RollBack_Does_Not_Roll_Back_Transaction_If_Transaction_InActive()
        {
            // Arrange
            var transaction = MockRepository.GenerateStub<ITransaction>();

            _session.Stub(sess => sess.BeginTransaction())
                .Return(transaction);

            var uow = UnitOfWork.Start();

            // Act
            uow.Rollback();

            // Assert
            transaction.AssertWasNotCalled(trans => trans.Rollback());
        }

        [Test]
        public void Dispose_Clears_Current_UnitOfWork()
        {
            // Arrange
            var uow = UnitOfWork.Start();

            // Act
            uow.Dispose();

            // Assert
            Assert.IsNull(UnitOfWork.CurrentUnitOfWork);
        }

        [Test]
        public void Dispose_Closes_And_Disposes_Current_Session()
        {
            // Arrange
            var uow = UnitOfWork.Start();

            // Act
            uow.Dispose();

            // Assert
            _session.AssertWasCalled(sess => sess.Close());
            _session.AssertWasCalled(sess => sess.Dispose());
        }

        [Test]
        public void Dispose_Disposes_Current_Transaction()
        {
            // Arrange
            var transaction = MockRepository.GenerateStub<ITransaction>();

            _session.Stub(sess => sess.BeginTransaction())
                .Return(transaction);

            var uow = UnitOfWork.Start();

            // Act
            uow.Dispose();

            // Assert
            transaction.AssertWasCalled(trans => trans.Dispose());
        }
    }
}