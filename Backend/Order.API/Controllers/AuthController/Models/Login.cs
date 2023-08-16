using System.ComponentModel.DataAnnotations;

namespace Order.API.Controllers.AuthController.Models
{
    /// <summary>
    /// The model for logging in.
    /// </summary>
    public class Login
    {
        /// <summary>
        /// The email of the user.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public string Email { get; set; } = "";
        /// <summary>
        /// The password of the user.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}