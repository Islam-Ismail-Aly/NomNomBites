using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Validators
{
    public class AuthenticatedUsersAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated &&
                (context.HttpContext.Request.Path.StartsWithSegments("/Account/Login") ||
                 context.HttpContext.Request.Path.StartsWithSegments("/Account/Register")))
            {
                context.Result = new RedirectResult("/Home/Index");
            }
            else if (!context.HttpContext.User.Identity.IsAuthenticated &&
                (context.HttpContext.Request.Path.StartsWithSegments("/Cart/Show") ||
                 context.HttpContext.Request.Path.StartsWithSegments("/Cart/Checkout")))
            {
                context.Result = new RedirectResult("/Account/Login");
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }
    }
}
