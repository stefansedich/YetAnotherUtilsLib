using System;
using System.Collections.Generic;
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
            _repository = new TestRepository();
            _unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            _session = MockRepository.GenerateStub<ISession>();

            _unitOfWork.Stub(uow => uow.CurrentSession)
                .Return(_session);

            UnitOfWorkFactory.SetGlobalUnitOfWork(_unitOfWork);
        }

        [TearDown]
        public void TearDown()
        {
            UnitOfWorkFactory.SetGlobalUnitOfWork(null);
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
        public void GetAll_Fetches_All_Using_Current_Session()
        {
            // Arrange
            var items = new List<TestEntity> {new TestEntity(), new TestEntity()};

            var criteria = MockRepository.GenerateStub<ICriteria>();

            _session.Stub(sess => sess.CreateCriteria(typeof (TestEntity)))
                .Return(criteria);

            criteria.Stub(crit => crit.List<TestEntity>())
                .Return(items);

            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.AreEqual(items, result);
        }

        [Test]
        public void Insert_Saves_Using_Current_Session()
        {
            // Arrange
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
            var entity = new TestEntity();

            // Act
            _repository.Delete(entity);

            // Assert
            _session.AssertWasCalled(sess => sess.Delete(entity));
        }

        public class TestEntity : EntityBase<Guid> { }
        public class TestRepository : RepositoryBase<TestEntity, Guid> { }
    }
}