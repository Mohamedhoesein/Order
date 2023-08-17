using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Order.API.Controllers.AuthController.Models;

namespace Order.Test.Controllers.AuthController
{
    /// <summary>
    /// Tests for the logging in of end users and employees.
    /// </summary>
    [TestClass]
    public class LoginTest : IDisposable
    {
        private readonly TestServer _testServer;
        private readonly CookieHttpClient _client;

        /// <summary>
        /// Initialize a new <see cref="LoginTest"/>.
        /// </summary>
        public LoginTest()
        {
            _testServer = Util.CreateTestServer();
            _client = new CookieHttpClient(_testServer.CreateClient());
            Util.CreateDatabase();
        }

        /// <summary>
        /// Test if an employee can successfully log in with the pre existing account and a new account.
        /// </summary>
        [TestMethod]
        public async Task EmployeeLogin_Success()
        {
            const int index = 1;
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var (registerResult, verifyResult) = await _client.CreateTestEmployee(index, true);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);
            Assert.IsNotNull(verifyResult);
            Assert.AreEqual(HttpStatusCode.OK, verifyResult.StatusCode);
            
            await _client.PostAsync("auth/logout");

            var login2Result = await _client.LoginEmployee("test1@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, login2Result.StatusCode);
            Assert.IsTrue(await _client.LoggedIn());
        }

        /// <summary>
        /// Test if an unauthorized response is returned when logging is as an employee,
        /// where either the account is not verified, the email is invalid, or the password is invalid.
        /// </summary>
        /// <param name="index">
        /// The index for the email (to create unique email addresses).
        /// </param>
        /// <param name="email">
        /// The email to use for logging in.
        /// </param>
        /// <param name="password">
        /// The password to use for logging in.
        /// </param>
        /// <param name="verify">
        /// If the account should be verified.
        /// </param>
        [DataRow(2, "test2@test.com", Util.DefaultPassword, false, DisplayName = "EmployeeLogin_Unauthorized_NotVerified")]
        [DataRow(3, "test3@test.com", "TestPassword123!@", true, DisplayName = "EmployeeLogin_Unauthorized_InvalidPassword")]
        [DataRow(4, "unkown@test.com", Util.DefaultPassword, true, DisplayName = "EmployeeLogin_Unauthorized_InvalidEmail")]
        [DataTestMethod]
        public async Task EmployeeLogin_Unauthorized(int index, string email, string password, bool verify)
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var (registerResult, verifyResult) = await _client.CreateTestEmployee(index, verify);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);
            if (verify)
            {
                Assert.IsNotNull(verifyResult);
                Assert.AreEqual(HttpStatusCode.OK, verifyResult.StatusCode);
            }

            var logOutResult = await _client.PostAsync("auth/logout");
            Assert.AreEqual(HttpStatusCode.OK, logOutResult.StatusCode);

            var logIn2Result = await _client.LoginEmployee(email, password);
            Assert.AreEqual(HttpStatusCode.Unauthorized, logIn2Result.StatusCode);
            Assert.IsFalse(await _client.LoggedIn());
        }

        /// <summary>
        /// Test if an unauthorized response is returned when logging in as an end user on the employee endpoint.
        /// </summary>
        [TestMethod]
        public async Task EmployeeLogin_Unauthorized_User()
        {
            const int index = 5;
            var email = $"test{index}@test.com";
            var (registerResult, verifyResult) = await _client.CreateTestEndUser(index, true);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);
            Assert.IsNotNull(verifyResult);
            Assert.AreEqual(HttpStatusCode.OK, verifyResult.StatusCode);

            var loginResult = await _client.LoginEmployee(email, Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.Unauthorized, loginResult.StatusCode);
            Assert.IsFalse(await _client.LoggedIn());
        }

        /// <summary>
        /// Test if an end user can log in on a new account.
        /// </summary>
        [TestMethod]
        public async Task EndUserLogin_Success()
        {
            const int index = 6;
            var email = $"test{index}@test.com";
            var (registerResult, verifyResult) = await _client.CreateTestEndUser(index, true);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);
            Assert.IsNotNull(verifyResult);
            Assert.AreEqual(HttpStatusCode.OK, verifyResult.StatusCode);

            var loginResult = await _client.LoginEndUser(email, Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);
            Assert.IsTrue(await _client.LoggedIn());
        }

        /// <summary>
        /// Test if an unauthorized response is returned when logging is as an end user,
        /// where either the account is not verified, the email is invalid, or the password is invalid.
        /// </summary>
        /// <param name="index">
        /// The index for the email (to create unique email addresses).
        /// </param>
        /// <param name="email">
        /// The email to use for logging in.
        /// </param>
        /// <param name="password">
        /// The password to use for logging in.
        /// </param>
        /// <param name="verify">
        /// If the account should be verified.
        /// </param>
        [DataRow(7, "test7@test.com", Util.DefaultPassword, false, DisplayName = "EndUserLogin_Unauthorized_NotVerified")]
        [DataRow(8, "test8@test.com", "TestPassword123!@", true, DisplayName = "EndUserLogin_Unauthorized_InvalidPassword")]
        [DataRow(9, "unkown@test.com", Util.DefaultPassword, true, DisplayName = "EndUserLogin_Unauthorized_InvalidEmail")]
        [DataTestMethod]
        public async Task EndUserLogin_Unauthorized(int index, string email, string password, bool verify)
        {
            var (registerResult, verifyResult) = await _client.CreateTestEndUser(index, verify);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);
            if (verify)
            {
                Assert.IsNotNull(verifyResult);
                Assert.AreEqual(HttpStatusCode.OK, verifyResult.StatusCode);
            }

            var loginResult = await _client.LoginEndUser(email, password);
            Assert.AreEqual(HttpStatusCode.Unauthorized, loginResult.StatusCode);
            Assert.IsFalse(await _client.LoggedIn());
        }

        /// <summary>
        /// Test if an unauthorized response is returned when logging in as an employee on the end user endpoint.
        /// </summary>
        [TestMethod]
        public async Task EndUserLogin_Unauthorized_Employee()
        {
            var loginResult = await _client.LoginEndUser("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.Unauthorized, loginResult.StatusCode);
            Assert.IsFalse(await _client.LoggedIn());
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