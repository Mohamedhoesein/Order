using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Order.Test.CookieHttpClient;

namespace Order.Test.Controllers.ProductController
{
    /// <summary>
    /// Tests for updating a product.
    /// </summary>
    [TestClass]
    public class UpdateTest : BaseTest
    {
        /// <summary>
        /// Test if a product is updated.
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

            var product = Util.UpdateProduct;
            product.Id = 1;
            var updateProductResult = await _client.UpdateProduct(
                product,
                new[]
                {
                    new File
                    {
                        Name = "Dark",
                        Type = FileType.Dark
                    },
                    new File
                    {
                        Name = "Dark",
                        Type = FileType.Empty
                    }
                });
            Assert.AreEqual(HttpStatusCode.OK, updateProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a unauthorized response is returned when not logged in.
        /// </summary>
        [TestMethod]
        public async Task Error_Unauthorized()
        {
            var product = Util.UpdateProduct;
            product.Id = 1;
            var updateProductResult = await _client.UpdateProduct(
                product,
                new[]
                {
                    new File
                    {
                        Name = "Dark",
                        Type = FileType.Dark
                    }
                });
            Assert.AreEqual(HttpStatusCode.Unauthorized, updateProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a not found response when the product does not exist.
        /// </summary>
        [TestMethod]
        public async Task Error_MissingProduct()
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

            var product = Util.UpdateProduct;
            product.Id = 2;
            var updateProductResult = await _client.UpdateProduct(
                product,
                new[]
                {
                    new File
                    {
                        Name = "Dark",
                        Type = FileType.Dark
                    }
                });
            Assert.AreEqual(HttpStatusCode.NotFound, updateProductResult.StatusCode);
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

            var product = Util.UpdateProductExtraClosed;
            product.Id = 1;
            var updateProductResult = await _client.UpdateProduct(
                product,
                new[]
                {
                    new File
                    {
                        Name = "Dark",
                        Type = FileType.Dark
                    }
                });
            Assert.AreEqual(HttpStatusCode.BadRequest, updateProductResult.StatusCode);
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

            var product = Util.UpdateProductExtraOpen;
            product.Id = 1;
            var updateProductResult = await _client.UpdateProduct(
                product,
                new[]
                {
                    new File
                    {
                        Name = "Dark",
                        Type = FileType.Dark
                    }
                });
            Assert.AreEqual(HttpStatusCode.BadRequest, updateProductResult.StatusCode);
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

            var product = Util.UpdateProductInvalidClosed;
            product.Id = 1;
            var updateProductResult = await _client.UpdateProduct(
                product,
                new[]
                {
                    new File
                    {
                        Name = "Dark",
                        Type = FileType.Dark
                    }
                });
            Assert.AreEqual(HttpStatusCode.BadRequest, updateProductResult.StatusCode);
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

            var product = Util.UpdateProductInvalidClosed;
            product.Id = 1;
            var updateProductResult = await _client.UpdateProduct(
                product,
                new[]
                {
                    new File
                    {
                        Name = "Jellyfish",
                        Type = FileType.Jellyfish
                    }
                });
            Assert.AreEqual(HttpStatusCode.BadRequest, updateProductResult.StatusCode);
        }
    }
}