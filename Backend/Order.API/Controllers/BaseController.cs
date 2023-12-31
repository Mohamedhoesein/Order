using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Return a <see cref="ObjectResult"/> with the given status code without a body.
        /// </summary>
        /// <param name="statusCode">
        /// The status code to return.
        /// </param>
        /// <returns>
        /// The <see cref="ObjectResult"/> with the given status code without a body.
        /// </returns>
        protected ObjectResult NewStatusCode(int statusCode)
        {
            return base.StatusCode(statusCode, "");
        }
    }
}