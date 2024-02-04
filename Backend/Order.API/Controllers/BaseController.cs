using Microsoft.AspNetCore.Mvc;
using Order.API.Context;

namespace Order.API.Controllers
{
    /// <summary>
    /// A base class for controllers.
    /// </summary>
    public class BaseController : Controller
    {
        protected readonly OrderContext _orderContext;

        /// <summary>
        /// Initialize a new <see cref="BaseController"/> with the required information.
        /// </summary>
        /// <param name="orderContext">
        /// The <see cref="OrderContext"/> used to handle database access.
        /// </param>
        public BaseController(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

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

        /// <summary>
        /// Save the database changes, and return an appropriate <see cref="IActionResult"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="OkResult"/> if the save was successful,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the save failed due to an exception.
        /// </returns>
        protected IActionResult Save()
        {
            try
            {
                _orderContext.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
                return NewStatusCode(500);
            }
        }
    }
}