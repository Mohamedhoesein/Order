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

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var updateSubResult = await _client.UpdateSubcategory("TestMainCategory", "TestCategory", "TestSubcategory", Util.SimpleSubcategory);
            Assert.AreEqual(HttpStatusCode.OK, updateSubResult.StatusCode);

            var createProductResult = await _client.CreateProduct(
                "TestMainCategory",
                "TestCategory",
                "TestSubcategory",
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

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var updateSubResult = await _client.UpdateSubcategory("TestMainCategory", "TestCategory", "TestSubcategory", Util.SimpleSubcategory);
            Assert.AreEqual(HttpStatusCode.OK, updateSubResult.StatusCode);

            var createProductResult = await _client.CreateProduct(
                "TestMainCategory",
                "TestCategory",
                "TestSubcategory",
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
                "TestMainCategory",
                "TestCategory",
                "TestSubcategory",
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

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var updateSubResult = await _client.UpdateSubcategory("TestMainCategory", "TestCategory", "TestSubcategory", Util.SimpleSubcategory);
            Assert.AreEqual(HttpStatusCode.OK, updateSubResult.StatusCode);

            var createProductResult = await _client.CreateProduct(
                "TestMainCategory",
                "TestCategory",
                "TestSubcategory",
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

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var updateSubResult = await _client.UpdateSubcategory("TestMainCategory", "TestCategory", "TestSubcategory", Util.SimpleSubcategory);
            Assert.AreEqual(HttpStatusCode.OK, updateSubResult.StatusCode);

            var createProductResult = await _client.CreateProduct(
                "TestMainCategory",
                "TestCategory",
                "TestSubcategory",
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

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var updateSubResult = await _client.UpdateSubcategory("TestMainCategory", "TestCategory", "TestSubcategory", Util.SimpleSubcategory);
            Assert.AreEqual(HttpStatusCode.OK, updateSubResult.StatusCode);

            var createProductResult = await _client.CreateProduct(
                "TestMainCategory",
                "TestCategory",
                "TestSubcategory",
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

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var updateSubResult = await _client.UpdateSubcategory("TestMainCategory", "TestCategory", "TestSubcategory", Util.SimpleSubcategory);
            Assert.AreEqual(HttpStatusCode.OK, updateSubResult.StatusCode);

            var createProductResult = await _client.CreateProduct(
                "TestMainCategory",
                "TestCategory",
                "TestSubcategory",
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