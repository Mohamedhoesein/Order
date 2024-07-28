using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Order.Test.Controllers.CategoryController
{
    /// <summary>
    /// Tests for restoring categories.
    /// </summary>
    [TestClass]
    public class RestoreTest : BaseTest
    {
        /// <summary>
        /// Test if a category is restored.
        /// </summary>
        [TestMethod]
        public async Task RestoreCategory_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var deleteResult = await _client.DeleteCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, deleteResult.StatusCode);

            var restoreResult = await _client.RestoreCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, restoreResult.StatusCode);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when restoring a category if not logged in.
        /// </summary>
        [TestMethod]
        public async Task RestoreCategory_Unauthorized()
        {
            var restoreResult = await _client.RestoreCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.Unauthorized, restoreResult.StatusCode);
        }

        /// <summary>
        /// Test if a not found response is returned when restoring a category if it does not exist.
        /// </summary>
        [TestMethod]
        public async Task RestoreCategory_NotFound()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var restoreResult = await _client.RestoreCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.NotFound, restoreResult.StatusCode);
        }
    }
}