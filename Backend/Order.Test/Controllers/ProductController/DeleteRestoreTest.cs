using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Order.Test.CookieHttpClient;

namespace Order.Test.Controllers.ProductController
{
    /// <summary>
    /// Tests for deleting and restoring a product.
    /// </summary>
    [TestClass]
    public class DeleteRestoreTest : BaseTest
    {
        /// <summary>
        /// Test if a product is deleted.
        /// </summary>
        [TestMethod]
        public async Task Delete_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createSubResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var updateResult1 = await _client.AddOverwriteClosedSpecification("TestCategory", Util.ClosedSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult1.StatusCode);

            var updateResult2 = await _client.AddOverwriteClosedSpecification("TestCategory", Util.ClosedSpecification2);
            Assert.AreEqual(HttpStatusCode.OK, updateResult2.StatusCode);

            var updateResult3 = await _client.AddOverwriteOpenSpecification("TestCategory", Util.OpenSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult3.StatusCode);

            var createProductResult = await _client.CreateProduct(
                "TestCategory",
                Util.Product,
                Array.Empty<File>());
            Assert.AreEqual(HttpStatusCode.OK, createProductResult.StatusCode);

            var deleteProductResult = await _client.DeleteProduct(1);
            Assert.AreEqual(HttpStatusCode.OK, deleteProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a not found response is returned when the product to be deleted does not exist.
        /// </summary>
        [TestMethod]
        public async Task Delete_Error_NotExists()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createSubResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var updateResult1 = await _client.AddOverwriteClosedSpecification("TestCategory", Util.ClosedSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult1.StatusCode);

            var updateResult2 = await _client.AddOverwriteClosedSpecification("TestCategory", Util.ClosedSpecification2);
            Assert.AreEqual(HttpStatusCode.OK, updateResult2.StatusCode);

            var updateResult3 = await _client.AddOverwriteOpenSpecification("TestCategory", Util.OpenSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult3.StatusCode);

            var createProductResult = await _client.CreateProduct(
                "TestCategory",
                Util.Product,
                Array.Empty<File>());
            Assert.AreEqual(HttpStatusCode.OK, createProductResult.StatusCode);

            var deleteProductResult = await _client.DeleteProduct(2);
            Assert.AreEqual(HttpStatusCode.NotFound, deleteProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a unauthorized response is returned when deleting a product if not logged in.
        /// </summary>
        [TestMethod]
        public async Task Delete_Error_Unauthorized()
        {
            var deleteProductResult = await _client.DeleteProduct(1);
            Assert.AreEqual(HttpStatusCode.Unauthorized, deleteProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a product is restored.
        /// </summary>
        [TestMethod]
        public async Task Restore_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createSubResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var updateResult1 = await _client.AddOverwriteClosedSpecification("TestCategory", Util.ClosedSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult1.StatusCode);

            var updateResult2 = await _client.AddOverwriteClosedSpecification("TestCategory", Util.ClosedSpecification2);
            Assert.AreEqual(HttpStatusCode.OK, updateResult2.StatusCode);

            var updateResult3 = await _client.AddOverwriteOpenSpecification("TestCategory", Util.OpenSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult3.StatusCode);

            var createProductResult = await _client.CreateProduct(
                "TestCategory",
                Util.Product,
                Array.Empty<File>());
            Assert.AreEqual(HttpStatusCode.OK, createProductResult.StatusCode);

            var deleteProductResult = await _client.DeleteProduct(1);
            Assert.AreEqual(HttpStatusCode.OK, deleteProductResult.StatusCode);

            var restoreProductResult = await _client.RestoreProduct(1);
            Assert.AreEqual(HttpStatusCode.OK, restoreProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a not found response is returned when the product to be restored does not exist.
        /// </summary>
        [TestMethod]
        public async Task Restore_Error_NotExists()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createSubResult = await _client.CreateCategory("TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var updateResult1 = await _client.AddOverwriteClosedSpecification("TestCategory", Util.ClosedSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult1.StatusCode);

            var updateResult2 = await _client.AddOverwriteClosedSpecification("TestCategory", Util.ClosedSpecification2);
            Assert.AreEqual(HttpStatusCode.OK, updateResult2.StatusCode);

            var updateResult3 = await _client.AddOverwriteOpenSpecification("TestCategory", Util.OpenSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult3.StatusCode);

            var createProductResult = await _client.CreateProduct(
                "TestCategory",
                Util.Product,
                Array.Empty<File>());
            Assert.AreEqual(HttpStatusCode.OK, createProductResult.StatusCode);

            var deleteProductResult = await _client.DeleteProduct(1);
            Assert.AreEqual(HttpStatusCode.OK, deleteProductResult.StatusCode);

            var restoreProductResult = await _client.RestoreProduct(2);
            Assert.AreEqual(HttpStatusCode.NotFound, restoreProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a unauthorized response is returned when restoring a product if not logged in.
        /// </summary>
        [TestMethod]
        public async Task Restore_Error_Unauthorized()
        {
            var restoreProductResult = await _client.RestoreProduct(1);
            Assert.AreEqual(HttpStatusCode.Unauthorized, restoreProductResult.StatusCode);
        }
    }
}