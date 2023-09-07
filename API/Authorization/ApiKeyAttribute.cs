using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!IsApiKeyValid(context.HttpContext))
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private bool IsApiKeyValid(HttpContext context)
        {
            var apiKey = context.Request.Headers["X-ApiKey"];

            if (string.IsNullOrEmpty(apiKey))
            {
                return false;
            }

            var actualKey = context.RequestServices
                .GetRequiredService<IConfiguration>()
                .GetValue<string>("ApiKey");

            return actualKey == apiKey;
        }
    }
}
