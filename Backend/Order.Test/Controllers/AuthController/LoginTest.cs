using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Order.Test.Controllers.AuthController
{
    /// <summary>
    /// Tests for the logging in of end users and employees.
    /// </summary>
    [TestClass]
    public class LoginTest : BaseTest
    {
        /// <summary>
        /// Test if an employee can successfully log in with the pre existing account and a new account.
        /// </summary>
        [TestMethod]
        public async Task EmployeeLogin_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var (registerResult, verifyResult) = await _client.CreateTestEmployee(true);
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
        [DataRow("test2@test.com", Util.DefaultPassword, false, DisplayName = "EmployeeLogin_Unauthorized_NotVerified")]
        [DataRow("test3@test.com", "TestPassword123!@", true, DisplayName = "EmployeeLogin_Unauthorized_InvalidPassword")]
        [DataRow("unkown@test.com", Util.DefaultPassword, true, DisplayName = "EmployeeLogin_Unauthorized_InvalidEmail")]
        [DataTestMethod]
        public async Task EmployeeLogin_Unauthorized(string email, string password, bool verify)
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var (registerResult, verifyResult) = await _client.CreateTestEmployee(verify);
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
            var email = "test1@test.com";
            var (registerResult, verifyResult) = await _client.CreateTestEndUser(true);
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
            var email = "test1@test.com";
            var (registerResult, verifyResult) = await _client.CreateTestEndUser(true);
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
        [DataRow("test7@test.com", Util.DefaultPassword, false, DisplayName = "EndUserLogin_Unauthorized_NotVerified")]
        [DataRow("test8@test.com", "TestPassword123!@", true, DisplayName = "EndUserLogin_Unauthorized_InvalidPassword")]
        [DataRow("unkown@test.com", Util.DefaultPassword, true, DisplayName = "EndUserLogin_Unauthorized_InvalidEmail")]
        [DataTestMethod]
        public async Task EndUserLogin_Unauthorized(string email, string password, bool verify)
        {
            var (registerResult, verifyResult) = await _client.CreateTestEndUser(verify);
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
    }
}