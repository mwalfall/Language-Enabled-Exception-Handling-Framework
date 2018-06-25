using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Site.Utility;

namespace Site.Attributes.Actions
{
    public class ApiLanguageSetterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            LanguageSetter.Set(actionContext);
        }
    }
}