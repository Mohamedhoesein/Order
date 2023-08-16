namespace Order.API.Controllers.AuthController.Models
{
    /// <summary>
    /// If the account could be retrieved and the role it has.
    /// </summary>
    internal enum UserState
    {
        /// <summary>
        /// If the account is for an employee.
        /// </summary>
        Employee,
        /// <summary>
        /// If the account is for an end user.
        /// </summary>
        User,
        /// <summary>
        /// If the account could not be found.
        /// </summary>
        NotExist
    }
}