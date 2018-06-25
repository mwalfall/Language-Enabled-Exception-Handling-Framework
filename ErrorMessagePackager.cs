using System;
using System.Collections.Generic;
using Model.Resources;

namespace Site.Utility
{
    public static class ErrorMessagePackager
    {
        #region Public Methods

        /// <summary>
        /// Extract the UserResourceKeys enum from the exception and 
        /// then calls the overloaded method to obtain a strin representation of the enum.
        /// </summary>
        /// <param name="exception">Thrown exception</param>
        /// 
        public static string GetResourceKeyString(Exception exception)
        {
            if (exception == null)
                return string.Empty;

            return exception.Data.Contains("ResourceKey")
                ? ResourceManager.GetResourceKeyString((ErrorMessageKeys)exception.Data["ResourceKey"])
                : string.Empty;
        }

        /// <summary>
        /// Obtain the error message that will be logged.
        /// </summary>
        /// <param name="exception">The thrown exception</param>
        /// 
        public static string GetErrorLogMessage(Exception exception)
        {
            return exception == null
                ? string.Empty
                : exception.Message;
        }

        /// <summary>
        /// Get the list of addition user error content that is to be sent to the client.
        /// </summary>
        /// <param name="exception">The thrown exception</param>
        /// 
        public static List<string> GetAdditionalContent(Exception exception)
        {
            if (exception == null)
                return null;

            if (exception.Data.Contains("AdditionalContent") && exception.Data["AdditionalContent"] != null)
                return (List<string>)exception.Data["AdditionalContent"];

            return new List<string>();
        }
        #endregion Public Methods
    }
}