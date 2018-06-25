using System.Web;
using System.Web.Mvc;
using Core.Logging;

namespace Helpers.Attributes
{
    public class JdErrorHandlerAttribute : HandleErrorAttribute
    {
        #region Public Methods

        /// <summary>
        /// Exception handler.
        /// </summary>
        /// <param name="filterContext">ExceptionContext object</param>
        /// 
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            // Only process exceptions with Http Code = 500.
            if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500)
                return;

            LogException(filterContext);

            // Display error via view or ajax message.
            NotifyUserOfException(filterContext);
        }
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Log the exception.
        /// </summary>
        /// <param name="filterContext">ExceptionContext object</param>
        /// 
        private void LogException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            while (exception.InnerException != null)
                exception = exception.InnerException;

            Logger.WriteError(exception);
        }

        /// <summary>
        /// Determines how the user is to be notified of the exception. 
        /// Presently via a view or ajax response.
        /// </summary>
        /// <param name="filterContext">ExceptionContext object</param>
        /// 
        private void NotifyUserOfException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                NotifyUserViaAjax(filterContext);
            else
                NotifyUserViaView(filterContext);
        }

        /// <summary>
        /// Notify user of the exception via ajax.
        /// </summary>
        /// <param name="filterContext">ExceptionContext object</param>
        /// 
        private void NotifyUserViaAjax(ExceptionContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            filterContext.ExceptionHandled = true;
            filterContext.Result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = filterContext.Exception.Message

            };
        }

        /// <summary>
        /// Notify user of exception via a View.
        /// </summary>
        /// <param name="filterContext">ExceptionContext object</param>
        /// 
        private void NotifyUserViaView(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            var errorMessage = filterContext.Exception.Message;
            var tempData = new TempDataDictionary();
            tempData.Add("ErrorMessage", errorMessage);

            filterContext.Result = new ViewResult { ViewName = "Error", TempData = tempData };
        }

        #endregion Private Methods
    }
}