using System;
using System.Collections.Generic;
using System.Linq;
using Model.DTO;
using Model.Resources;

namespace Model.Exceptions
{
    public class BusinessException : Exception
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="resourceKey">ErrorMessageKeys enum with user friendly error message</param>
        /// <param name="errorLogMessage">String containing technical error message</param>
        /// <param name="additionalContent">List of strings with additional error information</param>
        /// <param name="innerException">Inner exception</param>
        /// 
        public BusinessException(ErrorMessageKeys resourceKey, string errorLogMessage, List<string> additionalContent = null, Exception innerException = null)
            : base(errorLogMessage, innerException)
        {
            base.Data.Add("ResourceKey", resourceKey);
            base.Data.Add("AdditionalContent", additionalContent);
            base.Data.Add("ErrorItems", null);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="resourceKey">ErrorMessageKeys enum with user friendly error message</param>
        /// <param name="errorItems">String with technical error message. (Obtained form ModelState.GetErrorMessage method.)</param>
        /// <param name="innerException">Inner exception</param>
        /// 
        public BusinessException(ErrorMessageKeys resourceKey, IEnumerable<ErrorItem> errorItems, Exception innerException = null)
            : base(GetTechnicalMessage(errorItems), innerException)
        {
            base.Data.Add("ResourceKey", resourceKey);
            base.Data.Add("AdditionalContent", null);
            base.Data.Add("ErrorItems", errorItems);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="resourceKey">ErrorMessageKeys enum with user friendly error message</param>
        /// <param name="errorItems">String with technical error message. (Obtained form ModelState.GetErrorMessage method.)</param>
        /// <param name="additionalContent">List of strings with additional error information</param>
        /// <param name="innerException">Inner exception</param>
        /// 
        public BusinessException(ErrorMessageKeys resourceKey, IEnumerable<ErrorItem> errorItems, IEnumerable<string> additionalContent = null, Exception innerException = null)
            : base(GetTechnicalMessage(errorItems, additionalContent), innerException)
        {
            base.Data.Add("ResourceKey", resourceKey);
            base.Data.Add("AdditionalContent", additionalContent);
            base.Data.Add("ErrorItems", errorItems);
        }
        #endregion Constructors

        #region Public Methods

        public ErrorMessageKeys GetErrorMessageKeys()
        {
            return (ErrorMessageKeys)base.Data["ResourceKey"];
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates the technical error messages.
        /// </summary>
        /// <param name="errorItems">List of errors returned from ValidationAttribute objects.</param>
        /// <returns>String containing the derived technical error message</returns>
        /// 
        private static string GetTechnicalMessage(IEnumerable<ErrorItem> errorItems)
        {
            // Concatenate all the error messages into a string.
            return errorItems.Aggregate(string.Empty, (current, errorItem) => current + (errorItem.Message + " "));
        }
        
        /// <summary>
        /// Concatenate the technical error messages
        /// </summary>
        /// <param name="errorItems">List of ErrorItem objects</param>
        /// <param name="additonalContent">List of string objects</param>
        /// 
        private static string GetTechnicalMessage(IEnumerable<ErrorItem> errorItems, IEnumerable<string> additonalContent)
        {
            var technicalMessage = string.Empty;

            if (errorItems != null)
                technicalMessage = errorItems.Aggregate(technicalMessage, (current, item) => current + (item + " : "));

            if (additonalContent != null)
                technicalMessage = additonalContent.Aggregate(technicalMessage, (current, content) => current + (content + " : "));

            return technicalMessage.Substring(0, technicalMessage.LastIndexOf(':') - 1);
        }
        #endregion Private Methods
    }
}
