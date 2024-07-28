using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Order.Test.Controllers.CategoryController
{
    /// <summary>
    /// Tests for deleting categories.
    /// </summary>
    [TestClass]
    public class DeleteTest : BaseTest
    {
        /// <summary>
        /// Test if a category is deleted.
        /// </summary>
        [TestMethod]
        public async Task DeleteCategory_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var deleteResult = await _client.DeleteCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, deleteResult.StatusCode);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when deleting a category if not logged in.
        /// </summary>
        [TestMethod]
        public async Task DeleteCategory_Error_Unauthorized()
        {
            var deleteResult = await _client.DeleteCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.Unauthorized, deleteResult.StatusCode);
        }
    }
}