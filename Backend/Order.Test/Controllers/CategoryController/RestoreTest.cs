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
        /// Test if a main category is restored.
        /// </summary>
        [TestMethod]
        public async Task RestoreMainCategory_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var deleteResult = await _client.DeleteMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, deleteResult.StatusCode);

            var restoreResult = await _client.RestoreMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, restoreResult.StatusCode);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when restoring a main category if not logged in.
        /// </summary>
        [TestMethod]
        public async Task RestoreMainCategory_Unauthorized()
        {
            var restoreResult = await _client.RestoreMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.Unauthorized, restoreResult.StatusCode);
        }

        /// <summary>
        /// Test if a not found response is returned when restoring a main category if it does not exist.
        /// </summary>
        [TestMethod]
        public async Task RestoreMainCategory_NotFound()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var restoreResult = await _client.RestoreMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.NotFound, restoreResult.StatusCode);
        }

        /// <summary>
        /// Test if a category is restored.
        /// </summary>
        [TestMethod]
        public async Task RestoreCategory_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var deleteResult = await _client.DeleteCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, deleteResult.StatusCode);

            var restoreResult = await _client.RestoreCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, restoreResult.StatusCode);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when restoring a category if not logged in.
        /// </summary>
        [TestMethod]
        public async Task RestoreCategory_Unauthorized()
        {
            var restoreResult = await _client.RestoreCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.Unauthorized, restoreResult.StatusCode);
        }

        /// <summary>
        /// Test if a not found response is returned when restoring a category if it does not exist.
        /// </summary>
        [TestMethod]
        public async Task RestoreCategory_Notfound()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var restoreResult = await _client.RestoreCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.NotFound, restoreResult.StatusCode);
        }

        /// <summary>
        /// Test if a subcategory is restored.
        /// </summary>
        [TestMethod]
        public async Task RestoreSubcategory_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var deleteResult = await _client.DeleteSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, deleteResult.StatusCode);

            var restoreResult = await _client.RestoreSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, restoreResult.StatusCode);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when restoring a subcategory if not logged in.
        /// </summary>
        [TestMethod]
        public async Task RestoreSubcategory_Unauthorized()
        {
            var restoreResult = await _client.RestoreSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.Unauthorized, restoreResult.StatusCode);
        }

        /// <summary>
        /// Test if a not found response is returned when restoring a subcategory if it does not exist.
        /// </summary>
        [TestMethod]
        public async Task RestoreSubcategory_NotFound()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var restoreResult = await _client.RestoreSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.NotFound, restoreResult.StatusCode);
        }
    }
}