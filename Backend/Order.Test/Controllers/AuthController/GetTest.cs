using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Order.API.Controllers.AuthController.Models;

namespace Order.Test.Controllers.AuthController
{
    /// <summary>
    /// Tests for receiving account information.
    /// </summary>
    [TestClass]
    public class GetTest : BaseTest
    {
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
    }
}