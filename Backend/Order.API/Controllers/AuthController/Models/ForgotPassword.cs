using System.ComponentModel.DataAnnotations;

namespace Order.API.Controllers.AuthController.Models
{
    /// <summary>
    /// The model used for when someone forgot their password.
    /// </summary>
    public class ForgotPassword
    {
        /// <summary>
        /// The email of the account to reset the password for.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}