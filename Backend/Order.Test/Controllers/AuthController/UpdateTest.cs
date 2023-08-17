using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Order.API.Controllers.AuthController.Models;

namespace Order.Test.Controllers.AuthController
{
    /// <summary>
    /// Tests for account updating.
    /// </summary>
    [TestClass]
    public class UpdateTest : IDisposable
    {
        private readonly TestServer _testServer;
        private readonly CookieHttpClient _client;

        /// <summary>
        /// Initialize a new <see cref="LoginTest"/>.
        /// </summary>
        public UpdateTest()
        {
            _testServer = Util.CreateTestServer();
            _client = new CookieHttpClient(_testServer.CreateClient());
            Util.CreateDatabase();
        }

        /// <summary>
        /// Test if the account information of an employee is updated.
        /// </summary>
        [TestMethod]
        public async Task UpdateAccountEmployee_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var updateResult = await _client.UpdateAccountEmployee("NewName", "NewAddress", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, updateResult.StatusCode);

            var getResult = await _client.GetAccount();
            Assert.AreEqual(HttpStatusCode.OK, getResult.StatusCode);

            var account = await _client.Deserialize<Account>(getResult);
            Assert.IsNotNull(account);
            Assert.AreEqual("NewName", account.Name);
            Assert.AreEqual("NewAddress", account.Address);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when using the incorrect password as an employee.
        /// </summary>
        [TestMethod]
        public async Task UpdateAccountEmployee_Unauthorized_InvalidPassword()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var updateResult = await _client.UpdateAccountEmployee("NewName", "NewAddress", "TestPassword321!@#");
            Assert.AreEqual(HttpStatusCode.Unauthorized, updateResult.StatusCode);

            var getResult = await _client.GetAccount();
            Assert.AreEqual(HttpStatusCode.OK, getResult.StatusCode);

            var account = await _client.Deserialize<Account>(getResult);
            Assert.IsNotNull(account);
            Assert.AreEqual("TempAdmin", account.Name);
            Assert.AreEqual("Address", account.Address);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when trying to update an account while not logged in as an employee.
        /// </summary>
        [TestMethod]
        public async Task UpdateAccountEmployee_Unauthorized_NotLoggedIn()
        {
            var updateResult = await _client.UpdateAccountEmployee("NewName", "NewAddress", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.Unauthorized, updateResult.StatusCode);

            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var getResult = await _client.GetAccount();
            Assert.AreEqual(HttpStatusCode.OK, getResult.StatusCode);

            var account = await _client.Deserialize<Account>(getResult);
            Assert.IsNotNull(account);
            Assert.AreEqual("TempAdmin", account.Name);
            Assert.AreEqual("Address", account.Address);
        }

        /// <summary>
        /// Test if an forbidden response is returned when trying to update an account via the end user endpoint as an employee.
        /// </summary>
        [TestMethod]
        public async Task UpdateAccountEmployee_Forbidden_EndUserEndpoint()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var updateResult = await _client.UpdateAccountEndUser("NewName", "NewAddress", "test98@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.Forbidden, updateResult.StatusCode);

            var getResult = await _client.GetAccount();
            Assert.AreEqual(HttpStatusCode.OK, getResult.StatusCode);

            var account = await _client.Deserialize<Account>(getResult);
            Assert.IsNotNull(account);
            Assert.AreEqual("TempAdmin", account.Name);
            Assert.AreEqual("Address", account.Address);
        }

        /// <summary>
        /// Test if the account information of an end user is updated.
        /// </summary>
        [TestMethod]
        public async Task UpdateAccountEndUser_Success()
        {
            const int index = 1;
            var email = $"test{index}@test.com";

            var (registerResult, verify1Result) = await _client.CreateTestEndUser(index, true);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);
            Assert.IsNotNull(verify1Result);
            Assert.AreEqual(HttpStatusCode.OK, verify1Result.StatusCode);

            var loginResult = await _client.LoginEndUser(email, Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var updateResult = await _client.UpdateAccountEndUser("NewName", "NewAddress", "test99@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, updateResult.StatusCode);

            var (type, id, code) = Util.ParseLatestEmail("NewName", "test99@test.com");
            Assert.AreEqual("email", type);

            var verify2Result = await _client.PostAsync($"auth/email/{id}/{code}");
            Assert.AreEqual(HttpStatusCode.OK, verify2Result.StatusCode);

            var getResult = await _client.GetAccount();
            Assert.AreEqual(HttpStatusCode.Unauthorized, getResult.StatusCode);

            var login2Result = await _client.LoginEndUser("test99@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, login2Result.StatusCode);

            var get1Result = await _client.GetAccount();
            Assert.AreEqual(HttpStatusCode.OK, get1Result.StatusCode);

            var account = await _client.Deserialize<Account>(get1Result);
            Assert.IsNotNull(account);
            Assert.AreEqual("NewName", account.Name);
            Assert.AreEqual("NewAddress", account.Address);
            Assert.AreEqual("test99@test.com", account.Email);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when using the incorrect password as an end user.
        /// </summary>
        [TestMethod]
        public async Task UpdateAccountEndUser_Unauthorized_InvalidPassword()
        {
            const int index = 2;
            var email = $"test{index}@test.com";

            var (registerResult, verifyResult) = await _client.CreateTestEndUser(index, true);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);
            Assert.IsNotNull(verifyResult);
            Assert.AreEqual(HttpStatusCode.OK, verifyResult.StatusCode);

            var loginResult = await _client.LoginEndUser(email, Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var updateResult = await _client.UpdateAccountEndUser("NewName", "NewAddress", "test100@test.com", "TestPassword321!@#");
            Assert.AreEqual(HttpStatusCode.Unauthorized, updateResult.StatusCode);

            var getResult = await _client.GetAccount();
            Assert.AreEqual(HttpStatusCode.OK, getResult.StatusCode);

            var account = await _client.Deserialize<Account>(getResult);
            Assert.IsNotNull(account);
            Assert.AreEqual("TempEndUser", account.Name);
            Assert.AreEqual("Address", account.Address);
            Assert.AreEqual("test2@test.com", account.Email);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when trying to update an account while not logged in as an end user.
        /// </summary>
        [TestMethod]
        public async Task UpdateAccountEndUser_Unauthorized_NotLoggedIn()
        {
            const int index = 2;
            var email = $"test{index}@test.com";

            var (registerResult, verifyResult) = await _client.CreateTestEndUser(index, true);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);
            Assert.IsNotNull(verifyResult);
            Assert.AreEqual(HttpStatusCode.OK, verifyResult.StatusCode);

            var updateResult = await _client.UpdateAccountEndUser("NewName", "NewAddress", "test101@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.Unauthorized, updateResult.StatusCode);

            var loginResult = await _client.LoginEndUser(email, Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var getResult = await _client.GetAccount();
            Assert.AreEqual(HttpStatusCode.OK, getResult.StatusCode);

            var account = await _client.Deserialize<Account>(getResult);
            Assert.IsNotNull(account);
            Assert.AreEqual("TempEndUser", account.Name);
            Assert.AreEqual("Address", account.Address);
            Assert.AreEqual("test2@test.com", account.Email);
        }

        /// <summary>
        /// Test if an forbidden response is returned when trying to update an account via the employee endpoint as an end user.
        /// </summary>
        [TestMethod]
        public async Task UpdateAccountEndUser_Forbidden_EmployeeEndpoint()
        {
            const int index = 3;
            var email = $"test{index}@test.com";

            var (registerResult, verifyResult) = await _client.CreateTestEndUser(index, true);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);
            Assert.IsNotNull(verifyResult);
            Assert.AreEqual(HttpStatusCode.OK, verifyResult.StatusCode);

            var loginResult = await _client.LoginEndUser(email, Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var updateResult = await _client.UpdateAccountEmployee("NewName", "NewAddress", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.Forbidden, updateResult.StatusCode);

            var login2Result = await _client.LoginEndUser(email, Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, login2Result.StatusCode);

            var getResult = await _client.GetAccount();
            Assert.AreEqual(HttpStatusCode.OK, getResult.StatusCode);

            var account = await _client.Deserialize<Account>(getResult);
            Assert.IsNotNull(account);
            Assert.AreEqual("TempEndUser", account.Name);
            Assert.AreEqual("Address", account.Address);
            Assert.AreEqual("test3@test.com", account.Email);
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