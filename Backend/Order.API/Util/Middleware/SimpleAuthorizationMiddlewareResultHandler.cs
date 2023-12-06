using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using NuGet.Protocol;

namespace Order.API.Util.Middleware
{
    /// <summary>
    /// A result handler for the <see cref="AuthorizeAttribute"/> to just return the status code when the authorization fails.
    /// </summary>
    public class SimpleAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly IAuthorizationMiddlewareResultHandler _handler = new AuthorizationMiddlewareResultHandler();

        /// <summary>
        /// Return just a 403 when the <see cref="AuthorizeAttribute"/> fails.
        /// </summary>
        /// <param name="next">
        /// The next middleware in the application pipeline.
        /// </param>
        /// <param name="context">
        /// The <see cref="HttpContext"/> to use.
        /// </param>
        /// <param name="policy">
        /// The <see cref="AuthorizationPolicy"/> to use.
        /// </param>
        /// <param name="authorizeResult">
        /// The result of the authorization.
        /// </param>
        public Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult
        )
        {
            if (authorizeResult.Forbidden)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Task.CompletedTask;
            }

            if (authorizeResult.Challenged)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Task.CompletedTask;
            }

            _handler.HandleAsync(next, context, policy, authorizeResult).Wait();
            return Task.CompletedTask;
        }
    }
}