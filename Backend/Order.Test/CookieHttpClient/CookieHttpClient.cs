using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

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
        /// Do a get request to the specified path.
        /// </summary>
        /// <param name="path">
        /// The path to do a get request to.
        /// </param>
        /// <returns>
        /// The result of the get request.
        /// </returns>
        public async Task<HttpResponseMessage> GetAsync(string path)
        {
            return await SendAsync(HttpMethod.Get, path, null);
        }

        /// <summary>
        /// Do a post request to the specified path with the data.
        /// </summary>
        /// <param name="path">
        /// The path to do a post request to.
        /// </param>
        /// <param name="data">
        /// The data to send to the server.
        /// </param>
        /// <returns>
        /// The result of the post request.
        /// </returns>
        public async Task<HttpResponseMessage> PostAsync(string path, HttpContent? data = null)
        {
            return await SendAsync(HttpMethod.Post, path, data);
        }

        /// <summary>
        /// Do a delete request to the specified path with the data.
        /// </summary>
        /// <param name="path">
        /// The path to do a delete request to.
        /// </param>
        /// <param name="data">
        /// The data to send to the server.
        /// </param>
        /// <returns>
        /// The result of the delete request.
        /// </returns>
        public async Task<HttpResponseMessage> DeleteAsync(string path, HttpContent? data = null)
        {
            return await SendAsync(HttpMethod.Delete, path, data);
        }

        /// <summary>
        /// Do a request to the specified path with the data.
        /// </summary>
        /// <param name="method">
        /// The method to use for the request.
        /// </param>
        /// <param name="path">
        /// The path to do a request to.
        /// </param>
        /// <param name="data">
        /// The data to send to the server.
        /// </param>
        /// <returns>
        /// The result of the request.
        /// </returns>
        public async Task<HttpResponseMessage> SendAsync(HttpMethod method, string path, HttpContent? data)
        {
            var fullUri = new Uri(_client.BaseAddress ?? new Uri(""), path);
            var request = new HttpRequestMessage(method, fullUri);

            if (data != null)
                request.Content = data;

            var collection = _container.GetCookies(fullUri);
            if (collection.Count > 0)
                request.Headers.Add("Cookie", GetCookieStrings(collection));

            var response = await _client.SendAsync(request);

            if (!response.Headers.Contains("Set-Cookie"))
                return response;

            foreach (var s in response.Headers.GetValues("Set-Cookie"))
                _container.SetCookies(fullUri, s);

            return response;
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
            return JsonSerializer.Deserialize<T>(
                await message.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true,
                }
            );
        }

        /// <summary>
        /// Download a file from a path.
        /// </summary>
        /// <param name="path">
        /// The path of the file to download.
        /// </param>
        /// <returns>
        /// The bytes of the file.
        /// </returns>
        public Task<byte[]> Download(string path)
        {
            var fullUri = new Uri(_client.BaseAddress ?? new Uri(""), path);
            return _client.GetByteArrayAsync(fullUri);
        }
    }
}