using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASPChat.Middleware
{
    public enum AuthRole { User, Admin}

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ChatAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private AuthRole _role;
        public ChatAuthorizeAttribute() { }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(context.HttpContext?.Session.GetInt32("userId") == null)
            {
                context.Result = new RedirectResult("/Login");
                return;
            }
        }
    }
}
