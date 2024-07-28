using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Order.API.Controllers.CategoryController.Models;

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

            var createResult1 = await _client.CreateCategory("TestCategory1");
            Assert.AreEqual(HttpStatusCode.OK, createResult1.StatusCode);

            var updateResult1 = await _client.AddOverwriteOpenSpecification("TestCategory1", Util.OpenSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult1.StatusCode);

            var updateResult2 = await _client.AddOverwriteClosedSpecification("TestCategory1", Util.ClosedSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult2.StatusCode);

            var updateNameResult = await _client.UpdateCategory("TestCategory1", "TestCategory");
            Assert.AreEqual(HttpStatusCode.OK, updateNameResult.StatusCode);

            var createResult2 = await _client.CreateCategory("TestCategory2");
            Assert.AreEqual(HttpStatusCode.OK, createResult2.StatusCode);

            var deleteResult1 = await _client.DeleteCategory("TestCategory2");
            Assert.AreEqual(HttpStatusCode.OK, deleteResult1.StatusCode);

            var updateResult3 = await _client.AddOverwriteOpenSpecification("TestCategory2", Util.OpenSpecification2);
            Assert.AreEqual(HttpStatusCode.OK, updateResult3.StatusCode);

            var updateResult4 = await _client.AddOverwriteClosedSpecification("TestCategory2", Util.ClosedSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult4.StatusCode);

            var getCategories = await _client.GetCategoriesEmployee();
            Assert.AreEqual(HttpStatusCode.OK, getCategories.StatusCode);

            var categories =
                await _client.Deserialize<WholeCategory[]>(getCategories);
            Console.WriteLine(await getCategories.Content.ReadAsStringAsync());

            Assert.IsNotNull(categories);
            Assert.AreEqual(2, categories.Length);
            Assert.AreEqual("TestCategory", categories[0].Name);
            Assert.IsFalse(categories[0].Deleted);
            Assert.AreEqual(1, categories[0].OpenSpecifications.Length);
            Assert.AreEqual("OpenSpecification1", categories[0].OpenSpecifications[0].Name);
            Assert.IsFalse(categories[0].OpenSpecifications[0].Deleted);
            Assert.AreEqual(1, categories[0].ClosedSpecifications.Length);
            Assert.AreEqual("ClosedSpecification1", categories[0].ClosedSpecifications[0].Name);
            Assert.AreEqual(3, categories[0].ClosedSpecifications[0].Values.Length);
            Assert.AreEqual("Value1", categories[0].ClosedSpecifications[0].Values[0].Value);
            Assert.IsFalse(categories[0].ClosedSpecifications[0].Values[0].Deleted);
            Assert.AreEqual("Value2", categories[0].ClosedSpecifications[0].Values[1].Value);
            Assert.IsFalse(categories[0].ClosedSpecifications[0].Values[1].Deleted);
            Assert.AreEqual("Value3", categories[0].ClosedSpecifications[0].Values[2].Value);
            Assert.IsFalse(categories[0].ClosedSpecifications[0].Values[2].Deleted);
            Assert.AreEqual("Title", categories[0].ClosedSpecifications[0].Filter);
            Assert.AreEqual("TestCategory2", categories[1].Name);
            Assert.IsTrue(categories[1].Deleted);
            Assert.AreEqual(1, categories[1].OpenSpecifications.Length);
            Assert.AreEqual("OpenSpecification2", categories[1].OpenSpecifications[0].Name);
            Assert.IsFalse(categories[1].OpenSpecifications[0].Deleted);
            Assert.AreEqual(1, categories[1].ClosedSpecifications.Length);
            Assert.AreEqual("ClosedSpecification1", categories[1].ClosedSpecifications[0].Name);
            Assert.AreEqual(3, categories[1].ClosedSpecifications[0].Values.Length);
            Assert.AreEqual("Value1", categories[1].ClosedSpecifications[0].Values[0].Value);
            Assert.IsFalse(categories[1].ClosedSpecifications[0].Values[0].Deleted);
            Assert.AreEqual("Value2", categories[1].ClosedSpecifications[0].Values[1].Value);
            Assert.IsFalse(categories[1].ClosedSpecifications[0].Values[1].Deleted);
            Assert.AreEqual("Value3", categories[1].ClosedSpecifications[0].Values[2].Value);
            Assert.IsFalse(categories[1].ClosedSpecifications[0].Values[2].Deleted);
            Assert.AreEqual("Title", categories[1].ClosedSpecifications[0].Filter);
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

            var createResult1 = await _client.CreateCategory("TestCategory1");
            Assert.AreEqual(HttpStatusCode.OK, createResult1.StatusCode);

            var updateResult1 = await _client.AddOverwriteOpenSpecification("TestCategory1", Util.OpenSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult1.StatusCode);

            var updateResult2 = await _client.AddOverwriteClosedSpecification("TestCategory1", Util.ClosedSpecification1);
            Assert.AreEqual(HttpStatusCode.OK, updateResult2.StatusCode);

            var createResult2 = await _client.CreateCategory("TestCategory2");
            Assert.AreEqual(HttpStatusCode.OK, createResult2.StatusCode);

            var updateResult3 = await _client.AddOverwriteOpenSpecification("TestCategory2", Util.OpenSpecification2);
            Assert.AreEqual(HttpStatusCode.OK, updateResult3.StatusCode);

            var updateResult4 = await _client.AddOverwriteClosedSpecification("TestCategory2", Util.ClosedSpecification2);
            Assert.AreEqual(HttpStatusCode.OK, updateResult4.StatusCode);

            var deleteResult1 = await _client.DeleteCategory("TestCategory2");
            Assert.AreEqual(HttpStatusCode.OK, deleteResult1.StatusCode);

            var getCategories = await _client.GetCategories();
            Assert.AreEqual(HttpStatusCode.OK, getCategories.StatusCode);

            var categories =
                await _client.Deserialize<WholeCategory[]>(getCategories);

            Assert.IsNotNull(categories);
            Assert.AreEqual(1, categories.Length);
            Assert.AreEqual("TestCategory1", categories[0].Name);
            Assert.IsFalse(categories[0].Deleted);
            Assert.AreEqual(1, categories[0].OpenSpecifications.Length);
            Assert.AreEqual("OpenSpecification1", categories[0].OpenSpecifications[0].Name);
            Assert.IsFalse(categories[0].OpenSpecifications[0].Deleted);
            Assert.AreEqual(1, categories[0].ClosedSpecifications.Length);
            Assert.AreEqual("ClosedSpecification1", categories[0].ClosedSpecifications[0].Name);
            Assert.AreEqual(3, categories[0].ClosedSpecifications[0].Values.Length);
            Assert.AreEqual("Value1", categories[0].ClosedSpecifications[0].Values[0].Value);
            Assert.IsFalse(categories[0].ClosedSpecifications[0].Values[0].Deleted);
            Assert.AreEqual("Value2", categories[0].ClosedSpecifications[0].Values[1].Value);
            Assert.IsFalse(categories[0].ClosedSpecifications[0].Values[1].Deleted);
            Assert.AreEqual("Value3", categories[0].ClosedSpecifications[0].Values[2].Value);
            Assert.IsFalse(categories[0].ClosedSpecifications[0].Values[2].Deleted);
            Assert.AreEqual("Title", categories[0].ClosedSpecifications[0].Filter);
        }
    }
}