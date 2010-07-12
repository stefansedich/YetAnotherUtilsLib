using System;
using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;
using YetAnotherUtilsLib.Core.Domain;
using YetAnotherUtilsLib.Core.NHibernate;

namespace YetAnotherUtilsLib.Core.Tests.NHibernate
{
    [TestFixture]
    public class RepositoryFixture
    {
        private IUnitOfWork _unitOfWork;
        private ISession _session;
        private TestRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            _session = MockRepository.GenerateStub<ISession>();

            _unitOfWork.Stub(uow => uow.CurrentSession)
                .Return(_session);

            UnitOfWork.SetGlobalUnitOfWork(_unitOfWork);

            _repository = new TestRepository();
        }

        [Test]
        public void Get_Fetches_Using_Current_Session()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new TestEntity();

            _session.Stub(sess => sess.Get<TestEntity>(id))
                .Return(expected);

            // Act
            var result = _repository.Get(id);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Insert_Saves_Using_Current_Session()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new TestEntity();
            
            // Act
            _repository.Insert(entity);

            // Assert
            _session.AssertWasCalled(sess => sess.Save(entity));
        }

        [Test]
        public void Update_Updates_Using_Current_Session()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new TestEntity();

            // Act
            _repository.Update(entity);

            // Assert
            _session.AssertWasCalled(sess => sess.Update(entity));
        }

        [Test]
        public void Delete_Deletes_Using_Current_Session()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new TestEntity();

            // Act
            _repository.Delete(entity);

            // Assert
            _session.AssertWasCalled(sess => sess.Delete(entity));
        }

        public class TestEntity : EntityBase<Guid> { }
        public class TestRepository : Repository<TestEntity, Guid> { }
    }
}