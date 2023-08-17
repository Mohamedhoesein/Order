using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Order.Test.Controllers.AuthController
{
    /// <summary>
    /// Tests for the registering of end users and employees.
    /// </summary>
    [TestClass]
    public class RegisterTest : IDisposable
    {
        private readonly TestServer _testServer;
        private readonly CookieHttpClient _client;

        /// <summary>
        /// Initialize a new <see cref="LoginTest"/>.
        /// </summary>
        public RegisterTest()
        {
            _testServer = Util.CreateTestServer();
            _client = new CookieHttpClient(_testServer.CreateClient());
            Util.CreateDatabase();
        }

        /// <summary>
        /// Test if a new employee account can successfully be registered.
        /// </summary>
        [TestMethod]
        public async Task EmployeeRegister_Success()
        {
            const int index = 1;
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var (registerResult, verifyResult) = await _client.CreateTestEmployee(index, true);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);
            Assert.IsNotNull(verifyResult);
            Assert.AreEqual(HttpStatusCode.OK, verifyResult.StatusCode);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when registering a new employee without being logged in.
        /// </summary>
        [TestMethod]
        public async Task EmployeeRegister_Unauthorized()
        {
            const int index = 2;
            var (registerResult, _) = await _client.CreateTestEmployee(index, true);
            Assert.AreEqual(HttpStatusCode.Unauthorized, registerResult.StatusCode);
        }

        /// <summary>
        /// Test if a new end user account can successfully be registered.
        /// </summary>
        [TestMethod]
        public async Task EndUserRegister_Success()
        {
            const int index = 2;
            var (registerResult, _) = await _client.CreateTestEndUser(index, false);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);

            var (type, id, code) = Util.ParseLatestEmail("TempEndUser", $"test{index}@test.com");
            Assert.AreEqual("verify", type);
            var verifyResult = await _client.PostAsync($"auth/verify/{id}/{code}");
            Assert.AreEqual(HttpStatusCode.OK, verifyResult.StatusCode);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when verifying an account for which the id or code is invalid.
        /// </summary>
        /// <param name="index">
        /// The index of the user to use to make unique emails.
        /// </param>
        /// <param name="correctId">
        /// If the correct id should be used.
        /// </param>
        /// <param name="newId">
        /// The id to use if an invalid id needs to be used.
        /// </param>
        /// <param name="correctCode">
        /// If the correct code should be used.
        /// </param>
        /// <param name="newCode">
        /// The code to use if an invalid code needs to be used.
        /// </param>
        [DataRow(3, false, "-1", true, "code", DisplayName = "Verify_Unauthorized_InvalidId")]
        [DataRow(4, true, "0", false, "code", DisplayName = "Verify_Unauthorized_InvalidCode")]
        [DataTestMethod]
        public async Task Verify_Unauthorized(int index, bool correctId, string newId, bool correctCode, string newCode)
        {
            var (registerResult, _) = await _client.CreateTestEndUser(index, false);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);

            var (type, id, code) = Util.ParseLatestEmail("TempEndUser", $"test{index}@test.com");
            id = correctId ? id : newId;
            code = correctCode ? code : newCode;
            Assert.AreEqual("verify", type);
            var verifyResult = await _client.PostAsync($"auth/verify/{id}/{code}");
            Assert.AreEqual(HttpStatusCode.Unauthorized, verifyResult.StatusCode);
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