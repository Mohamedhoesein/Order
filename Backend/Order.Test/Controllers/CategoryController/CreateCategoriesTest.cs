using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Order.Test.Controllers.CategoryController
{
    /// <summary>
    /// Tests creating categories.
    /// </summary>
    [TestClass]
    public class CreateCategoriesTest : BaseTest
    {
        /// <summary>
        /// Test if a main category is created.
        /// </summary>
        [TestMethod]
        public async Task CreateMainCategory_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);
        }

        /// <summary>
        /// Test if a main category is created if it was deleted.
        /// </summary>
        [TestMethod]
        public async Task CreateMainCategory_Success_Deleted()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult1 = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult1.StatusCode);

            var deleteMainResult = await _client.DeleteMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, deleteMainResult.StatusCode);

            var createMainResult2 = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult2.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when the main category exists.
        /// </summary>
        [TestMethod]
        public async Task CreateMainCategory_Error_Exists()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult1 = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult1.StatusCode);

            var createMainResult2 = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.BadRequest, createMainResult2.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when not logged in when creating a main category.
        /// </summary>
        [TestMethod]
        public async Task CreateMainCategory_Error_Unauthorized()
        {
            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.Unauthorized, createMainResult.StatusCode);
        }

        /// <summary>
        /// Test if a category is created.
        /// </summary>
        [TestMethod]
        public async Task CreateCategory_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);
        }

        /// <summary>
        /// Test if a category is created if it was deleted.
        /// </summary>
        [TestMethod]
        public async Task CreateCategory_Success_Deleted()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult1 = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult1.StatusCode);

            var deleteCategoryResult = await _client.DeleteCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, deleteCategoryResult.StatusCode);

            var createCategoryResult2 = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult2.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when the category exists.
        /// </summary>
        [TestMethod]
        public async Task CreateCategory_Error_Exists()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult1 = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult1.StatusCode);

            var createCategoryResult2 = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.BadRequest, createCategoryResult2.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when not logged in when creating a category.
        /// </summary>
        [TestMethod]
        public async Task CreateCategory_Error_Unauthorized()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var logoutResult = await _client.PostAsync("auth/logout");
            Assert.AreEqual(HttpStatusCode.OK, logoutResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.Unauthorized, createCategoryResult.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when the main category does not exist.
        /// </summary>
        [TestMethod]
        public async Task CreateCategory_Error_NotExists()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult1 = await _client.CreateCategory("TestMainCategory1", "TestCategory");
            Assert.AreEqual(HttpStatusCode.NotFound, createCategoryResult1.StatusCode);
        }

        /// <summary>
        /// Test if a sub category is created.
        /// </summary>
        [TestMethod]
        public async Task CreateSubCategory_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);
        }

        /// <summary>
        /// Test if a sub category is created if it was deleted.
        /// </summary>
        [TestMethod]
        public async Task CreateSubCategory_Success_Deleted()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult1 = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult1.StatusCode);

            var deleteSubResult = await _client.DeleteSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, deleteSubResult.StatusCode);

            var createSubResult2 = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult2.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when the sub category exists. 
        /// </summary>
        [TestMethod]
        public async Task CreateSubCategory_Error_Exists()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult1 = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult1.StatusCode);

            var createSubResult2 = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.BadRequest, createSubResult2.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when not logged in when creating a category.
        /// </summary>
        [TestMethod]
        public async Task CreateSubCategory_Error_Unauthorized()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var logoutResult = await _client.PostAsync("auth/logout");
            Assert.AreEqual(HttpStatusCode.OK, logoutResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.Unauthorized, createSubResult.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when the category does not exist. 
        /// </summary>
        [TestMethod]
        public async Task CreateSubCategory_Error_NotExists()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult1 = await _client.CreateSubCategory("TestMainCategory", "TestCategory1", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.NotFound, createSubResult1.StatusCode);
        }
    }
}