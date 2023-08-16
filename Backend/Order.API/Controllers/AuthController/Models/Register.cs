﻿using System.ComponentModel.DataAnnotations;

namespace Order.API.Controllers.AuthController.Models
{
    /// <summary>
    /// The model used when registering an account.
    /// </summary>
    public class Register
    {
        /// <summary>
        /// The name of the user.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = "";
        /// <summary>
        /// The address for the user.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Address { get; set; } = "";
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
        /// <summary>
        /// A copy of <see cref="Password"/>, this is not used in the code. It is only used by the model validator.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "";
    }
}