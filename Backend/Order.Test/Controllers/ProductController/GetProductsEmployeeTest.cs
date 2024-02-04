using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Order.API.Controllers.ProductController.Models.Send;
using Order.Test.CookieHttpClient;

namespace Order.Test.Controllers.ProductController
{
    /// <summary>
    /// Tests for getting products for employee.
    /// </summary>
    [TestClass]
    public class GetProductsEmployeeTest : BaseTest
    {
        /// <summary>
        /// Test if products are returned including a deleted one.
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

            var product1 = Util.Product;
            product1.Name = "Product";
            var createProductResult1 = await _client.CreateProduct(
                "TestMainCategory",
                "TestCategory",
                "TestSubcategory",
                product1,
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
            Assert.AreEqual(HttpStatusCode.OK, createProductResult1.StatusCode);

            var deleteProductResult = await _client.DeleteProduct(1);
            Assert.AreEqual(HttpStatusCode.OK, deleteProductResult.StatusCode);

            var product2 = Util.Product;
            product2.Name = "Product1";
            var createProductResult2 = await _client.CreateProduct(
                "TestMainCategory",
                "TestCategory",
                "TestSubcategory",
                product2,
                new[]
                {
                    new File
                    {
                        Name = "Light",
                        Type = FileType.Light
                    }
                });
            Assert.AreEqual(HttpStatusCode.OK, createProductResult2.StatusCode);

            var getProductsEmployeeResult =
                await _client.GetEmployeeProducts("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, getProductsEmployeeResult.StatusCode);

            var products = await _client.Deserialize<EmployeeProduct[]>(getProductsEmployeeResult);
            Assert.IsNotNull(products);
            Assert.AreEqual(2, products.Length);
            Assert.AreEqual(1, products[0].Id);
            Assert.AreEqual("Product", products[0].Name);
            Assert.IsTrue(products[0].Deleted);
            Assert.AreEqual(1UL, products[0].Price);
            Assert.AreEqual("Description", products[0].Description);
            Assert.AreEqual(2, products[0].ClosedSpecificationValues.Length);
            Assert.AreEqual("ClosedSpecification1", products[0].ClosedSpecificationValues[0].Specification);
            Assert.AreEqual("Value1", products[0].ClosedSpecificationValues[0].Value);
            Assert.AreEqual("ClosedSpecification2", products[0].ClosedSpecificationValues[1].Specification);
            Assert.AreEqual("Value6", products[0].ClosedSpecificationValues[1].Value);
            Assert.AreEqual(1, products[0].OpenSpecificationValues.Length);
            Assert.AreEqual("OpenSpecification1", products[0].OpenSpecificationValues[0].Specification);
            Assert.AreEqual("TEST", products[0].OpenSpecificationValues[0].Value);
            Assert.AreEqual(2, products[0].Images.Length);
            Assert.AreEqual("Dark", products[0].Images[0].Name);
            CollectionAssert.AreEqual(Util.GetImage(FileType.Dark), await _client.Download(products[0].Images[0].File));
            Assert.AreEqual("Light", products[0].Images[1].Name);
            StringAssert.StartsWith(products[0].Images[1].File, "/images/");
            CollectionAssert.AreEqual(Util.GetImage(FileType.Light), await _client.Download(products[0].Images[1].File));

            Assert.AreEqual(2, products[1].Id);
            Assert.AreEqual("Product1", products[1].Name);
            Assert.IsFalse(products[1].Deleted);
            Assert.AreEqual(1UL, products[1].Price);
            Assert.AreEqual("Description", products[1].Description);
            Assert.AreEqual(2, products[1].ClosedSpecificationValues.Length);
            Assert.AreEqual("ClosedSpecification1", products[1].ClosedSpecificationValues[0].Specification);
            Assert.AreEqual("Value1", products[1].ClosedSpecificationValues[0].Value);
            Assert.AreEqual("ClosedSpecification2", products[1].ClosedSpecificationValues[1].Specification);
            Assert.AreEqual("Value6", products[1].ClosedSpecificationValues[1].Value);
            Assert.AreEqual(1, products[1].OpenSpecificationValues.Length);
            Assert.AreEqual("OpenSpecification1", products[1].OpenSpecificationValues[0].Specification);
            Assert.AreEqual("TEST", products[1].OpenSpecificationValues[0].Value);
            Assert.AreEqual(1, products[1].Images.Length);
            Assert.AreEqual("Light", products[1].Images[0].Name);
            StringAssert.StartsWith(products[1].Images[0].File, "/images/");
            CollectionAssert.AreEqual(Util.GetImage(FileType.Light), await _client.Download(products[1].Images[0].File));
        }

        /// <summary>
        /// Test if updated products are correctly returned.
        /// </summary>
        [TestMethod]
        public async Task Success_Updated()
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

            var product1 = Util.Product;
            product1.Name = "Product";
            var createProductResult = await _client.CreateProduct(
                "TestMainCategory",
                "TestCategory",
                "TestSubcategory",
                product1,
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

            var updateProduct1 = Util.UpdateProduct;
            updateProduct1.Id = 1;
            updateProduct1.Name = "Product";
            var updateProductResult1 = await _client.UpdateProduct(
                updateProduct1,
                new[]
                {
                    new File
                    {
                        Name = "Dark",
                        Type = FileType.Empty
                    }
                });
            Assert.AreEqual(HttpStatusCode.OK, updateProductResult1.StatusCode);

            var product2 = Util.Product;
            product2.Name = "Product1";
            var createProductResult2 = await _client.CreateProduct(
                "TestMainCategory",
                "TestCategory",
                "TestSubcategory",
                product2,
                new[]
                {
                    new File
                    {
                        Name = "Light",
                        Type = FileType.Light
                    }
                });
            Assert.AreEqual(HttpStatusCode.OK, createProductResult2.StatusCode);

            var updateProduct2 = Util.UpdateProduct;
            updateProduct2.Id = 2;
            updateProduct2.Name = "Product1";
            var updateProductResult2 = await _client.UpdateProduct(
                updateProduct1,
                new[]
                {
                    new File
                    {
                        Name = "Dark",
                        Type = FileType.Dark
                    }
                });
            Assert.AreEqual(HttpStatusCode.OK, updateProductResult2.StatusCode);

            var getProductsEmployeeResult =
                await _client.GetEmployeeProducts("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, getProductsEmployeeResult.StatusCode);

            var products = await _client.Deserialize<EmployeeProduct[]>(getProductsEmployeeResult);
            Assert.IsNotNull(products);
            Assert.AreEqual(2, products.Length);
            Assert.AreEqual(1, products[0].Id);
            Assert.AreEqual("Product", products[0].Name);
            Assert.IsFalse(products[0].Deleted);
            Assert.AreEqual(1UL, products[0].Price);
            Assert.AreEqual("Description", products[0].Description);
            Assert.AreEqual(2, products[0].ClosedSpecificationValues.Length);
            Assert.AreEqual("ClosedSpecification1", products[0].ClosedSpecificationValues[0].Specification);
            Assert.AreEqual("Value1", products[0].ClosedSpecificationValues[0].Value);
            Assert.AreEqual("ClosedSpecification2", products[0].ClosedSpecificationValues[1].Specification);
            Assert.AreEqual("Value6", products[0].ClosedSpecificationValues[1].Value);
            Assert.AreEqual(1, products[0].OpenSpecificationValues.Length);
            Assert.AreEqual("OpenSpecification1", products[0].OpenSpecificationValues[0].Specification);
            Assert.AreEqual("TEST", products[0].OpenSpecificationValues[0].Value);
            Assert.AreEqual(1, products[0].Images.Length);
            Assert.AreEqual("Dark", products[0].Images[0].Name);
            CollectionAssert.AreEqual(Util.GetImage(FileType.Dark), await _client.Download(products[0].Images[0].File));

            Assert.AreEqual(2, products[1].Id);
            Assert.AreEqual("Product1", products[1].Name);
            Assert.IsFalse(products[1].Deleted);
            Assert.AreEqual(1UL, products[1].Price);
            Assert.AreEqual("Description", products[1].Description);
            Assert.AreEqual(2, products[1].ClosedSpecificationValues.Length);
            Assert.AreEqual("ClosedSpecification1", products[1].ClosedSpecificationValues[0].Specification);
            Assert.AreEqual("Value1", products[1].ClosedSpecificationValues[0].Value);
            Assert.AreEqual("ClosedSpecification2", products[1].ClosedSpecificationValues[1].Specification);
            Assert.AreEqual("Value6", products[1].ClosedSpecificationValues[1].Value);
            Assert.AreEqual(1, products[1].OpenSpecificationValues.Length);
            Assert.AreEqual("OpenSpecification1", products[1].OpenSpecificationValues[0].Specification);
            Assert.AreEqual("TEST", products[1].OpenSpecificationValues[0].Value);
            Assert.AreEqual(1, products[1].Images.Length);
            Assert.AreEqual("Dark", products[1].Images[0].Name);
            CollectionAssert.AreEqual(Util.GetImage(FileType.Dark), await _client.Download(products[0].Images[0].File));
        }

        /// <summary>
        /// Test if a not found response is returned when the subcategory does not exist.
        /// </summary>
        [TestMethod]
        public async Task Error_SubcategoryNotFound()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var getProductResult = await _client.GetEmployeeProducts("TestMainCategory", "TestCategory", "TEST");
            Assert.AreEqual(HttpStatusCode.NotFound, getProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a not found response is returned when the category does not exist.
        /// </summary>
        [TestMethod]
        public async Task Error_CategoryNotFound()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var getProductResult = await _client.GetEmployeeProducts("TestMainCategory", "TEST", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.NotFound, getProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a not found response is returned when the main category does not exist.
        /// </summary>
        [TestMethod]
        public async Task Error_MainCategoryNotFound()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult = await _client.CreateMainCategory("TestMainCategory");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult.StatusCode);

            var createCategoryResult = await _client.CreateCategory("TestMainCategory", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult.StatusCode);

            var createSubResult = await _client.CreateSubCategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult.StatusCode);

            var getProductResult = await _client.GetEmployeeProducts("TEST", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.NotFound, getProductResult.StatusCode);
        }

        /// <summary>
        /// Test if a unauthorized response is returned when not logged in.
        /// </summary>
        [TestMethod]
        public async Task Error_Unauthorized()
        {
            var getProductResult = await _client.GetEmployeeProducts("TEST", "TEST", "TEST");
            Assert.AreEqual(HttpStatusCode.Unauthorized, getProductResult.StatusCode);
        }
    }
}