namespace Order.API.Controllers.AuthController.Models
{
    /// <summary>
    /// A type of error when logging in.
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// The user is locked out.
        /// </summary>
        Locked,
        /// <summary>
        /// Two factor authentication is needed
        /// </summary>
        TwoFactor,
        /// <summary>
        /// The credentials are invalid.
        /// </summary>
        Invalid
    }
}