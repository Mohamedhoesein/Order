namespace Order.API
{
    /// <summary>
    /// A class to contain the different policy names.
    /// </summary>
    public static class Cors
    {
        /// <summary>
        /// The policy to allow any method header, and credentials from the front-end.
        /// </summary>
        public const string ALLOW_FRONTEND = "_allowFrontend";
    }
}