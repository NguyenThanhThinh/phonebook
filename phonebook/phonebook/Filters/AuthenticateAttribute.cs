using System.Web;
using System.Web.Mvc;

namespace phonebook.Filters
{
    using phonebook.Services;
    public class AuthenticateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!AuthenticationService.IsLogged)
            {
                filterContext.Result = new RedirectResult("~/Account/Login?RedirectUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.Url.ToString()));
            }
        }
    }
}