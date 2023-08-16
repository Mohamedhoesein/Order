using System.ComponentModel.DataAnnotations;

namespace Order.API.Controllers.AuthController.Models
{
    /// <summary>
    /// The model for updating the information of an employee.
    /// </summary>
    public class UpdateAccountEmployee
    {
        /// <summary>
        /// The name of the employee
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = "";
        /// <summary>
        /// The address of the employee.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Address { get; set; } = "";
        /// <summary>
        /// The password of the employee.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}