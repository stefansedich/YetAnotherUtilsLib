using System.Data;
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
        private IUnitOfWorkFactory	_unitOfWorkFactory;
        private ISession _session;
        private ITransaction _transaction;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkFactory = MockRepository.GenerateStub<IUnitOfWorkFactory>();
            var container = MockRepository.GenerateStub<IWindsorContainer>();

            IoC.InitializeWith(container);

            container.Stub(cont => cont.Resolve<IUnitOfWorkFactory>())
                .Return(_unitOfWorkFactory);

            _session = MockRepository.GenerateStub<ISession>();
            _transaction = MockRepository.GenerateStub<ITransaction>();
        }

        [Test]
        public void Start_Uses_Factory_To_Create_New_UnitOfWork()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(null, null);

            _unitOfWorkFactory.Stub(factory => factory.Create())
                .Return(unitOfWork);

            // Act
            var uow = UnitOfWork.Start();

            // Assert
            Assert.AreEqual(unitOfWork, uow);
        }
        
        [Test]
        public void Commit_Commits_Transaction_If_Transaction_Active()
        {
            // Arrange
            _transaction.Stub(trans => trans.IsActive)
                .Return(true);
            
            var uow = new UnitOfWork(_session, _transaction);

            // Act
            uow.Commit();

            // Assert
            _transaction.AssertWasCalled(trans => trans.Commit());
        }

        [Test]
        public void Commit_Does_Not_Commit_Transaction_If_Transaction_InActive()
        {
            // Arrange
            var uow = new UnitOfWork(_session, _transaction);

            // Act
            uow.Commit();

            // Assert
            _transaction.AssertWasNotCalled(trans => trans.Commit());
        }

        [Test]
        public void RollBack_Rolls_Back_Transaction_If_Transaction_Active()
        {
            // Arrange
            _transaction.Stub(trans => trans.IsActive)
                .Return(true);

            var uow = new UnitOfWork(_session, _transaction);

            // Act
            uow.Rollback();

            // Assert
            _transaction.AssertWasCalled(trans => trans.Rollback());
        }

        [Test]
        public void RollBack_Does_Not_Roll_Back_Transaction_If_Transaction_InActive()
        {
            // Arrange
            var uow = new UnitOfWork(_session, _transaction);

            // Act
            uow.Rollback();

            // Assert
            _transaction.AssertWasNotCalled(trans => trans.Rollback());
        }

        [Test]
        public void Dispose_Clears_Current_UnitOfWork()
        {
            // Arrange
            var uow = new UnitOfWork(_session, _transaction);
            UnitOfWorkFactory.SetGlobalUnitOfWork(uow);

            Assert.IsNotNull(UnitOfWorkFactory.CurrentUnitOfWork);

            // Act
            uow.Dispose();

            // Assert
            Assert.IsNull(UnitOfWorkFactory.CurrentUnitOfWork);
            UnitOfWorkFactory.SetGlobalUnitOfWork(null);
        }

        [Test]
        public void Dispose_Closes_And_Disposes_Current_Session()
        {
            // Arrange
            var uow = new UnitOfWork(_session, _transaction);

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
            var uow = new UnitOfWork(_session, _transaction);

            // Act
            uow.Dispose();

            // Assert
            _transaction.AssertWasCalled(trans => trans.Dispose());
        }
    }
}