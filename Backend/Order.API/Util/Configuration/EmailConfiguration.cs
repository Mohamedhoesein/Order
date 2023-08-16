namespace Order.API.Util.Configuration
{
    /// <summary>
    /// The configurations needed to send an email.
    /// </summary>
    public class EmailConfiguration
    {
        /// <summary>
        /// The host of the smtp server.
        /// </summary>
        public string Host { get; set; } = "";
        /// <summary>
        /// The port of the smtp server.
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// If SSL is being used.
        /// </summary>
        public bool Ssl { get; set; }
        /// <summary>
        /// The display name to use.
        /// </summary>
        public string DisplayName { get; set; } = "";
        /// <summary>
        /// The email address from which to send the emails.
        /// </summary>
        public string Email { get; set; } = "";
        /// <summary>
        /// The password for the email which is used to send the emails.
        /// </summary>
        public string Password { get; set; } = "";
        /// <summary>
        /// The directory to drop the emails to if needed.
        /// </summary>
        public string DropEmailDirectory { get; set; } = "./emails";
        /// <summary>
        /// If the emails should be dropped on disk.
        /// </summary>
        public bool DropEmails { get; set; }
        /// <summary>
        /// If the emails should be send.
        /// </summary>
        public bool SendEmails { get; set; } = true;
    }
}