using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Order.Test.Controllers.CategoryController
{
    /// <summary>
    /// Tests for creating categories.
    /// </summary>
    [TestClass]
    public class CreateTest : BaseTest
    {
        /// <summary>
        /// Test if a category is created.
        /// </summary>
        [TestMethod]
        public async Task CreateCategory_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);
        }

        /// <summary>
        /// Test if a category is created if it was deleted.
        /// </summary>
        [TestMethod]
        public async Task CreateCategory_Success_Deleted()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult1 = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult1.StatusCode);

            var deleteResult = await _client.DeleteCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, deleteResult.StatusCode);

            var createResult2 = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult2.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when the category exists. 
        /// </summary>
        [TestMethod]
        public async Task CreateCategory_Error_Exists()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult1 = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult1.StatusCode);

            var createResult2 = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.BadRequest, createResult2.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when not logged in when creating a category.
        /// </summary>
        [TestMethod]
        public async Task CreateCategory_Error_Unauthorized()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var logoutResult = await _client.PostAsync("auth/logout");
            Assert.AreEqual(HttpStatusCode.OK, logoutResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.Unauthorized, createResult.StatusCode);
        }
    }
}