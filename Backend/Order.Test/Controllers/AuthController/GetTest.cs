using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Order.API.Controllers.AuthController.Models;

namespace Order.Test.Controllers.AuthController
{
    /// <summary>
    /// Tests for receiving account information.
    /// </summary>
    [TestClass]
    public class GetTest : IDisposable
    {
        private readonly TestServer _testServer;
        private readonly CookieHttpClient _client;

        /// <summary>
        /// Initialize a new <see cref="GetTest"/>.
        /// </summary>
        public GetTest()
        {
            _testServer = Util.CreateTestServer();
            _client = new CookieHttpClient(_testServer.CreateClient());
            Util.CreateDatabase();
        }

        /// <summary>
        /// Test if the correct account information is returned.
        /// </summary>
        [TestMethod]
        public async Task Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var getAccount = await _client.GetAccount();
            Assert.AreEqual(HttpStatusCode.OK, getAccount.StatusCode);

            var account = await _client.Deserialize<Account>(getAccount);
            Assert.IsNotNull(account);
            Assert.AreEqual("TempAdmin", account.Name);
            Assert.AreEqual("Address", account.Address);
            Assert.AreEqual("test@test.com", account.Email);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when not logged in.
        /// </summary>
        [TestMethod]
        public async Task Unauthorized_NotLoggedIn()
        {
            var getAccount = await _client.GetAccount();
            Assert.AreEqual(HttpStatusCode.Unauthorized, getAccount.StatusCode);
        }

        /// <summary>
        /// Log out after every test.
        /// </summary>
        [TestCleanup]
        public async Task LogOut()
        {
            await _client.PostAsync("auth/logout");
        }

        /// <summary>
        /// Dispose of the underlying <see cref="TestServer"/>, and <see cref="CookieHttpClient"/>.
        /// Also delete the database, and delete the files associated with the emails.
        /// </summary>
        public void Dispose()
        {
            _testServer.Dispose();
            _client.Dispose();
            Util.DeleteDatabase();
            Util.DeleteEmails();
            GC.SuppressFinalize(this);
        }
    }
}