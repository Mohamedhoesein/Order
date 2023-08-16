namespace Order.API.Util
{
    /// <summary>
    /// A class to contain the different policy names.
    /// </summary>
    public static class Cors
    {
        /// <summary>
        /// The policy to allow any method, header, and credentials from both front-ends.
        /// </summary>
        public const string AllowFrontend = "_allowFrontend";
        /// <summary>
        /// The policy to allow any method, header, and credentials from the admin website.
        /// </summary>
        public const string AllowAdmin = "_allowAdmin";
        /// <summary>
        /// The policy to allow any method, header, and credentials from the end user website.
        /// </summary>
        public const string AllowEndUser = "_allowEndUser";
    }
}