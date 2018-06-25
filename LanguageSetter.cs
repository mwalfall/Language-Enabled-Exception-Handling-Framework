using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http.Controllers;
using Site.Resources;

namespace Site.Utility
{
    /// <summary>
    /// This class sets the language for the application. It reads in the language preferences from the 
    /// HttpActionContext object and sets the language to the first match for the application's
    /// implemented languages.
    /// </summary>
    public static class LanguageSetter
    {
        #region Public Methods

        /// <summary>
        /// Set the language for the executing thread. 
        /// </summary>
        /// <param name="actionContext">HttpActionContext object</param>
        /// 
        public static void Set(HttpActionContext actionContext)
        {
            string languageToUse;

            var userLanguages = actionContext.Request.Headers.AcceptLanguage;
            if (userLanguages != null && userLanguages.Count > 0)
            {
                var languages = GetLanguageList(userLanguages);
                languageToUse = GetLanguageToUse(languages);
            }
            else
                languageToUse = GetDefaultLanguage();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(languageToUse);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        /// <summary>
        /// Gets the default language for the application. An entry in the Web.config file is first used.
        /// If a value is not found Engish is used.
        /// </summary>
        /// <returns>String containing default language</returns>
        /// 
        public static string GetDefaultLanguage()
        {
            var defaultLanguage = System.Web.Configuration.WebConfigurationManager.AppSettings["DefaultLanguage"];

            return defaultLanguage ?? "en";
        }
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Determine the language to be used by the executing thread.
        /// </summary>
        /// <param name="languages">List of languages</param>
        /// 
        private static string GetLanguageToUse(IEnumerable<string> languages)
        {
            foreach (var language in languages)
            {
                var languageFound = ImplementedLanguages.Languages.FirstOrDefault(x => x.Contains(language));
                if (languageFound != null)
                    return language;
            }
            return GetDefaultLanguage();
        }

        /// <summary>
        /// Return a list containing the languages accepted by the user.
        /// </summary>
        /// <param name="userLanguages">HttpHeaderValueCollection containing StringWithQualityHeaderValue objects</param>
        /// <returns>List of languages</returns>
        /// 
        private static IEnumerable<string> GetLanguageList(ICollection<StringWithQualityHeaderValue> userLanguages)
        {
            var languageList = new List<string>();

            if (userLanguages == null || !userLanguages.Any()) return languageList;

            var languages = userLanguages.ToList();
            foreach (var language in languages)
            {
                var obj = language.ToString().Substring(0, 2);
                languageList.Add(obj);
            }
            return languageList;
        }
        #endregion Private Methods
    }
}