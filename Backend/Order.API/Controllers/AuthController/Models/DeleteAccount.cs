namespace Order.API.Controllers.AuthController.Models
{
    /// <summary>
    /// The password for account deletion.
    /// </summary>
    public class DeleteAccount
    {
        /// <summary>
        /// The password of the user to be deleted.
        /// </summary>
        public string Password { get; set; }
    }
}