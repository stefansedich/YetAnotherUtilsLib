using System;
using System.Web.Mvc;
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
        private ActionExecutedContext _actionExecutedContext;
        private IUnitOfWorkFactory _unitOfWorkFactory;

        [SetUp]
        public void SetUp()
        {
            _filter = new UnitOfWorkFilter();
            _actionExecutedContext = new ActionExecutedContext {Controller = new MockController()};
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
        
        [Test]
        public void OnActionExecuted_Commits_UnitOfWork_If_No_Exceptions_And_ModelState_Valid()
        {
            // Arrange
            var unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();

            _unitOfWorkFactory.Stub(factory => factory.Create())
                .Return(unitOfWork);

            _filter.OnActionExecuting(null); 

            // Act
            _filter.OnActionExecuted(_actionExecutedContext);

            // Assert
            unitOfWork.AssertWasCalled(uow => uow.Commit());
        }

        [Test]
        public void OnActionExecuted_RollsBack_UnitOfWork_If_ModelState_Invalid()
        {
            // Arrange
            var unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();

            _unitOfWorkFactory.Stub(factory => factory.Create())
                .Return(unitOfWork);

            _filter.OnActionExecuting(null); 

            ((MockController) _actionExecutedContext.Controller).SetModelStateInvalid();

            // Act
            _filter.OnActionExecuted(_actionExecutedContext);

            // Assert
            unitOfWork.AssertWasCalled(uow => uow.Rollback());
        }

        [Test]
        public void OnActionExecuted_RollsBack_UnitOfWork_If_Exception_Thrown_Inside_Action()
        {
            // Arrange
            var unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();

            _unitOfWorkFactory.Stub(factory => factory.Create())
                .Return(unitOfWork);

            _filter.OnActionExecuting(null); 

            _actionExecutedContext.Exception = new Exception();

            // Act
            _filter.OnActionExecuted(_actionExecutedContext);

            // Assert
            unitOfWork.AssertWasCalled(uow => uow.Rollback());
        }

        [Test]
        public void OnActionExecuted_Disposes_UnitOfWork()
        {
            // Arrange
            var unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();

            _unitOfWorkFactory.Stub(factory => factory.Create())
                .Return(unitOfWork);

            _filter.OnActionExecuting(null);

            // Act
            _filter.OnActionExecuted(_actionExecutedContext);

            // Assert
            unitOfWork.AssertWasCalled(uow => uow.Dispose());
        }

        public class MockController : Controller
        {
            public MockController()
            {
                ViewData = new ViewDataDictionary();
            }

            public void SetModelStateInvalid()
            {
                ViewData.ModelState.AddModelError("test", "test");
            }
        }
    }
}