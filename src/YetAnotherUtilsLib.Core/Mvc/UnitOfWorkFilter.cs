using System.Web.Mvc;
using YetAnotherUtilsLib.Core.NHibernate;

namespace YetAnotherUtilsLib.Core.Mvc
{
    public class UnitOfWorkFilter : ActionFilterAttribute
    {
        private IUnitOfWork _unitOfWork;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _unitOfWork = UnitOfWork.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            bool noExceptions = filterContext.Exception == null || filterContext.ExceptionHandled;
            bool modelValid = filterContext.Controller.ViewData.ModelState.IsValid;

            try
            {
                if (noExceptions && modelValid)
                    _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
            }
            finally
            {
                _unitOfWork.Dispose();    
            }
        }
    }
}