namespace Order.API
{
    /// <summary>
    /// A class to contain static information.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// The name of the cookie that contains the jwt.
        /// </summary>
        public const string JWT_COOKIE = "JWT";
        /// <summary>
        /// The name of the header that contains the jwt.
        /// </summary>
        public const string JWT_HEADER = "Authorization";
        /// <summary>
        /// The name of the cookie that contains the anti forgery token.
        /// </summary>
        public const string ANTI_FORGER_COOKIE = "AntiForgery";
        /// <summary>
        /// The name of the header that contains the anti forgery token.
        /// </summary>
        public const string ANTI_FORGER_HEADER = "X-XSRF-TOKEN";
    }
}