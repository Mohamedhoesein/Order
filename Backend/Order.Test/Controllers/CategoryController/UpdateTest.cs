using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Order.Test.Controllers.CategoryController
{
    /// <summary>
    /// Tests for updating categories.
    /// </summary>
    [TestClass]
    public class UpdateTest : BaseTest
    {
        /// <summary>
        /// Test if updating the name is successful.
        /// </summary>
        [TestMethod]
        public async Task UpdateName_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var updateResult = await _client.UpdateCategory("TestCategory", "TestCategory1");
            Assert.AreEqual(HttpStatusCode.OK, updateResult.StatusCode);
        }

        /// <summary>
        /// Test if adding an open specification is successful.
        /// </summary>
        [TestMethod]
        public async Task AddOverwriteOpenSpecification_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var updateResult = await _client.AddOverwriteOpenSpecification("TestCategory", Util.OpenSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult.StatusCode);
        }

        /// <summary>
        /// Test if an open specification was added when the category was deleted.
        /// </summary>
        [TestMethod]
        public async Task AddOverwriteOpenSpecification_Success_DeletedCategory()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var deletedResult = await _client.DeleteCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, deletedResult.StatusCode);

            var updateResult = await _client.AddOverwriteOpenSpecification("TestCategory", Util.OpenSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult.StatusCode);
        }

        /// <summary>
        /// Test if an open specification was deleted.
        /// </summary>
        [TestMethod]
        public async Task AddOverwriteOpenSpecification_Success_Delete()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var deletedResult = await _client.DeleteCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, deletedResult.StatusCode);

            var specification = Util.OpenSpecification1;
            var updateResult1 = await _client.AddOverwriteOpenSpecification("TestCategory", specification);
            Assert.AreEqual(HttpStatusCode.OK, updateResult1.StatusCode);

            specification.Deleted = true;
            var updateResult2 = await _client.AddOverwriteOpenSpecification("TestCategory", specification);
            Assert.AreEqual(HttpStatusCode.OK, updateResult2.StatusCode);
        }

        /// <summary>
        /// Test if an open specification was added when the it was deleted.
        /// </summary>
        [TestMethod]
        public async Task AddOverwriteOpenSpecification_Success_Restore()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var deletedResult = await _client.DeleteCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, deletedResult.StatusCode);

            var specification = Util.OpenSpecification1;
            var updateResult1 = await _client.AddOverwriteOpenSpecification("TestCategory", specification);
            Assert.AreEqual(HttpStatusCode.OK, updateResult1.StatusCode);

            specification.Deleted = true;
            var updateResult2 = await _client.AddOverwriteOpenSpecification("TestCategory", specification);
            Assert.AreEqual(HttpStatusCode.OK, updateResult2.StatusCode);

            specification.Deleted = false;
            var updateResult3 = await _client.AddOverwriteOpenSpecification("TestCategory", specification);
            Assert.AreEqual(HttpStatusCode.OK, updateResult3.StatusCode);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when adding an open specification and not being logged in.
        /// </summary>
        [TestMethod]
        public async Task AddOverwriteOpenSpecification_Error_Unauthorized()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var logoutResult = await _client.PostAsync("auth/logout");
            Assert.AreEqual(HttpStatusCode.OK, logoutResult.StatusCode);

            var updateResult = await _client.AddOverwriteOpenSpecification("TestCategory", Util.OpenSpecification1);
            Assert.AreEqual(HttpStatusCode.Unauthorized, updateResult.StatusCode);
        }

        /// <summary>
        /// Test if an not found response is returned when adding to a category which does not exist.
        /// </summary>
        [TestMethod]
        public async Task AddOverwriteOpenSpecification_Error_NotExist()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var updateResult = await _client.AddOverwriteOpenSpecification("TestCategory1", Util.OpenSpecification1);
            Assert.AreEqual(HttpStatusCode.NotFound, updateResult.StatusCode);
        }

        /// <summary>
        /// Test if adding a closed specification is successful.
        /// </summary>
        [TestMethod]
        public async Task AddOverwriteClosedSpecification_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var updateResult = await _client.AddOverwriteClosedSpecification("TestCategory", Util.ClosedSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult.StatusCode);
        }


        /// <summary>
        /// Test if a closed specification was added when the category was deleted.
        /// </summary>
        [TestMethod]
        public async Task AddOverwriteClosedSpecification_Success_DeletedCategory()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var deletedResult = await _client.DeleteCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, deletedResult.StatusCode);

            var updateResult = await _client.AddOverwriteOpenSpecification("TestCategory", Util.OpenSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult.StatusCode);
        }

        /// <summary>
        /// Test if a closed specification was deleted.
        /// </summary>
        [TestMethod]
        public async Task AddOverwriteClosedSpecification_Success_Delete()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var deletedResult = await _client.DeleteCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, deletedResult.StatusCode);

            var specification = Util.ClosedSpecification1;
            var updateResult1 = await _client.AddOverwriteClosedSpecification("TestCategory", specification);
            Assert.AreEqual(HttpStatusCode.OK, updateResult1.StatusCode);

            specification.Deleted = true;
            var updateResult2 = await _client.AddOverwriteClosedSpecification("TestCategory", specification);
            Assert.AreEqual(HttpStatusCode.OK, updateResult2.StatusCode);
        }

        /// <summary>
        /// Test if a closed specification was added when the it was deleted.
        /// </summary>
        [TestMethod]
        public async Task AddOverwriteClosedSpecification_Success_Restore()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var deletedResult = await _client.DeleteCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, deletedResult.StatusCode);

            var specification = Util.ClosedSpecification1;
            var updateResult1 = await _client.AddOverwriteClosedSpecification("TestCategory", specification);
            Assert.AreEqual(HttpStatusCode.OK, updateResult1.StatusCode);

            specification.Deleted = true;
            var updateResult2 = await _client.AddOverwriteClosedSpecification("TestCategory", specification);
            Assert.AreEqual(HttpStatusCode.OK, updateResult2.StatusCode);

            specification.Deleted = false;
            var updateResult3 = await _client.AddOverwriteClosedSpecification("TestCategory", specification);
            Assert.AreEqual(HttpStatusCode.OK, updateResult3.StatusCode);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when adding a closed specification and not being logged in.
        /// </summary>
        [TestMethod]
        public async Task AddOverwriteClosedSpecification_Error_Unauthorized()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var logoutResult = await _client.PostAsync("auth/logout");
            Assert.AreEqual(HttpStatusCode.OK, logoutResult.StatusCode);

            var updateResult = await _client.AddOverwriteClosedSpecification("TestCategory", Util.ClosedSpecification1);
            Assert.AreEqual(HttpStatusCode.Unauthorized, updateResult.StatusCode);
        }

        /// <summary>
        /// Test if an not found response is returned when adding to a category which does not exist.
        /// </summary>
        [TestMethod]
        public async Task AddOverwriteClosedSpecification_Error_NotExist()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createResult.StatusCode);

            var updateResult = await _client.AddOverwriteClosedSpecification("TestCategory1", Util.ClosedSpecification1);
            Assert.AreEqual(HttpStatusCode.NotFound, updateResult.StatusCode);
        }
    }
}