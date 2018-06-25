using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Logging;
using Site.Helpers;
using Model.DTO;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using Client.Site.Utility;
using Model.Exceptions;
using Model.Resources;
using Framework.Security;
using Microsoft.Practices.ServiceLocation;

namespace Site.Attributes.Exceptions
{
    public class ApiErrorHandlerAttribute : ExceptionFilterAttribute
    {
        #region Public Method

        /// <summary>
        /// Initiate the loggong of the technical details of the exception.
        /// Configure the error message to be sent to the client.
        /// </summary>
        /// <param name="actionExecutedContext">HttpActionExecutedContext object</param>
        /// 
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            LogException(actionExecutedContext);

            if (actionExecutedContext.Exception is BusinessException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = GetDetailedErrorMessage(actionExecutedContext),
                    ReasonPhrase = GetMainErrorMessage(actionExecutedContext)
                });
            }

            if (actionExecutedContext.Exception is NotAuthenticatedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = GetDetailedErrorMessage(actionExecutedContext),
                    ReasonPhrase = GetMainErrorMessage(actionExecutedContext)
                });
            }

            if (actionExecutedContext.Exception is UnAuthorizedActionException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    Content = GetDetailedErrorMessage(actionExecutedContext),
                    ReasonPhrase = GetMainErrorMessage(actionExecutedContext)
                });
            }

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = GetDefaultDetailedErrorMessage(actionExecutedContext),
                ReasonPhrase = GetDefaultMainErrorMessage()
            });
        }
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Extract the error items from the exception object and places them in a list of ErrorItem objects.
        /// </summary>
        /// <param name="actionExecutedContext">HttpActionExecutedContext object</param>
        /// <returns>List of ErrorItem objects</returns>
        /// 
        private List<ErrorItem> GetErrorItems(HttpActionExecutedContext actionExecutedContext)
        {
            var additionalContent = ErrorMessagePackager.GetAdditionalContent(actionExecutedContext.Exception);

            return additionalContent.Select(messsage => new ErrorItem { Message = messsage }).ToList();
        }

        /// <summary>
        /// Configure the error message object that will contain the default user friendly language enabled
        /// message. The method is used with exceptions that do not derive from the BusinessException class.
        /// This exceptions can be considered ASP.NET platform excepions that are not captured by this
        /// application. For example, unable to conntect to database exception. In such cases a generic message
        /// is sent to the client.  
        /// </summary>
        /// <param name="actionExecutedContext">HttpActionExecutedContext object</param>
        /// 
        private HttpContent GetDefaultDetailedErrorMessage(HttpActionExecutedContext actionExecutedContext)
        {
            var errorMessage = new ErrorMessage
            {
                TechDetails = GetTechnicalDetails(actionExecutedContext),
                Message = LanguageTranslator.Translate(ResourceManager.GetResourceKeyString(ErrorMessageKeys.DefaultErrorMessage), ResourceType.Error)
            };

            return new StringContent(JsonConvert.SerializeObject(errorMessage));
        }

        /// <summary>
        /// Default main error message.
        /// </summary>
        ///
        private string GetDefaultMainErrorMessage()
        {
            return LanguageTranslator.Translate(ResourceManager.GetResourceKeyString(ErrorMessageKeys.DefaultErrorMessage), ResourceType.Error);
        }

        /// <summary>
        /// Configure the error message object that will be sent to the client application.
        /// </summary>
        /// <param name="actionExecutedContext">HttpActionExecutedContext object</param>
        ///
        private HttpContent GetDetailedErrorMessage(HttpActionExecutedContext actionExecutedContext)
        {
            var errorMessage = new ErrorMessage
            {
                TechDetails = GetTechnicalDetails(actionExecutedContext),
                Message = GetMainErrorMessage(actionExecutedContext),
                ErrorItems = GetErrorItems(actionExecutedContext),
                ResourceKey = GetResourceKey(actionExecutedContext)
            };

            return new StringContent(JsonConvert.SerializeObject(errorMessage));
        }

        /// <summary>
        /// Get the main user friendly language enabled error message that will be used by the client application.
        /// </summary>
        /// <param name="actionExecutedContext">HttpActionExecutedContext object</param>
        ///
        private string GetMainErrorMessage(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;
            return LanguageTranslator.Translate(ErrorMessagePackager.GetResourceKeyString(exception), ResourceType.Error);
        }

        private string GetResourceKey(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;
            var key = exception.Data["ResourceKey"];

            if (key == null)
            {
                return string.Empty;
            }
            else
            {
                return key.ToString();
            }
        }

        /// <summary>
        /// Returns the detailed exception message if code is being executed in the development environment.
        /// </summary>
        /// <param name="actionExecutedContext">HttpActionExecutedContext object</param>
        /// <returns>String containing all the exception messages. Including inner exception messages.</returns>
        /// 
        private string GetTechnicalDetails(HttpActionExecutedContext actionExecutedContext)
        {
            var exceptionMessages = string.Empty;

            if (ShowTechnicalDetails())
            {
                var exception = actionExecutedContext.Exception;
                exceptionMessages = exception.Message;
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                    exceptionMessages += " -- " + exception.Message;
                }
            }
            return exceptionMessages;
        }

        private bool ShowTechnicalDetails()
        {
            try
            {
                if (AppHelpers.InDevelopment())
                {
                    return true;
                }

                var authManager = ServiceLocator.Current.GetInstance<Security.IAuthenticationManager>();
                var config = ServiceLocator.Current.GetInstance<Configuration.IConfigurationManager>();
                var sessionToken = authManager.GetSessionToken();

                if (sessionToken.TenantID == config.GetAppSetting<int>("Security.OrgRootID", 2))
                {
                    return true;
                }
            }
            catch
            {
                // Just return false
            }

            return false;
        }

        /// <summary>
        /// Log the exception.
        /// </summary>
        /// <param name="actionExecutedContext">ExceptionContext object</param>
        /// 
        private void LogException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is DbEntityValidationException)
            {
                var ex = (DbEntityValidationException) actionExecutedContext.Exception;
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Logger.WriteLog(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Logger.WriteLog(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
            }
            Logger.WriteError(actionExecutedContext.Exception);
        }
        #endregion Private Methods
    }
}