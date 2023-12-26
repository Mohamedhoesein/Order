using System.Globalization;
using MailKit.Net.Smtp;
using MimeKit;
using Order.API.Util.Configuration;

namespace Order.API.Util
{
    /// <summary>
    /// A wrapper class for email sending.
    /// </summary>
    public class MailSender
    {
        private readonly SmtpClient _client;
        private readonly MailboxAddress _from;
        private readonly EmailConfiguration _configuration;

        /// <summary>
        /// Initialize a new <see cref="MailSender"/>.
        /// </summary>
        /// <param name="client">
        /// The <see cref="SmtpClient"/> used to send the message.
        /// </param>
        /// <param name="from">
        /// The from address used to send the message.
        /// </param>
        /// <param name="configuration">
        /// The email configurations used to determine if the email is actually send and if it is drooped to disk.
        /// </param>
        public MailSender(SmtpClient client, MailboxAddress from, EmailConfiguration configuration)
        {
            _client = client;
            _from = from;
            _configuration = configuration;
        }

        /// <summary>
        /// Send an email to an user.
        /// Based on the email configuration, the email is actually send and or dumped to disk.
        /// </summary>
        /// <param name="name">
        /// The name of the receiver.
        /// </param>
        /// <param name="email">
        /// The email of the receiver.
        /// </param>
        /// <param name="messageText">
        /// The email body.
        /// </param>
        /// <returns>
        /// If the message was send successfully.
        /// </returns>
        public async Task<bool> Send(string name, string email, string messageText)
        {
            var message = new MimeMessage();
            message.From.Add(_from);
            message.To.Add(new MailboxAddress(name, email));
            message.Body = new TextPart("plain")
            {
                Text = messageText
            };

            if (_configuration.DropEmails)
            {
                if (!await DumpToDisk(name, email, message))
                    return false;
            }

            if (!_configuration.SendEmails)
                return true;

            try
            {
                await _client.SendAsync(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Dump a message to disk.
        /// </summary>
        /// <param name="name">
        /// The name of the receiver.
        /// </param>
        /// <param name="email">
        /// The email of the receiver.
        /// </param>
        /// <param name="message">
        /// The email body.
        /// </param>
        /// <returns>
        /// If the message was dumped successfully.
        /// </returns>
        private async Task<bool> DumpToDisk(string name, string email, MimeMessage message)
        {
            var (valid, stream) = CreateStream(name, email);
            if (!valid)
                return false;
            try
            {
                await using (stream)
                {
                    var options = FormatOptions.Default.Clone();
                    options.NewLineFormat = NewLineFormat.Dos;

                    await message.WriteToAsync(options, stream);
                    await stream.FlushAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Create a file stream for the specified receiver.
        /// </summary>
        /// <param name="name">
        /// The name of the receiver.
        /// </param>
        /// <param name="email">
        /// The email of the receiver.
        /// </param>
        /// <returns>
        /// If the file stream was created successfully, and the stream it self.
        /// If the stream was not created successfully it is an empty <see cref="MemoryStream"/>.
        /// </returns>
        private (bool, Stream) CreateStream(string name, string email)
        {
            try
            {
                var suffixNumber = 0;
                while (true)
                {
                    var path = CreateEmailPath(name, email, suffixNumber);
                    if (!File.Exists(path))
                        return (true, File.Open(path, FileMode.Create));

                    if (suffixNumber == int.MaxValue)
                        return (false, new MemoryStream());
                    suffixNumber++;
                }
            }
            catch
            {
                return (false, new MemoryStream());
            }
        }

        /// <summary>
        /// Get the path for a specific user.
        /// </summary>
        /// <param name="name">
        /// The name of the receiver.
        /// </param>
        /// <param name="email">
        /// The email of the receiver.
        /// </param>
        /// <param name="suffixNumber">
        /// A suffix number used when the file already exists.
        /// If it is 0 it is not used.
        /// </param>
        /// <returns>
        /// The path for a specific user.
        /// </returns>
        private string CreateEmailPath(string name, string email, int suffixNumber)
        {
            var subfolder = $"{name}.{email.Replace("@", "_at_")}";
            var folder = Path.Combine(_configuration.DropEmailDirectory, subfolder);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            var filename = CreateFileName(suffixNumber);
            var path = Path.Combine(folder, filename);
            return path;
        }

        /// <summary>
        /// Create a file name for the email.
        /// </summary>
        /// <param name="suffixNumber">
        /// A suffix number used when the file already exists.
        /// If it is 0 it is not used.
        /// </param>
        /// <returns>
        /// The file name.
        /// </returns>
        private string CreateFileName(int suffixNumber)
        {
            var dateString = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
                .Replace("/", ".")
                .Replace("\\", ".")
                .Replace(" ", ".")
                .Replace(":", ".");
            var suffix = suffixNumber == 0 ? string.Empty : $".{suffixNumber.ToString()}";
            return $"{Environment.ProcessId}.{dateString}{suffix}.eml";
        }
    }
}