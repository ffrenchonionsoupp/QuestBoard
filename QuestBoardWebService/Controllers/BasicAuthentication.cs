using Microsoft.AspNetCore.Mvc.Filters;

namespace QuestBoardWebService.Controllers
{
    public class BasicAuthentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.IsNullOrEmpty(context.HttpContext.Request.Headers.Authorization))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                return;
            }

            var authHeader = context.HttpContext.Request.Headers.Authorization.ToString();
            var authHeaderParts = authHeader.Split(' ');

            // The value should be in the format "Basic base64encodedemailandpassword"
            if (authHeaderParts.Length != 2 || authHeaderParts[0] != "Basic")
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                return;
            }

            string credentials;
            try
            {
                credentials = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderParts[1]));
            }
            catch (FormatException)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                return;
            }

            // The credentials should be in the format "email:password" - both parts present.
            var parts = credentials.Split(':', 2);
            if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                return;
            }

            // Well-formed Basic Auth header received - allow the request to proceed.
            base.OnActionExecuting(context);
        }
    }
}
