using Castle.Windsor;
using NUnit.Framework;
using Rhino.Mocks;
using YetAnotherUtilsLib.Core.Container;

namespace YetAnotherUtilsLib.Core.Tests.Container
{
    [TestFixture]
    public class IoCFixture
    {

        [Test]
        public void Initialize_Sets_Container()
        {
            // Arrange
            var container = new WindsorContainer();

            Assert.AreEqual(null, IoC.Container);

            // Act
            IoC.InitializeWith(container);

            // Assert
            Assert.AreEqual(container, IoC.Container);
        }

        [Test]
        public void Resolve_Resolves_Using_Container()
        {
            // Arrange
            var container = MockRepository.GenerateStub<IWindsorContainer>();
            var foo = new Foo();

            container.Stub(cont => cont.Resolve<IFoo>())
                .Return(foo);

            IoC.InitializeWith(container);

            // Act
            var resolved = IoC.Resolve<IFoo>();

            // Assert
            Assert.AreEqual(foo, resolved);
        }
    }

    public interface IFoo {}
    public class Foo : IFoo {}
}