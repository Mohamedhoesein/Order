using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Order.API.Middleware
{
    /// <summary>
    /// Middleware to place the jwt value in a header.
    /// </summary>
    public class JwtMapperMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initialize a new instance of <see cref="JwtMapperMiddleware"/>.
        /// </summary>
        /// <param name="next">
        /// The next method to be called.
        /// </param>
        public JwtMapperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke the mapping of the jwt.
        /// </summary>
        /// <param name="context">
        /// The http context of the current request.
        /// </param>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey(Constants.JWT_COOKIE))
            {
                var value = context.Request.Cookies[Constants.JWT_COOKIE];

                if (!string.IsNullOrWhiteSpace(value))
                    context.Request.Headers.Append(Constants.JWT_HEADER, $"bearer {value}");
            }
            await _next(context);
        }
    }
}