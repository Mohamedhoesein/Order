using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Order.Test.Controllers.AuthController
{
    /// <summary>
    /// Tests for resetting a password.
    /// </summary>
    [TestClass]
    public class ForgotPasswordTest : IDisposable
    {
        private readonly TestServer _testServer;
        private readonly CookieHttpClient _client;

        /// <summary>
        /// Initialize a new <see cref="ForgotPasswordTest"/>.
        /// </summary>
        public ForgotPasswordTest()
        {
            _testServer = Util.CreateTestServer();
            _client = new CookieHttpClient(_testServer.CreateClient());
            Util.CreateDatabase();
        }

        /// <summary>
        /// Test if the password is correctly reset when specifying the email of an employee.
        /// </summary>
        [TestMethod]
        public async Task ForgotPasswordEmployee_Success()
        {
            var forgotResult =  await _client.ForgotPasswordEmployee("test@test.com");
            Assert.AreEqual(HttpStatusCode.OK, forgotResult.StatusCode);

            var (type, id, code) = Util.ParseLatestEmail("TempAdmin", "test@test.com");
            Assert.AreEqual("change-password", type);

            var changeResult = await _client.ChangePassword(Util.NewPassword, Util.NewPassword, id, code);
            Assert.AreEqual(HttpStatusCode.OK, changeResult.StatusCode);

            var loginResult = await _client.LoginEmployee("test@test.com", Util.NewPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);
        }

        /// <summary>
        /// Test if no email is send if it is not known for an employee.
        /// </summary>
        [TestMethod]
        public async Task ForgotPasswordEmployee_NoNewEmail()
        {
            var exists = new DirectoryInfo("./emails").GetDirectories()
                .Any(info => info.Name.Contains("test1@test.com"));
            Assert.IsFalse(exists);

            var forgotResult =  await _client.ForgotPasswordEmployee("test1@test.com");
            Assert.AreEqual(HttpStatusCode.OK, forgotResult.StatusCode);

            var exists1 = new DirectoryInfo("./emails").GetDirectories()
                .Any(info => info.Name.Contains("test1@test.com"));
            Assert.IsFalse(exists1);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when changing the password of an employee via the email
        /// for which the id or code is invalid.
        /// </summary>
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
        [DataRow(false, "-1", true, "code", DisplayName = "ForgotPasswordEmployee_Unauthorized_InvalidId")]
        [DataRow(true, "-1", false, "code", DisplayName = "ForgotPasswordEmployee_Unauthorized_InvalidCode")]
        [DataTestMethod]
        public async Task ForgotPasswordEmployee_Unauthorized(bool correctId, string newId, bool correctCode, string newCode)
        {
            var forgotResult =  await _client.ForgotPasswordEmployee("test@test.com");
            Assert.AreEqual(HttpStatusCode.OK, forgotResult.StatusCode);

            var (type, id, code) = Util.ParseLatestEmail("TempAdmin", "test@test.com");
            id = correctId ? id : newId;
            code = correctCode ? code : newCode;
            Assert.AreEqual("change-password", type);
            var verifyResult = await _client.ChangePassword(Util.NewPassword, Util.NewPassword, id, code);
            Assert.AreEqual(HttpStatusCode.Unauthorized, verifyResult.StatusCode);
        }

        /// <summary>
        /// Test if the password is correctly reset for the currently logged in employee.
        /// </summary>
        [TestMethod]
        public async Task ForgotPasswordCurrentEmployee_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var forgotResult =  await _client.ForgotPasswordCurrentEmployee();
            Assert.AreEqual(HttpStatusCode.OK, forgotResult.StatusCode);

            var (type, id, code) = Util.ParseLatestEmail("TempAdmin", "test@test.com");
            Assert.AreEqual("change-password", type);

            var changeResult = await _client.ChangePassword(Util.NewPassword, Util.NewPassword, id, code);
            Assert.AreEqual(HttpStatusCode.OK, changeResult.StatusCode);

            var loggedIn = await _client.LoggedIn();
            Assert.IsFalse(loggedIn);

            var login2Result = await _client.LoginEmployee("test@test.com", Util.NewPassword);
            Assert.AreEqual(HttpStatusCode.OK, login2Result.StatusCode);
        }

        /// <summary>
        /// Test if no email is send for the currently logged in employee if an employee is not logged in.
        /// </summary>
        [TestMethod]
        public async Task ForgotPasswordCurrentEmployee_NoNewEmail()
        {
            var exists = new DirectoryInfo("./emails").GetDirectories()
                .Any(info => info.Name.Contains("test1@test.com"));
            Assert.IsFalse(exists);

            var forgotResult =  await _client.ForgotPasswordCurrentEmployee();
            Assert.AreEqual(HttpStatusCode.Unauthorized, forgotResult.StatusCode);

            var exists1 = new DirectoryInfo("./emails").GetDirectories()
                .Any(info => info.Name.Contains("test1@test.com"));
            Assert.IsFalse(exists1);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when changing the password for the currently logged in employee
        /// for which the id or code is invalid.
        /// </summary>
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
        [DataRow(false, "-1", true, "code", DisplayName = "ForgotPasswordCurrentEmployee_Unauthorized_InvalidId")]
        [DataRow(true, "-1", false, "code", DisplayName = "ForgotPasswordCurrentEmployee_Unauthorized_InvalidCode")]
        [DataTestMethod]
        public async Task ForgotPasswordCurrentEmployee_Unauthorized(bool correctId, string newId, bool correctCode, string newCode)
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var forgotResult =  await _client.ForgotPasswordCurrentEmployee();
            Assert.AreEqual(HttpStatusCode.OK, forgotResult.StatusCode);

            var (type, id, code) = Util.ParseLatestEmail("TempAdmin", "test@test.com");
            id = correctId ? id : newId;
            code = correctCode ? code : newCode;
            Assert.AreEqual("change-password", type);
            var verifyResult = await _client.ChangePassword(Util.NewPassword, Util.NewPassword, id, code);
            Assert.AreEqual(HttpStatusCode.Unauthorized, verifyResult.StatusCode);
        }

        /// <summary>
        /// Test if the password is correctly reset when specifying the email of an end user.
        /// </summary>
        [TestMethod]
        public async Task ForgotPasswordEndUser_Success()
        {
            const int index = 2;
            var email = $"test{index}@test.com";
            var (registerResult, _) = await _client.CreateTestEndUser(index, true);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);

            var forgotResult =  await _client.ForgotPasswordEndUser(email);
            Assert.AreEqual(HttpStatusCode.OK, forgotResult.StatusCode);

            var (type, id, code) = Util.ParseLatestEmail("TempEndUser", email);
            Assert.AreEqual("change-password", type);

            var changeResult = await _client.ChangePassword(Util.NewPassword, Util.NewPassword, id, code);
            Assert.AreEqual(HttpStatusCode.OK, changeResult.StatusCode);

            var loginResult = await _client.LoginEndUser(email, Util.NewPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);
        }

        /// <summary>
        /// Test if no email is send if it is not known for an end user.
        /// </summary>
        [TestMethod]
        public async Task ForgotPasswordEndUser_NoNewEmail()
        {
            var exists = new DirectoryInfo("./emails").GetDirectories()
                .Any(info => info.Name.Contains("test1@test.com"));
            Assert.IsFalse(exists);

            var forgotResult =  await _client.ForgotPasswordEndUser("test1@test.com");
            Assert.AreEqual(HttpStatusCode.OK, forgotResult.StatusCode);

            var exists1 = new DirectoryInfo("./emails").GetDirectories()
                .Any(info => info.Name.Contains("test1@test.com"));
            Assert.IsFalse(exists1);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when changing the password of an end user via the email
        /// for which the id or code is invalid.
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
        [DataRow(3, false, "-1", true, "code", DisplayName = "ForgotPasswordEndUser_Unauthorized_InvalidId")]
        [DataRow(4, true, "-1", false, "code", DisplayName = "ForgotPasswordEndUser_Unauthorized_InvalidCode")]
        [DataTestMethod]
        public async Task ForgotPasswordEndUser_Unauthorized(int index, bool correctId, string newId, bool correctCode, string newCode)
        {
            var email = $"test{index}@test.com";
            var (registerResult, _) = await _client.CreateTestEndUser(index, true);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);

            var forgotResult =  await _client.ForgotPasswordEndUser(email);
            Assert.AreEqual(HttpStatusCode.OK, forgotResult.StatusCode);

            var (type, id, code) = Util.ParseLatestEmail("TempEndUser", email);
            id = correctId ? id : newId;
            code = correctCode ? code : newCode;
            Assert.AreEqual("change-password", type);
            var verifyResult = await _client.ChangePassword(Util.NewPassword, Util.NewPassword, id, code);
            Assert.AreEqual(HttpStatusCode.Unauthorized, verifyResult.StatusCode);
        }

        /// <summary>
        /// Test if the password is correctly reset for the currently logged in end user.
        /// </summary>
        [TestMethod]
        public async Task ForgotPasswordCurrentEndUser_Success()
        {
            const int index = 2;
            var email = $"test{index}@test.com";
            var (registerResult, _) = await _client.CreateTestEndUser(index, true);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);

            var loginResult = await _client.LoginEndUser(email, Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var forgotResult =  await _client.ForgotPasswordCurrentEndUser();
            Assert.AreEqual(HttpStatusCode.OK, forgotResult.StatusCode);

            var (type, id, code) = Util.ParseLatestEmail("TempEndUser", email);
            Assert.AreEqual("change-password", type);

            var changeResult = await _client.ChangePassword(Util.NewPassword, Util.NewPassword, id, code);
            Assert.AreEqual(HttpStatusCode.OK, changeResult.StatusCode);

            var loggedIn = await _client.LoggedIn();
            Assert.IsFalse(loggedIn);

            var login2Result = await _client.LoginEndUser(email, Util.NewPassword);
            Assert.AreEqual(HttpStatusCode.OK, login2Result.StatusCode);
        }

        /// <summary>
        /// Test if no email is send for the currently logged in employee if an employee is not logged in.
        /// </summary>
        [TestMethod]
        public async Task ForgotPasswordCurrentEndUser_NoNewEmail()
        {
            var exists = new DirectoryInfo("./emails").GetDirectories()
                .Any(info => info.Name.Contains("test1@test.com"));
            Assert.IsFalse(exists);

            var forgotResult =  await _client.ForgotPasswordCurrentEndUser();
            Assert.AreEqual(HttpStatusCode.Unauthorized, forgotResult.StatusCode);

            var exists1 = new DirectoryInfo("./emails").GetDirectories()
                .Any(info => info.Name.Contains("test1@test.com"));
            Assert.IsFalse(exists1);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when changing the password for the currently logged in end user
        /// for which the id or code is invalid.
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
        [DataRow(3, false, "-1", true, "code", DisplayName = "ForgotPasswordCurrentEndUser_Unauthorized_InvalidId")]
        [DataRow(4, true, "-1", false, "code", DisplayName = "ForgotPasswordCurrentEndUser_Unauthorized_InvalidCode")]
        [DataTestMethod]
        public async Task ForgotPasswordCurrentEndUser_Unauthorized(int index, bool correctId, string newId, bool correctCode, string newCode)
        {
            var email = $"test{index}@test.com";
            var (registerResult, _) = await _client.CreateTestEndUser(index, true);
            Assert.AreEqual(HttpStatusCode.OK, registerResult.StatusCode);

            var loginResult = await _client.LoginEndUser(email, Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var forgotResult =  await _client.ForgotPasswordCurrentEndUser();
            Assert.AreEqual(HttpStatusCode.OK, forgotResult.StatusCode);

            var (type, id, code) = Util.ParseLatestEmail("TempEndUser", email);
            id = correctId ? id : newId;
            code = correctCode ? code : newCode;
            Assert.AreEqual("change-password", type);
            var verifyResult = await _client.ChangePassword(Util.NewPassword, Util.NewPassword, id, code);
            Assert.AreEqual(HttpStatusCode.Unauthorized, verifyResult.StatusCode);
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