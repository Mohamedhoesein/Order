using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Order.API.Middleware
{
    /// <summary>
    /// Middleware to place the anti forgery cookie in the header. 
    /// </summary>
    public class AntiForgeryMapperMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initialize a new instance of <see cref="AntiForgeryMapperMiddleware"/>.
        /// </summary>
        /// <param name="next">
        /// The next method to be called.
        /// </param>
        public AntiForgeryMapperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke the mapping of the anti forgery token.
        /// </summary>
        /// <param name="context">
        /// The http context of the current request.
        /// </param>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey(Constants.ANTI_FORGER_COOKIE))
            {
                var value = context.Request.Cookies[Constants.ANTI_FORGER_COOKIE];

                if (!string.IsNullOrWhiteSpace(value))
                    context.Request.Headers.Append(Constants.ANTI_FORGER_HEADER, value);
            }
            await _next(context);
        }
    }
}