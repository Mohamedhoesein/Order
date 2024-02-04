using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Protocol;
using Order.API.Controllers.CategoryController.Models;
using Order.API.Controllers.CategoryController.Models.Send;

namespace Order.Test.Controllers.CategoryController
{
    /// <summary>
    /// Tests for getting categories.
    /// </summary>
    [TestClass]
    public class GetTest : BaseTest
    {
        /// <summary>
        /// Test if getting the categories for the employees is correct.
        /// </summary>
        [TestMethod]
        public async Task GetCategoriesEmployee_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult1 = await _client.CreateMainCategory("TestMainCategory1");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult1.StatusCode);

            var createCategoryResult1 = await _client.CreateCategory("TestMainCategory1", "TestCategory1");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult1.StatusCode);

            var createSubResult1 = await _client.CreateSubCategory("TestMainCategory1", "TestCategory1", "TestSubcategory1");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult1.StatusCode);

            var createSubResult2 = await _client.CreateSubCategory("TestMainCategory1", "TestCategory1", "TestSubcategory2");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult2.StatusCode);

            var deleteSubResult1 = await _client.DeleteSubCategory("TestMainCategory1", "TestCategory1", "TestSubcategory2");
            Assert.AreEqual(HttpStatusCode.OK, deleteSubResult1.StatusCode);

            var createCategoryResult2 = await _client.CreateCategory("TestMainCategory1", "TestCategory2");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult2.StatusCode);

            var createSubResult3 = await _client.CreateSubCategory("TestMainCategory1", "TestCategory2", "TestSubcategory3");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult3.StatusCode);

            var deleteCategoryResult1 = await _client.DeleteCategory("TestMainCategory1", "TestCategory2");
            Assert.AreEqual(HttpStatusCode.OK, deleteCategoryResult1.StatusCode);

            var createCategoryResult3 = await _client.CreateCategory("TestMainCategory1", "TestCategory3");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult3.StatusCode);

            var deleteCategoryResult2 = await _client.DeleteCategory("TestMainCategory1", "TestCategory3");
            Assert.AreEqual(HttpStatusCode.OK, deleteCategoryResult2.StatusCode);

            var createSubResult4 = await _client.CreateSubCategory("TestMainCategory1", "TestCategory3", "TestSubcategory4");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult4.StatusCode);

            var createMainResult2 = await _client.CreateMainCategory("TestMainCategory2");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult2.StatusCode);

            var createCategoryResult4 = await _client.CreateCategory("TestMainCategory2", "TestCategory4");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult4.StatusCode);

            var createSubResult5 = await _client.CreateSubCategory("TestMainCategory2", "TestCategory4", "TestSubcategory5");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult5.StatusCode);

            var deleteMainResult = await _client.DeleteMainCategory("TestMainCategory2");
            Assert.AreEqual(HttpStatusCode.OK, deleteMainResult.StatusCode);

            var createCategoryResult5 = await _client.CreateCategory("TestMainCategory2", "TestCategory5");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult5.StatusCode);

            var createSubResult6 = await _client.CreateSubCategory("TestMainCategory2", "TestCategory5", "TestSubcategory6");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult6.StatusCode);

            var getCategories = await _client.GetCategoriesEmployee();
            Assert.AreEqual(HttpStatusCode.OK, getCategories.StatusCode);

            var categories =
                await _client.Deserialize<MainCategory[]>(getCategories);
            Console.WriteLine(await getCategories.Content.ReadAsStringAsync());

            Assert.IsNotNull(categories);
            Assert.AreEqual(2, categories.Length);
            Assert.AreEqual("TestMainCategory1", categories[0].Name);
            Assert.IsFalse(categories[0].Deleted);
            Assert.AreEqual(3, categories[0].Categories.Length);
            Assert.AreEqual("TestCategory1", categories[0].Categories[0].Name);
            Assert.IsFalse(categories[0].Categories[0].Deleted);
            Assert.AreEqual(2, categories[0].Categories[0].Subcategories.Length);
            Assert.AreEqual("TestSubcategory1", categories[0].Categories[0].Subcategories[0].Name);
            Assert.IsFalse(categories[0].Categories[0].Subcategories[0].Deleted);
            Assert.AreEqual("TestSubcategory2", categories[0].Categories[0].Subcategories[1].Name);
            Assert.IsTrue(categories[0].Categories[0].Subcategories[1].Deleted);
            Assert.AreEqual("TestCategory2", categories[0].Categories[1].Name);
            Assert.IsTrue(categories[0].Categories[1].Deleted);
            Assert.AreEqual(1, categories[0].Categories[1].Subcategories.Length);
            Assert.AreEqual("TestSubcategory3", categories[0].Categories[1].Subcategories[0].Name);
            Assert.IsTrue(categories[0].Categories[1].Subcategories[0].Deleted);
            Assert.AreEqual("TestCategory3", categories[0].Categories[2].Name);
            Assert.IsTrue(categories[0].Categories[2].Deleted);
            Assert.AreEqual(1, categories[0].Categories[2].Subcategories.Length);
            Assert.AreEqual("TestSubcategory4", categories[0].Categories[2].Subcategories[0].Name);
            Assert.IsFalse(categories[0].Categories[2].Subcategories[0].Deleted);
            
            Assert.AreEqual("TestMainCategory2", categories[1].Name);
            Assert.IsTrue(categories[1].Deleted);
            Assert.AreEqual(2, categories[1].Categories.Length);
            Assert.AreEqual("TestCategory4", categories[1].Categories[0].Name);
            Assert.IsTrue(categories[1].Categories[0].Deleted);
            Assert.AreEqual(1, categories[1].Categories[0].Subcategories.Length);
            Assert.AreEqual("TestSubcategory5", categories[1].Categories[0].Subcategories[0].Name);
            Assert.IsTrue(categories[1].Categories[0].Subcategories[0].Deleted);
            Assert.AreEqual("TestCategory5", categories[1].Categories[1].Name);
            Assert.IsFalse(categories[1].Categories[1].Deleted);
            Assert.AreEqual(1, categories[1].Categories[1].Subcategories.Length);
            Assert.AreEqual("TestSubcategory6", categories[1].Categories[1].Subcategories[0].Name);
            Assert.IsFalse(categories[1].Categories[1].Subcategories[0].Deleted);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when getting the categories for employees and not being logged in.
        /// </summary>
        [TestMethod]
        public async Task GetCategoriesEmployee_Error_Unauthorized()
        {
            var getCategories = await _client.GetCategoriesEmployee();
            Assert.AreEqual(HttpStatusCode.Unauthorized, getCategories.StatusCode);
        }

        /// <summary>
        /// Test if getting the categories is correct.
        /// </summary>
        [TestMethod]
        public async Task GetCategories_Success()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var createMainResult1 = await _client.CreateMainCategory("TestMainCategory1");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult1.StatusCode);

            var createCategoryResult1 = await _client.CreateCategory("TestMainCategory1", "TestCategory1");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult1.StatusCode);

            var createSubResult1 = await _client.CreateSubCategory("TestMainCategory1", "TestCategory1", "TestSubcategory1");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult1.StatusCode);

            var createSubResult2 = await _client.CreateSubCategory("TestMainCategory1", "TestCategory1", "TestSubcategory2");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult2.StatusCode);

            var deleteSubResult1 = await _client.DeleteSubCategory("TestMainCategory1", "TestCategory1", "TestSubcategory2");
            Assert.AreEqual(HttpStatusCode.OK, deleteSubResult1.StatusCode);

            var createCategoryResult2 = await _client.CreateCategory("TestMainCategory1", "TestCategory2");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult2.StatusCode);

            var createSubResult3 = await _client.CreateSubCategory("TestMainCategory1", "TestCategory2", "TestSubcategory3");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult3.StatusCode);

            var deleteCategoryResult1 = await _client.DeleteCategory("TestMainCategory1", "TestCategory2");
            Assert.AreEqual(HttpStatusCode.OK, deleteCategoryResult1.StatusCode);

            var createCategoryResult3 = await _client.CreateCategory("TestMainCategory1", "TestCategory3");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult3.StatusCode);

            var deleteCategoryResult2 = await _client.DeleteCategory("TestMainCategory1", "TestCategory3");
            Assert.AreEqual(HttpStatusCode.OK, deleteCategoryResult2.StatusCode);

            var createSubResult4 = await _client.CreateSubCategory("TestMainCategory1", "TestCategory3", "TestSubcategory4");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult4.StatusCode);

            var createMainResult2 = await _client.CreateMainCategory("TestMainCategory2");
            Assert.AreEqual(HttpStatusCode.OK, createMainResult2.StatusCode);

            var createCategoryResult4 = await _client.CreateCategory("TestMainCategory2", "TestCategory4");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult4.StatusCode);

            var createSubResult5 = await _client.CreateSubCategory("TestMainCategory2", "TestCategory4", "TestSubcategory5");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult5.StatusCode);

            var deleteMainResult = await _client.DeleteMainCategory("TestMainCategory2");
            Assert.AreEqual(HttpStatusCode.OK, deleteMainResult.StatusCode);

            var createCategoryResult5 = await _client.CreateCategory("TestMainCategory2", "TestCategory5");
            Assert.AreEqual(HttpStatusCode.OK, createCategoryResult5.StatusCode);

            var createSubResult6 = await _client.CreateSubCategory("TestMainCategory2", "TestCategory5", "TestSubcategory6");
            Assert.AreEqual(HttpStatusCode.OK, createSubResult6.StatusCode);

            var getCategories = await _client.GetCategories();
            Assert.AreEqual(HttpStatusCode.OK, getCategories.StatusCode);

            var categories =
                await _client.Deserialize<MainCategory[]>(getCategories);

            Assert.IsNotNull(categories);
            Assert.AreEqual(1, categories.Length);
            Assert.AreEqual("TestMainCategory1", categories[0].Name);
            Assert.IsFalse(categories[0].Deleted);
            Assert.AreEqual(1, categories[0].Categories.Length);
            Assert.AreEqual("TestCategory1", categories[0].Categories[0].Name);
            Assert.IsFalse(categories[0].Categories[0].Deleted);
            Assert.AreEqual(1, categories[0].Categories[0].Subcategories.Length);
            Assert.AreEqual("TestSubcategory1", categories[0].Categories[0].Subcategories[0].Name);
            Assert.IsFalse(categories[0].Categories[0].Subcategories[0].Deleted);
        }

        /// <summary>
        /// Test if getting the subcategory for the employees is correct.
        /// </summary>
        [TestMethod]
        public async Task GetSubcategoryAdmin_Success()
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

            var getResult = await _client.GetSubcategoryAdmin("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, getResult.StatusCode);

            var wholeSubcategory = await _client.Deserialize<WholeSubcategory>(getResult);
            Assert.IsNotNull(wholeSubcategory);
            Assert.AreEqual("TestSubcategory", wholeSubcategory.Name);
            Assert.IsFalse(wholeSubcategory.Deleted);
            Assert.AreEqual(3, wholeSubcategory.OpenSpecifications.Length);
            Assert.AreEqual("OpenSpecification1", wholeSubcategory.OpenSpecifications[0].Name);
            Assert.IsFalse(wholeSubcategory.OpenSpecifications[0].Deleted);
            Assert.AreEqual("OpenSpecification2", wholeSubcategory.OpenSpecifications[1].Name);
            Assert.IsTrue(wholeSubcategory.OpenSpecifications[1].Deleted);
            Assert.AreEqual("OpenSpecification3", wholeSubcategory.OpenSpecifications[2].Name);
            Assert.IsTrue(wholeSubcategory.OpenSpecifications[2].Deleted);
            Assert.AreEqual(4, wholeSubcategory.ClosedSpecifications.Length);
            Assert.AreEqual("ClosedSpecification1", wholeSubcategory.ClosedSpecifications[0].Name);
            Assert.AreEqual(3, wholeSubcategory.ClosedSpecifications[0].Values.Length);
            Assert.AreEqual("Value1", wholeSubcategory.ClosedSpecifications[0].Values[0].Value);
            Assert.IsFalse(wholeSubcategory.ClosedSpecifications[0].Values[0].Deleted);
            Assert.AreEqual("Value2", wholeSubcategory.ClosedSpecifications[0].Values[1].Value);
            Assert.IsTrue(wholeSubcategory.ClosedSpecifications[0].Values[1].Deleted);
            Assert.AreEqual("Value3", wholeSubcategory.ClosedSpecifications[0].Values[2].Value);
            Assert.IsTrue(wholeSubcategory.ClosedSpecifications[0].Values[2].Deleted);
            Assert.IsNull(wholeSubcategory.ClosedSpecifications[0].Filter);
            Assert.AreEqual("ClosedSpecification2", wholeSubcategory.ClosedSpecifications[1].Name);
            Assert.IsTrue(wholeSubcategory.ClosedSpecifications[1].Deleted);
            Assert.AreEqual(0, wholeSubcategory.ClosedSpecifications[1].Values.Length);
            Assert.IsNull(wholeSubcategory.ClosedSpecifications[1].Filter);
            Assert.AreEqual("ClosedSpecification3", wholeSubcategory.ClosedSpecifications[2].Name);
            Assert.IsTrue(wholeSubcategory.ClosedSpecifications[2].Deleted);
            Assert.AreEqual(0, wholeSubcategory.ClosedSpecifications[2].Values.Length);
            Assert.IsNull(wholeSubcategory.ClosedSpecifications[2].Filter);
            Assert.AreEqual("ClosedSpecification4", wholeSubcategory.ClosedSpecifications[3].Name);
            Assert.IsFalse(wholeSubcategory.ClosedSpecifications[3].Deleted);
            Assert.AreEqual(0, wholeSubcategory.ClosedSpecifications[3].Values.Length);
            Assert.IsNotNull(wholeSubcategory.ClosedSpecifications[3].Filter);
            Assert.AreEqual("Filter", wholeSubcategory.ClosedSpecifications[3].Filter);
        }

        /// <summary>
        /// Test if an unauthorized response is returned when getting the subcategory for employees and not being logged in.
        /// </summary>
        [TestMethod]
        public async Task GetSubcategoryAdmin_Error_Unauthorized()
        {
            var getCategories = await _client.GetSubcategoryAdmin("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.Unauthorized, getCategories.StatusCode);
        }

        /// <summary>
        /// Test if an not found response is returned when getting the subcategory for employees and it does not exist.
        /// </summary>
        [TestMethod]
        public async Task GetSubcategoryAdmin_Error_NotFound()
        {
            var loginResult = await _client.LoginEmployee("test@test.com", Util.DefaultPassword);
            Assert.AreEqual(HttpStatusCode.OK, loginResult.StatusCode);

            var getCategories = await _client.GetSubcategoryAdmin("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.NotFound, getCategories.StatusCode);
        }

        /// <summary>
        /// Test if getting the subcategory is correct.
        /// </summary>
        [TestMethod]
        public async Task GetSubcategory_Success()
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

            var logoutResult = await _client.PostAsync("auth/logout");
            Assert.AreEqual(HttpStatusCode.OK, logoutResult.StatusCode);

            var getResult = await _client.GetSubcategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.OK, getResult.StatusCode);

            var wholeSubcategory = await _client.Deserialize<WholeSubcategory>(getResult);
            Assert.IsNotNull(wholeSubcategory);
            Assert.AreEqual("TestSubcategory", wholeSubcategory.Name);
            Assert.IsFalse(wholeSubcategory.Deleted);
            Assert.AreEqual(1, wholeSubcategory.OpenSpecifications.Length);
            Assert.AreEqual("OpenSpecification1", wholeSubcategory.OpenSpecifications[0].Name);
            Assert.IsFalse(wholeSubcategory.OpenSpecifications[0].Deleted);
            Assert.AreEqual(2, wholeSubcategory.ClosedSpecifications.Length);
            Assert.AreEqual("ClosedSpecification1", wholeSubcategory.ClosedSpecifications[0].Name);
            Assert.AreEqual(1, wholeSubcategory.ClosedSpecifications[0].Values.Length);
            Assert.AreEqual("Value1", wholeSubcategory.ClosedSpecifications[0].Values[0].Value);
            Assert.IsFalse(wholeSubcategory.ClosedSpecifications[0].Values[0].Deleted);
            Assert.IsNull(wholeSubcategory.ClosedSpecifications[0].Filter);
            Assert.AreEqual("ClosedSpecification4", wholeSubcategory.ClosedSpecifications[1].Name);
            Assert.IsFalse(wholeSubcategory.ClosedSpecifications[1].Deleted);
            Assert.AreEqual(0, wholeSubcategory.ClosedSpecifications[1].Values.Length);
            Assert.IsNotNull(wholeSubcategory.ClosedSpecifications[1].Filter);
            Assert.AreEqual("Filter", wholeSubcategory.ClosedSpecifications[1].Filter);
        }

        /// <summary>
        /// Test if an not found response is returned when getting the subcategory and it does not exist.
        /// </summary>
        [TestMethod]
        public async Task GetSubcategory_Error_NotFound()
        {
            var getCategories = await _client.GetSubcategory("TestMainCategory", "TestCategory", "TestSubcategory");
            Assert.AreEqual(HttpStatusCode.NotFound, getCategories.StatusCode);
        }
    }
}