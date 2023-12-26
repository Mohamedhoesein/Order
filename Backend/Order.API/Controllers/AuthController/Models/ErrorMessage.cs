namespace Order.API.Controllers.AuthController.Models
{
    /// <summary>
    /// A model for a login error.
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        /// A string representing the error that occurred.
        /// </summary>
        public string Error { get; set; }
    }
}