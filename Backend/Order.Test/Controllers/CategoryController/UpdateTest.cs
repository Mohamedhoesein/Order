using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Order.Test.Controllers.CategoryController
{
    /// <summary>
    /// Tests for updating a subcategory.
    /// </summary>
    [TestClass]
    public class UpdateTest : BaseTest
    {
        /// <summary>
        /// Test if new information is correctly added.
        /// </summary>
        [TestMethod]
        public async Task Success_New()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var updateSubResult = await _client.UpdateSubcategory("TestMainCategory", "TestCategory", "TestSubcategory", Util.NewSubcategory);
            Assert.AreEqual(HttpStatusCode.OK, updateSubResult.StatusCode);
        }

        /// <summary>
        /// Test if information is correctly updated.
        /// </summary>
        [TestMethod]
        public async Task Success_Update()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var updateSubResult1 = await _client.UpdateSubcategory("TestMainCategory", "TestCategory", "TestSubcategory", Util.NewSubcategory);
            Assert.AreEqual(HttpStatusCode.OK, updateSubResult1.StatusCode);

            var updateSubResult2 = await _client.UpdateSubcategory("TestMainCategory", "TestCategory", "TestSubcategory", Util.UpdatedSubcategory);
            Assert.AreEqual(HttpStatusCode.OK, updateSubResult2.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when the subcategory in the URI and in the body are different.
        /// </summary>
        [TestMethod]
        public async Task Error_BadRequest()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var updateSubResult2 = await _client.UpdateSubcategory("TestMainCategory", "TestCategory", "TestSubcategory1", Util.NewSubcategory);
            Assert.AreEqual(HttpStatusCode.BadRequest, updateSubResult2.StatusCode);
        }

        /// <summary>
        /// Test if a not found response is returned when the given subcategory does not exist.
        /// </summary>
        [TestMethod]
        public async Task Error_NotFound()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var updateSubResult2 = await _client.UpdateSubcategory("TestMainCategory", "TestCategory", "TestSubcategory", Util.NewSubcategory);
            Assert.AreEqual(HttpStatusCode.NotFound, updateSubResult2.StatusCode);
        }

        /// <summary>
        /// Test if a unauthorized response is returned when updating while not being logged in.
        /// </summary>
        [TestMethod]
        public async Task Error_Unauthorized()
        {
            var updateSubResult2 = await _client.UpdateSubcategory("TestMainCategory", "TestCategory", "TestSubcategory", Util.NewSubcategory);
            Assert.AreEqual(HttpStatusCode.Unauthorized, updateSubResult2.StatusCode);
        }
    }
}