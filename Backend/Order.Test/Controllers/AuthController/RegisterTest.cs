using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Order.Test.Controllers.AuthController
{
    /// <summary>
    /// Tests for the registering of end users and employees.
    /// </summary>
    [TestClass]
    public class RegisterTest : BaseTest
    {
        /// <summary>
        /// Test if a new employee account can successfully be registered.
        /// </summary>
        [TestMethod]
        public async Task EmployeeRegister_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var (registerResult, verifyResult) = await _client.CreateTestEmployee(true);
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
            var (registerResult, _) = await _client.CreateTestEmployee(true);
            Assert.AreEqual(HttpStatusCode.Unauthorized, registerResult.StatusCode);
        }

        /// <summary>
        /// Test if a new end user account can successfully be registered.
        /// </summary>
        [TestMethod]
        public async Task EndUserRegister_Success()
        {
            var (registerResult, _) = await _client.CreateTestEndUser(false);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);

            var (type, id, code) = Util.ParseLatestEmail("TempEndUser", "test1@test.com");
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
        [DataRow(false, "-1", true, "code", DisplayName = "Verify_Unauthorized_InvalidId")]
        [DataRow(true, "0", false, "code", DisplayName = "Verify_Unauthorized_InvalidCode")]
        [DataTestMethod]
        public async Task Verify_Unauthorized(bool correctId, string newId, bool correctCode, string newCode)
        {
            var (registerResult, _) = await _client.CreateTestEndUser(false);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);

            var (type, id, code) = Util.ParseLatestEmail("TempEndUser", "test1@test.com");
            id = correctId ? id : newId;
            code = correctCode ? code : newCode;
            Assert.AreEqual("verify", type);
            var verifyResult = await _client.PostAsync($"auth/verify/{id}/{code}");
            Assert.AreEqual(HttpStatusCode.Unauthorized, verifyResult.StatusCode);
        }
    }
}