using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Order.Test.Controllers.AuthController
{
    /// <summary>
    /// Tests for deleting an account.
    /// </summary>
    [TestClass]
    public class DeleteTest : BaseTest
    {
        /// <summary>
        /// Test if an account is deleted.
        /// </summary>
        [TestMethod]
        public async Task Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var deleteResult = await _client.DeleteAccount(Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, deleteResult.StatusCode);

            var login2Result = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.Unauthorized, login2Result.StatusCode);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when using a wrong password.
        /// </summary>
        [TestMethod]
        public async Task Unauthorized_WrongPassword()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var deleteResult = await _client.DeleteAccount(Util.NewPassword);
            Assert.AreEqual(HttpStatusCode.Unauthorized, deleteResult.StatusCode);

            var logoutResult = await _client.PostAsync("auth/logout");
            Assert.AreEqual(HttpStatusCode.OK, logoutResult.StatusCode);

            var login2Result = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, login2Result.StatusCode);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when not logged in.
        /// </summary>
        [TestMethod]
        public async Task Unauthorized_NotLoggedIn()
        {
            var deleteResult = await _client.DeleteAccount(Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.Unauthorized, deleteResult.StatusCode);

            var login2Result = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, login2Result.StatusCode);
        }
    }
}