using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Order.API.Controllers.CategoryController.Models;

namespace Order.Test.CookieHttpClient
{
    /// <summary>
    /// The <see cref="CookieHttpClient"/> functions associated with categories.
    /// </summary>
    public partial class CookieHttpClient
    {
        /// <summary>
        /// Create a main category.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the main category.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the main category creation.
        /// </returns>
        public async Task<HttpResponseMessage> CreateMainCategory(string mainCategory)
        {
            return await PostAsync($"category/{mainCategory}");
        }

        /// <summary>
        /// Create a category.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the associated main category.
        /// </param>
        /// <param name="category">
        /// The name of the category.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the category creation.
        /// </returns>
        public async Task<HttpResponseMessage> CreateCategory(string mainCategory, string category)
        {
            return await PostAsync($"category/{mainCategory}/{category}");
        }

        /// <summary>
        /// Create a subcategory.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the associated main category.
        /// </param>
        /// <param name="category">
        /// The name of the associated category.
        /// </param>
        /// <param name="subcategory">
        /// The name of the subcategory.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the subcategory creation.
        /// </returns>
        public async Task<HttpResponseMessage> CreateSubCategory(string mainCategory, string category, string subcategory)
        {
            return await PostAsync($"category/{mainCategory}/{category}/{subcategory}");
        }

        /// <summary>
        /// Delete a main category.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the main category.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the main category deletion.
        /// </returns>
        public async Task<HttpResponseMessage> DeleteMainCategory(string mainCategory)
        {
            return await DeleteAsync($"category/{mainCategory}");
        }

        /// <summary>
        /// Delete a category.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the associated main category.
        /// </param>
        /// <param name="category">
        /// The name of the category.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the category deletion.
        /// </returns>
        public async Task<HttpResponseMessage> DeleteCategory(string mainCategory, string category)
        {
            return await DeleteAsync($"category/{mainCategory}/{category}");
        }

        /// <summary>
        /// Delete a subcategory.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the associated main category.
        /// </param>
        /// <param name="category">
        /// The name of the associated category.
        /// </param>
        /// <param name="subcategory">
        /// The name of the subcategory.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the subcategory deletion.
        /// </returns>
        public async Task<HttpResponseMessage> DeleteSubCategory(string mainCategory, string category, string subcategory)
        {
            return await DeleteAsync($"category/{mainCategory}/{category}/{subcategory}");
        }

        /// <summary>
        /// Get the categories for the employees.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the category retrieval.
        /// </returns>
        public async Task<HttpResponseMessage> GetCategoriesEmployee()
        {
            return await GetAsync("category/employee");
        }

        /// <summary>
        /// Get the subcategory for the employees.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the associated main category.
        /// </param>
        /// <param name="category">
        /// The name of the associated category.
        /// </param>
        /// <param name="subcategory">
        /// The name of the subcategory.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the subcategory retrieval.
        /// </returns>
        public async Task<HttpResponseMessage> GetSubcategoryAdmin(string mainCategory, string category, string subcategory)
        {
            return await GetAsync($"category/employee/{mainCategory}/{category}/{subcategory}");
        }

        /// <summary>
        /// Get the categories for the end users.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the category retrieval.
        /// </returns>
        public async Task<HttpResponseMessage> GetCategories()
        {
            return await GetAsync("category/enduser");
        }

        /// <summary>
        /// Get the subcategory for the endusers.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the associated main category.
        /// </param>
        /// <param name="category">
        /// The name of the associated category.
        /// </param>
        /// <param name="subcategory">
        /// The name of the subcategory.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the subcategory retrieval.
        /// </returns>
        public async Task<HttpResponseMessage> GetSubcategory(string mainCategory, string category, string subcategory)
        {
            return await GetAsync($"category/enduser/{mainCategory}/{category}/{subcategory}");
        }

        /// <summary>
        /// Update an subcategory.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the associated main category.
        /// </param>
        /// <param name="category">
        /// The name of the associated category.
        /// </param>
        /// <param name="subcategory">
        /// The name of the subcategory.
        /// </param>
        /// <param name="wholeSubcategory">
        /// The data for the subcategory
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the subcategory update.
        /// </returns>
        public async Task<HttpResponseMessage> UpdateSubcategory(string mainCategory, string category, string subcategory, WholeSubcategory wholeSubcategory)
        {
            return await PostAsync(
                $"category/{mainCategory}/{category}/{subcategory}/update",
                JsonContent.Create(wholeSubcategory)
            );
        }
    }
}