using System.Collections.Generic;
using System.Reflection;
using SysResources = System.Resources;
using System.Threading;
using Model.Resources;
using Models.api.JobFields;

namespace Site.Utility
{
    public static class LanguageTranslator
    {
        #region Public Methods

        /// <summary>
        /// Return the language based value for the submitted resourceKey
        /// </summary>
        /// <param name="resourceKey">Resource Key</param>
        /// <param name="resourceType">Resource file that is to be searched.</param>
        /// <returns>string containing the language-based message</returns>
        /// 
        public static string Translate(string resourceKey, ResourceType resourceType)
        {
            if (string.IsNullOrWhiteSpace(resourceKey))
                return string.Empty;

            var culture = Thread.CurrentThread.CurrentUICulture;

            var resourceManager = GetResourceManager(resourceType);

            return resourceManager.GetString(resourceKey, culture);
        }

        /// <summary>
        /// Return the language based value for the submitted resourceKey
        /// </summary>
        /// <param name="enumIntValue">Integer value for the ErrorMessageKeys enum</param>
        /// <param name="resourceType">Resource file that is to be searched.</param>
        /// <returns>string containing the language-based message</returns>
        /// 
        public static string Translate(int enumIntValue, ResourceType resourceType)
        {
            var key = (ErrorMessageKeys) enumIntValue;
            var resourceKey = key.ToString("g");

            if (string.IsNullOrWhiteSpace(resourceKey))
                return string.Empty;

            var culture = Thread.CurrentThread.CurrentUICulture;

            var resourceManager = GetResourceManager(resourceType);

            return resourceManager.GetString(resourceKey, culture);
        }

        /// <summary>
        /// Takes a JobField object and translates the Label to the language specified by the client thread.
        /// </summary>
        /// <param name="fields">Lsit of JobField object</param>
        /// <param name="resourceType">ResourceType object contains the names of resource fields that are available</param>
        /// <returns>Translated Label</returns>
        /// 
        public static List<JobField> Translate(List<JobField> fields, ResourceType resourceType)
        {
            if (fields == null)
                return null;

            var culture = Thread.CurrentThread.CurrentUICulture;

            var resourceManager = GetResourceManager(resourceType);

            foreach (var field in fields)
            {
                if (string.IsNullOrWhiteSpace(field.Label))
                    continue;

                var labelName = resourceManager.GetString(field.Label.Trim(), culture);
                if (!string.IsNullOrWhiteSpace(labelName))
                    field.Label = labelName;
            }
            return fields;
        }
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Returns the resource manager that is to be used to provide language translation.
        /// </summary>
        /// <param name="resourceType">ResourceType object</param>
        /// <returns>ResourceManager object</returns>
        /// 
        private static SysResources.ResourceManager GetResourceManager(ResourceType resourceType)
        {
            var resourceManager = new SysResources.ResourceManager("Resources.UI", Assembly.GetExecutingAssembly());

            if (resourceType == ResourceType.Error)
                resourceManager = new SysResources.ResourceManager("Resources.Error", Assembly.GetExecutingAssembly());

            if (resourceType == ResourceType.Labels)
                resourceManager = new SysResources.ResourceManager("Resources.Labels", Assembly.GetExecutingAssembly());

            if (resourceType == ResourceType.Messages)
                resourceManager = new SysResources.ResourceManager("Resources.Messages", Assembly.GetExecutingAssembly());

            return resourceManager;
        }
        #endregion Private Methods
    }
}