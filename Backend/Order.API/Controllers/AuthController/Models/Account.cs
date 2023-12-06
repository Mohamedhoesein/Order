namespace Order.API.Controllers.AuthController.Models
{
    /// <summary>
    /// The account information of a user.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// The name of the user.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The address of the user.
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }
    }
}