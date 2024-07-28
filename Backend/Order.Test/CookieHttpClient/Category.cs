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
        /// Create a category.
        /// </summary>
        /// <param name="category">
        /// The name of the category.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the category creation.
        /// </returns>
        public async Task<HttpResponseMessage> CreateCategory(string category)
        {
            return await PostAsync($"category/employee/{category}");
        }

        /// <summary>
        /// Delete a category.
        /// </summary>
        /// <param name="category">
        /// The name of the category.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the category deletion.
        /// </returns>
        public async Task<HttpResponseMessage> DeleteCategory(string category)
        {
            return await DeleteAsync($"category/employee/{category}");
        }

        /// <summary>
        /// Restore a category.
        /// </summary>
        /// <param name="category">
        /// The name of the category.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the category deletion.
        /// </returns>
        public async Task<HttpResponseMessage> RestoreCategory(string category)
        {
            return await PostAsync($"category/employee/{category}/restore");
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
        /// Update an category.
        /// </summary>
        /// <param name="oldName">
        /// The old name of the category.
        /// </param>
        /// <param name="newName">
        /// The new name of the category.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the category update.
        /// </returns>
        public async Task<HttpResponseMessage> UpdateCategory(string oldName, string newName)
        {
            return await PostAsync($"category/employee/{oldName}/update/{newName}");
        }

        /// <summary>
        /// Add or overwrite an closed specification.
        /// </summary>
        /// <param name="category">
        /// The name of the category to update.
        /// </param>
        /// <param name="specification">
        /// The specification data to use.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the closed specification update.
        /// </returns>
        public async Task<HttpResponseMessage> AddOverwriteClosedSpecification(string category, ClosedSpecification specification)
        {
            return await PostAsync(
                $"category/employee/{category}/closed",
                JsonContent.Create(specification)
            );
        }

        /// <summary>
        /// Add or overwrite an open specification.
        /// </summary>
        /// <param name="category">
        /// The name of the category to update.
        /// </param>
        /// <param name="specification">
        /// The specification data to use.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the closed specification update.
        /// </returns>
        public async Task<HttpResponseMessage> AddOverwriteOpenSpecification(string category, OpenSpecification specification)
        {
            return await PostAsync(
                $"category/employee/{category}/open",
                JsonContent.Create(specification)
            );
        }
    }
}