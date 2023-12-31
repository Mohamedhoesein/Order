using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Order.Test.CookieHttpClient
{
    /// <summary>
    /// An <see cref="HttpClient"/> wrapper that handles cookies.
    /// Based on https://stackoverflow.com/a/61428920.
    /// </summary>
    public partial class CookieHttpClient
    {
        private readonly HttpClient _client;
        private readonly CookieContainer _container;

        /// <summary>
        /// Initialize a new <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="client">
        /// The client that is going to be wrapped.
        /// </param>
        public CookieHttpClient(HttpClient client)
        {
            _client = client;
            _container = new CookieContainer();
        }

        /// <summary>
        /// Deserialize the content of the message.
        /// </summary>
        /// <param name="message">
        /// The message to deserialize the content of.
        /// </param>
        /// <typeparam name="T">
        /// The type to deserialize to.
        /// </typeparam>
        /// <returns>
        /// The content of the message.
        /// </returns>
        public async Task<T?> Deserialize<T>(HttpResponseMessage message)
        {
            return JsonSerializer.CreateDefault().Deserialize<T>(
                new JsonTextReader(
                    new StreamReader(
                        await message.Content.ReadAsStreamAsync()
                    )
                )
            );
        }
    }
}