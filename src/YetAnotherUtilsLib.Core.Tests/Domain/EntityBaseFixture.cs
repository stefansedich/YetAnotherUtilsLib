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

            var a = new TestEnttiy() { Id = id };
            var b = new TestEnttiy() { Id = id };

            Assert.IsTrue(a == b);
        }

        [Test]
        public void Two_Transient_Entities_Of_Same_Type_Are_Not_Equal()
        {
            var id = Guid.NewGuid();

            var a = new TestEnttiy();
            var b = new TestEnttiy();

            Assert.IsFalse(a == b);
        }

        [Test]
        public void Two_Entities_Of_Same_Type_With_Different_Ids_Are_Not_Equal()
        {
            var a = new TestEnttiy() { Id = Guid.NewGuid() };
            var b = new TestEnttiy() { Id = Guid.NewGuid() };

            Assert.IsFalse(a == b);
        }

        [Test]
        public void Two_Entities_Of_different_Types_With_Same_Id_Are_Not_Equal()
        {
            var id = Guid.NewGuid();

            var a = new TestEnttiy() { Id = id };
            var b = new TestEnttiy2() { Id = id };

            Assert.IsFalse(a == b);
        }

        [Test]
        public void Two_Transient_Entities_Of_Different_Types_Are_Not_Equal()
        {
            var id = Guid.NewGuid();

            var a = new TestEnttiy();
            var b = new TestEnttiy2();

            Assert.IsFalse(a == b);
        }

        [Test]
        public void New_Entity_Is_Transient()
        {
            var modelA = new TestEnttiy();

            Assert.IsTrue(modelA.IsTransient);
        }

        [Test]
        public void Entity_Is_Not_Transient_When_Id_Is_Not_Empty()
        {
            var modelA = new TestEnttiy() { Id = Guid.NewGuid() };

            Assert.IsFalse(modelA.IsTransient);
        }

        internal class TestEnttiy : EntityBase<Guid> { }
        internal class TestEnttiy2 : EntityBase<Guid> { }
    }
}