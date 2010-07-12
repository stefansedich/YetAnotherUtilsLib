using Castle.Windsor;
using NUnit.Framework;
using Rhino.Mocks;
using YetAnotherUtilsLib.Core.Container;
using YetAnotherUtilsLib.Core.Mvc;
using YetAnotherUtilsLib.Core.NHibernate;

namespace YetAnotherUtilsLib.Core.Tests.Mvc
{
    [TestFixture]
    public class UnitOfWorkFilterFixture
    {
        private UnitOfWorkFilter _filter;
        private IUnitOfWorkFactory _unitOfWorkFactory;

        [SetUp]
        public void SetUp()
        {
            _filter = new UnitOfWorkFilter();

            _unitOfWorkFactory = MockRepository.GenerateStub<IUnitOfWorkFactory>();
            
            var container = MockRepository.GenerateStub<IWindsorContainer>();
            IoC.InitializeWith(container);

            container.Stub(cont => cont.Resolve<IUnitOfWorkFactory>())
                .Return(_unitOfWorkFactory);
        }

        [Test]
        public void OnActionExecuting_Starts_New_UnitOfWork()
        {
            // Arrange
            
            // Act
            _filter.OnActionExecuting(null);

            // Assert
            _unitOfWorkFactory.AssertWasCalled(factory => factory.Create());
        }
    }
}