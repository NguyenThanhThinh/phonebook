using System.Web.Mvc;

namespace phonebook.Filters
{
    using phonebook.Services;
    public class AuthorizeAttribute : AuthenticateAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.Result != null)
            {
                return;
            }

            if (!AuthenticationService.LoggedUser.IsAdmin)
            {
                filterContext.Result = new RedirectResult("~");
            }
        }
    }
}