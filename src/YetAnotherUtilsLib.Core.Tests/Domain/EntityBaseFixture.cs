using System;
using NUnit.Framework;
using YetAnotherUtilsLib.Core.Domain;

namespace YetAnotherUtilsLib.Core.Tests.Domain
{
    [TestFixture]
    public class EntityBaseFixture
    {
        [Test]
        public void Two_Entities_Of_Same_Type_With_Same_Id_Are_Equal()
        {
            var id = Guid.NewGuid();

            var a = new TestEntity() { Id = id };
            var b = new TestEntity() { Id = id };

            Assert.IsTrue(a == b);
        }

        [Test]
        public void Two_Transient_Entities_Of_Same_Type_Are_Not_Equal()
        {
            var id = Guid.NewGuid();

            var a = new TestEntity();
            var b = new TestEntity();

            Assert.IsFalse(a == b);
        }

        [Test]
        public void Two_Entities_Of_Same_Type_With_Different_Ids_Are_Not_Equal()
        {
            var a = new TestEntity() { Id = Guid.NewGuid() };
            var b = new TestEntity() { Id = Guid.NewGuid() };

            Assert.IsFalse(a == b);
        }

        [Test]
        public void Two_Entities_Of_different_Types_With_Same_Id_Are_Not_Equal()
        {
            var id = Guid.NewGuid();

            var a = new TestEntity() { Id = id };
            var b = new TestEntity2() { Id = id };

            Assert.IsFalse(a == b);
        }

        [Test]
        public void Two_Transient_Entities_Of_Different_Types_Are_Not_Equal()
        {
            var id = Guid.NewGuid();

            var a = new TestEntity();
            var b = new TestEntity2();

            Assert.IsFalse(a == b);
        }

        [Test]
        public void New_Entity_Is_Transient()
        {
            var modelA = new TestEntity();

            Assert.IsTrue(modelA.IsTransient);
        }

        [Test]
        public void Entity_Is_Not_Transient_When_Id_Is_Not_Empty()
        {
            var modelA = new TestEntity() { Id = Guid.NewGuid() };

            Assert.IsFalse(modelA.IsTransient);
        }

        [Test]
        public void GetHashCode_Returns_HashCode_Of_Id()
        {
            // Arrange
            var model = new TestEntity {Id = Guid.NewGuid()};

            // Act
            int hashCode = model.GetHashCode();

            // Assert
            Assert.AreEqual(hashCode, model.Id.GetHashCode());
        }

        internal class TestEntity : EntityBase<Guid> { }
        internal class TestEntity2 : EntityBase<Guid> { }
    }
}