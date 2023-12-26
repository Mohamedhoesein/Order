namespace Order.API.Controllers.AuthController.Models
{
    /// <summary>
    /// A model for if a user is logged in or not.
    /// </summary>
    public class IsLoggedIn
    {
        /// <summary>
        /// If the user is logged in or not.
        /// </summary>
        public bool LoggedIn { get; set; }
    }
}