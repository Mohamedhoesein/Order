using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Order.Test.Controllers.CategoryController
{
    [TestClass]
    public class DeleteCategoriesTest : BaseTest
    {
        /// <summary>
        /// Test if a main category is deleted.
        /// </summary>
        [TestMethod]
        public async Task DeleteMainCategory_Success()
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
        }

        /// <summary>
        /// Test if an unauthorized response is returned when deleting a main category if not logged in..
        /// </summary>
        [TestMethod]
        public async Task DeleteMainCategory_Error_Unauthorized()
        {
            var deleteResult = await _client.DeleteMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.Unauthorized, deleteResult.StatusCode);
        }

        /// <summary>
        /// Test if a not found response is returned when deleting a main category if it does not exist.
        /// </summary>
        [TestMethod]
        public async Task DeleteMainCategory_Error_NotFound()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var deleteResult = await _client.DeleteMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.NotFound, deleteResult.StatusCode);
        }

        /// <summary>
        /// Test if a category is deleted.
        /// </summary>
        [TestMethod]
        public async Task DeleteCategory_Success()
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
        }

        /// <summary>
        /// Test if an unauthorized response is returned when deleting a category if not logged in..
        /// </summary>
        [TestMethod]
        public async Task DeleteCategory_Error_Unauthorized()
        {
            var deleteResult = await _client.DeleteCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.Unauthorized, deleteResult.StatusCode);
        }

        /// <summary>
        /// Test if a not found response is returned when deleting a category if it does not exist.
        /// </summary>
        [TestMethod]
        public async Task DeleteCategory_Error_NotFound()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var deleteResult = await _client.DeleteCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.NotFound, deleteResult.StatusCode);
        }

        /// <summary>
        /// Test if a subcategory is deleted.
        /// </summary>
        [TestMethod]
        public async Task DeleteSubcategory_Success()
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
        }

        /// <summary>
        /// Test if an unauthorized response is returned when deleting a subcategory if not logged in..
        /// </summary>
        [TestMethod]
        public async Task DeleteSubcategory_Error_Unauthorized()
        {
            var deleteResult = await _client.DeleteSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.Unauthorized, deleteResult.StatusCode);
        }

        /// <summary>
        /// Test if a not found response is returned when deleting a subcategory if it does not exist.
        /// </summary>
        [TestMethod]
        public async Task DeleteSubcategory_Error_NotFound()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var deleteResult = await _client.DeleteSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.NotFound, deleteResult.StatusCode);
        }
    }
}