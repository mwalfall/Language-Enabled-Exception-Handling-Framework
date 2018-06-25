using System;
using System.Collections.Generic;
using Model.DTO;
using Model.Resources;

namespace Model.Exceptions
{
    public class JobPostingException : BusinessException
    {
        /// <summary>
        /// Job Posting Exception
        /// </summary>
        /// <param name="resourceKey">ErrorMessageKeys ResourceKey</param>
        /// <param name="errorLogMessage">Detailed technical error message</param>
        /// <param name="additionalContent">List of strings containing additional error messages</param>
        /// <param name="innerException">Inner Exception</param>
        /// 
        public JobPostingException(ErrorMessageKeys resourceKey, string errorLogMessage, List<string> additionalContent = null, Exception innerException = null)
            : base(resourceKey, errorLogMessage, additionalContent, innerException)
        { }

        /// <summary>
        /// Job Posting exception
        /// </summary>
        /// <param name="resourceKey">ErrorMessageKeys ResourceKey</param>
        /// <param name="errorItems">List of ErrorItem objects derived from the ModelStateDictionary object</param>
        /// <param name="innerException">Inner Exception</param>
        /// 
        public JobPostingException(ErrorMessageKeys resourceKey, IEnumerable<ErrorItem> errorItems, Exception innerException = null)
            : base(resourceKey, errorItems, innerException)
        { }

        /// <summary>
        /// Job Posting exception
        /// </summary>
        /// <param name="resourceKey">ErrorMessageKeys ResourceKey</param>
        /// <param name="errorItems">List of ErrorItem objects derived from the ModelStateDictionary object</param>
        /// <param name="additionalContent">List of strings containing additional error messages</param>
        /// <param name="innerException">Inner Exception</param>
        /// 
        public JobPostingException(ErrorMessageKeys resourceKey, IEnumerable<ErrorItem> errorItems, IEnumerable<string> additionalContent, Exception innerException = null)
            : base(resourceKey, errorItems, additionalContent, innerException)
        { }
    }
}
