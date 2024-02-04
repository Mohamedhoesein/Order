using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Order.Test.Controllers
{
    /// <summary>
    /// A base class for all tests.
    /// </summary>
    public class BaseTest : IDisposable
    {
        protected readonly TestServer _testServer;
        protected readonly CookieHttpClient.CookieHttpClient _client;

        /// <summary>
        /// Initialize a new <see cref="BaseTest"/>.
        /// </summary>
        public BaseTest()
        {
            _testServer = Util.CreateTestServer();
            _client = new CookieHttpClient.CookieHttpClient(_testServer.CreateClient());
        }

        /// <summary>
        /// Create the database before each test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            Util.CreateDatabase();
        }

        /// <summary>
        /// Log out, and delete the database and emails after each test.
        /// </summary>
        [TestCleanup]
        public async Task LogOut()
        {
            await _client.PostAsync("auth/logout");
            Util.DeleteDatabase();
            Util.DeleteEmails();
            Util.DeleteImages();
        }

        /// <summary>
        /// Dispose of the underlying <see cref="TestServer"/>, and <see cref="CookieHttpClient.CookieHttpClient"/>.
        /// Also delete the database, and delete the files associated with the emails.
        /// </summary>
        public void Dispose()
        {
            _testServer.Dispose();
            _client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}