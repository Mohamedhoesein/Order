using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Order.API.Controllers.AuthController.Models;

namespace Order.Test
{
    /// <summary>
    /// An <see cref="HttpClient"/> wrapper that handles cookies.
    /// Based on https://stackoverflow.com/a/61428920.
    /// </summary>
    public class CookieHttpClient : IDisposable
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

        /// <summary>
        /// Check if the current user is logged in by doing a request to the server.
        /// </summary>
        /// <returns>
        /// If the current user is logged in by doing a request to the server.
        /// </returns>
        public async Task<bool> LoggedIn()
        {
            var loggedIn = await GetAsync("auth/logged-in");
            return (await Deserialize<IsLoggedIn>(loggedIn))?.LoggedIn ?? false;
        }

        /// <summary>
        /// Get the account information of the currently logged in user.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the account retrieval.
        /// </returns>
        public async Task<HttpResponseMessage> GetAccount()
        {
            return await GetAsync("auth");
        }

        /// <summary>
        /// Update the account information of an end user.
        /// </summary>
        /// <param name="name">
        /// The new name for the end user.
        /// </param>
        /// <param name="address">
        /// The new address for the end user.
        /// </param>
        /// <param name="email">
        /// The new email for the end user.
        /// </param>
        /// <param name="password">
        /// The current password of the end user.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the account update.
        /// </returns>
        public async Task<HttpResponseMessage> UpdateAccountEndUser(string name, string address, string email, string password)
        {
            return await UpdateAccount(new UpdateAccountEndUser
            {
                Name = name,
                Address = address,
                Email = email,
                Password = password
            }, "user");
        }

        /// <summary>
        /// Update the account information of an employee.
        /// </summary>
        /// <param name="name">
        /// The new name for the employee.
        /// </param>
        /// <param name="address">
        /// The new address for the employee.
        /// </param>
        /// <param name="password">
        /// The current password of the employee.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the account update.
        /// </returns>
        public async Task<HttpResponseMessage> UpdateAccountEmployee(string name, string address, string password)
        {
            return await UpdateAccount(new UpdateAccountEmployee
            {
                Name = name,
                Address = address,
                Password = password
            }, "employee");
        }

        /// <summary>
        /// Update the account information of an user.
        /// </summary>
        /// <param name="content">
        /// The object to send to the endpoint.
        /// </param>
        /// <param name="path">
        /// The final part of the path to send the request to.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the account update.
        /// </returns>
        private async Task<HttpResponseMessage> UpdateAccount(object content, string path)
        {
            return await PostAsync(
                $"auth/{path}",
                JsonContent.Create(content)
            );
        }

        /// <summary>
        /// Request a password change email for the currently logged in end user.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the email send.
        /// </returns>
        public async Task<HttpResponseMessage> ForgotPasswordCurrentEndUser()
        {
            return await ForgotPasswordCurrent("user");
        }

        /// <summary>
        /// Request a password change email for the currently logged in employee.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the email send.
        /// </returns>
        public async Task<HttpResponseMessage> ForgotPasswordCurrentEmployee()
        {
            return await ForgotPasswordCurrent("employee");
        }

        /// <summary>
        /// Request a password change email for the currently logged in end user.
        /// </summary>
        /// <param name="path">
        /// The final part of the path to send the request to.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the email send.
        /// </returns>
        private async Task<HttpResponseMessage> ForgotPasswordCurrent(string path)
        {
            return await PostAsync(
                $"auth/forgot-password/current/{path}"
            );
        }

        /// <summary>
        /// Request a password change email for the end user associated with the email.
        /// </summary>
        /// <param name="email">
        /// The email to use for the lookup.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the email send.
        /// </returns>
        public async Task<HttpResponseMessage> ForgotPasswordEndUser(string email)
        {
            return await ForgotPassword(email, "user");
        }

        /// <summary>
        /// Request a password change email for the employee associated with the email.
        /// </summary>
        /// <param name="email">
        /// The email to use for the lookup.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the email send.
        /// </returns>
        public async Task<HttpResponseMessage> ForgotPasswordEmployee(string email)
        {
            return await ForgotPassword(email, "employee");
        }

        /// <summary>
        /// Request a password change email for the user associated with the email.
        /// </summary>
        /// <param name="email">
        /// The email to use for the lookup.
        /// </param>
        /// <param name="path">
        /// The final part of the path to send the request to.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the email send.
        /// </returns>
        private async Task<HttpResponseMessage> ForgotPassword(string email, string path)
        {
            return await PostAsync(
                $"auth/forgot-password/{path}",
                JsonContent.Create(
                    new ForgotPassword
                    {
                        Email = email
                    }
                )
            );
        }

        /// <summary>
        /// Change the password of a user.
        /// </summary>
        /// <param name="password">
        /// The new password.
        /// </param>
        /// <param name="confirmPassword">
        /// A copy of <see cref="password"/>.
        /// </param>
        /// <param name="id">
        /// The id of the user for which to change the password.
        /// </param>
        /// <param name="code">
        /// The password change code to use in changing.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the password change.
        /// </returns>
        public async Task<HttpResponseMessage> ChangePassword(string password, string confirmPassword, string id, string code)
        {
            return await PostAsync(
                $"auth/change-password/{id}/{code}",
                JsonContent.Create(
                    new UpdatePassword
                    {
                        Password = password,
                        ConfirmPassword = confirmPassword
                    }
                )
            );
        }

        /// <summary>
        /// Log an end user in.
        /// </summary>
        /// <param name="email">
        /// The email of the end user.
        /// </param>
        /// <param name="password">
        /// The password of the end user.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the login.
        /// </returns>
        public async Task<HttpResponseMessage> LoginEndUser(string email, string password)
        {
            return await Login(email, password, "user");
        }

        /// <summary>
        /// Log an employee in.
        /// </summary>
        /// <param name="email">
        /// The email of the employee.
        /// </param>
        /// <param name="password">
        /// The password of the employee.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the login.
        /// </returns>
        public async Task<HttpResponseMessage> LoginEmployee(string email, string password)
        {
            return await Login(email, password, "employee");
        }

        /// <summary>
        /// Log an user in.
        /// </summary>
        /// <param name="email">
        /// The email of user.
        /// </param>
        /// <param name="password">
        /// The password of the user.
        /// </param>
        /// <param name="path">
        /// The final part of the path to send the request to.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the login.
        /// </returns>
        private async Task<HttpResponseMessage> Login(string email, string password, string path)
        {
            return await PostAsync(
                $"auth/login/{path}",
                JsonContent.Create(
                    new Login
                    {
                        Email = email,
                        Password = password
                    }
                )
            );
        }

        /// <summary>
        /// Create a test employee.
        /// </summary>
        /// <param name="index">
        /// The index for the email (to create unique email addresses).
        /// </param>
        /// <param name="verify">
        /// If the employee should be verified
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the registration, and if it was needed that of the verification.
        /// </returns>
        public async Task<(HttpResponseMessage, HttpResponseMessage?)> CreateTestEmployee(int index, bool verify)
        {
            return await CreateTestAccount("employee", index, verify);
        }

        /// <summary>
        /// Create a test end user.
        /// </summary>
        /// <param name="index">
        /// The index for the email (to create unique email addresses).
        /// </param>
        /// <param name="verify">
        /// If the end user should be verified
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the registration, and if it was needed that of the verification.
        /// </returns>
        public async Task<(HttpResponseMessage, HttpResponseMessage?)> CreateTestEndUser(int index, bool verify)
        {
            return await CreateTestAccount("user", index, verify);
        }

        /// <summary>
        /// Create a test account.
        /// </summary>
        /// <param name="endpoint">
        /// The endpoint to use.
        /// </param>
        /// <param name="index">
        /// The index for the email (to create unique email addresses).
        /// </param>
        /// <param name="verify">
        /// If the end user should be verified
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the registration, and if it was needed that of the verification.
        /// </returns>
        private async Task<(HttpResponseMessage, HttpResponseMessage?)> CreateTestAccount(string endpoint, int index, bool verify)
        {
            var registerResult = await PostAsync(
                $"auth/register/{endpoint}",
                JsonContent.Create(
                    new Register
                    {
                        Name = "TempEndUser",
                        Email = $"test{index}@test.com",
                        Address = "Address",
                        Password = Util.DefaultPassword,
                        ConfirmPassword = Util.DefaultPassword,
                    }
                )
            );

            if (!verify || registerResult.StatusCode != HttpStatusCode.OK)
                return (registerResult, null);

            var (type, id, code) = Util.ParseLatestEmail("TempEndUser", $"test{index}@test.com");
            if (type != "verify")
                return (registerResult, null);

            var verifyResult = await PostAsync($"auth/verify/{id}/{code}");
            return (registerResult, verifyResult);
        }

        /// <summary>
        /// Delete an account.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the deletion.
        /// </returns>
        public async Task<HttpResponseMessage> DeleteAccount()
        {
            return await DeleteAsync("auth");
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
        /// Get the string representation of each of the cookies in the collection.
        /// </summary>
        /// <param name="collection">
        /// The <see cref="CookieCollection"/> containing all the cookies.
        /// </param>
        /// <returns>
        /// The string representation of each of the cookies in the collection.
        /// </returns>
        private IEnumerable<string> GetCookieStrings(CookieCollection collection)
        {
            var output = new List<string>(collection.Count);
            output.AddRange(
                collection.Where(cookie => !cookie.Expired)
                    .Select(cookie => cookie.Name + "=" + cookie.Value)
            );
            return output;
        }

        /// <summary>
        /// Dispose of the underlying <see cref="HttpClient"/>.
        /// </summary>
        public void Dispose()
        {
            _client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}