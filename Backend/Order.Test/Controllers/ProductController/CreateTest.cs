using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Order.Test.CookieHttpClient;

namespace Order.Test.Controllers.ProductController
{
    /// <summary>
    /// Tests for creating a product.
    /// </summary>
    [TestClass]
    public class CreateTest : BaseTest
    {
        /// <summary>
        /// Test if a product is created.
        /// </summary>
        [TestMethod]
        public async Task Success()
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
                new[]
                {
                    new File
                    {
                        Name = "Dark",
                        Type = FileType.Dark
                    },
                    new File
                    {
                        Name = "Light",
                        Type = FileType.Light
                    }
                });
            Assert.AreEqual(HttpStatusCode.OK, createProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a product is created with no images.
        /// </summary>
        [TestMethod]
        public async Task Success_NoImage()
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
        }

        /// <summary>
        /// Test if an unauthorized response is returned when not logged in.
        /// </summary>
        [TestMethod]
        public async Task Error_Unauthorized()
        {
            var createProductResult = await _client.CreateProduct(
                "TestCategory",
                Util.Product,
                new[]
                {
                    new File
                    {
                        Name = "Dark",
                        Type = FileType.Dark
                    },
                    new File
                    {
                        Name = "Light",
                        Type = FileType.Light
                    }
                });
            Assert.AreEqual(HttpStatusCode.Unauthorized, createProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when having a closed specification that does not exist.
        /// </summary>
        [TestMethod]
        public async Task Error_ExtraClosedSpecification()
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
                Util.ProductExtraClosed,
                Array.Empty<File>());
            Assert.AreEqual(HttpStatusCode.BadRequest, createProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when having a open specification that does not exist.
        /// </summary>
        [TestMethod]
        public async Task Error_ExtraOpenSpecification()
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
                Util.ProductExtraOpen,
                Array.Empty<File>());
            Assert.AreEqual(HttpStatusCode.BadRequest, createProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when having an invalid closed specification valid that does not exist.
        /// </summary>
        [TestMethod]
        public async Task Error_InvalidClosedSpecification()
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
                Util.ProductInvalidClosed,
                Array.Empty<File>());
            Assert.AreEqual(HttpStatusCode.BadRequest, createProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a bad request response is returned when not uploading a jpg.
        /// </summary>
        [TestMethod]
        public async Task Error_InvalidImage()
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
                new[]
                {
                    new File
                    {
                        Name = "Jellyfish",
                        Type = FileType.Jellyfish
                    }
                });
            Assert.AreEqual(HttpStatusCode.BadRequest, createProductResult.StatusCode);
        }
    }
}