using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Order.API.Controllers.ProductController.Models.Receive;

namespace Order.Test.CookieHttpClient
{
    /// <summary>
    /// The file used by the product part of <see cref="CookieHttpClient"/>.
    /// </summary>
    public enum FileType
    {
        Dark,
        Light,
        Jellyfish,
        Empty
    }

    /// <summary>
    /// Information of a file to upload.
    /// </summary>
    public class File
    {
        public FileType Type { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// The <see cref="CookieHttpClient"/> functions associated with products.
    /// </summary>
    public partial class CookieHttpClient
    {
        /// <summary>
        /// Create a product.
        /// </summary>
        /// <param name="mainCategory">
        /// The main category for the product.
        /// </param>
        /// <param name="category">
        /// The category for the product.
        /// </param>
        /// <param name="subcategory">
        /// The subcategory for the product.
        /// </param>
        /// <param name="product">
        /// The product information, <see cref="NewProduct.Images"/> will be ignored and <see cref="files"/> will be used.
        /// </param>
        /// <param name="files">
        /// The images for the product.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the creation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// An exception thrown when an <see cref="FileType"/> is invalid.
        /// </exception>
        public Task<HttpResponseMessage> CreateProduct(string mainCategory, string category, string subcategory, NewProduct product, File[] files)
        {
            var form = new MultipartFormDataContent();

            form.Add(new StringContent(product.Name), "Name");
            form.Add(new StringContent(product.Description), "Description");
            form.Add(new StringContent(product.Price.ToString()), "Price");
            form.Add(new StringContent(product.Deleted.ToString()), "Deleted");

            for (var i = 0; i < product.ClosedSpecificationValues.Length; i++)
            {
                var closedSpecificationValue = product.ClosedSpecificationValues[i];
                form.Add(new StringContent(closedSpecificationValue.Specification), $"ClosedSpecificationValues[{i}][Specification]");
                form.Add(new StringContent(closedSpecificationValue.Value), $"ClosedSpecificationValues[{i}][Value]");
            }

            for (var i = 0; i < product.OpenSpecificationValues.Length; i++)
            {
                var openSpecificationValue = product.OpenSpecificationValues[i];
                form.Add(new StringContent(openSpecificationValue.Specification), $"OpenSpecificationValues[{i}][Specification]");
                form.Add(new StringContent(openSpecificationValue.Value), $"OpenSpecificationValues[{i}][Value]");
            }

            for (var i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var content = TypeToContent(file.Type);
                if (content != null)
                    form.Add(content, $"Images[{i}].File", file.Name);
                form.Add(new StringContent(file.Name), $"Images[{i}].Name");
            }

            return PostAsync(
                $"product/employee/{mainCategory}/{category}/{subcategory}",
                form
            );
        }

        /// <summary>
        /// Update a product.
        /// </summary>
        /// <param name="product">
        /// The product information, <see cref="Product.Images"/> will be ignored and <see cref="files"/> will be used.
        /// </param>
        /// <param name="files">
        /// The images for the product.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the updating.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// An exception thrown when an <see cref="FileType"/> is invalid.
        /// </exception>
        public Task<HttpResponseMessage> UpdateProduct(Product product, File[] files)
        {
            var form = new MultipartFormDataContent();

            form.Add(new StringContent(product.Id.ToString()), "Id");
            form.Add(new StringContent(product.Name), "Name");
            form.Add(new StringContent(product.Description), "Description");
            form.Add(new StringContent(product.Price.ToString()), "Price");
            form.Add(new StringContent(product.Deleted.ToString()), "Deleted");

            for (var i = 0; i < product.ClosedSpecificationValues.Length; i++)
            {
                var closedSpecificationValue = product.ClosedSpecificationValues[i];
                form.Add(new StringContent(closedSpecificationValue.Specification), $"ClosedSpecificationValues[{i}][Specification]");
                form.Add(new StringContent(closedSpecificationValue.Value), $"ClosedSpecificationValues[{i}][Value]");
            }

            for (var i = 0; i < product.OpenSpecificationValues.Length; i++)
            {
                var openSpecificationValue = product.OpenSpecificationValues[i];
                form.Add(new StringContent(openSpecificationValue.Specification), $"OpenSpecificationValues[{i}][Specification]");
                form.Add(new StringContent(openSpecificationValue.Value), $"OpenSpecificationValues[{i}][Value]");
            }

            for (var i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var content = TypeToContent(file.Type);
                if (content != null)
                    form.Add(content, $"Images[{i}].File", file.Name);
                form.Add(new StringContent(file.Name), $"Images[{i}].Name");
                i++;
            }

            return PostAsync(
                "product/employee",
                form
            );
        }

        /// <summary>
        /// Delete a product.
        /// </summary>
        /// <param name="id">
        /// The id of the product.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the deletion.
        /// </returns>
        public Task<HttpResponseMessage> DeleteProduct(long id)
        {
            return DeleteAsync($"product/employee/{id}");
        }

        /// <summary>
        /// Restore a product.
        /// </summary>
        /// <param name="id">
        /// The id of the product.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the restoration.
        /// </returns>
        public Task<HttpResponseMessage> RestoreProduct(long id)
        {
            return PostAsync($"product/employee/{id}");
        }

        /// <summary>
        /// Get products for employees.
        /// </summary>
        /// <param name="mainCategory">
        /// The main category for which to get the products.
        /// </param>
        /// <param name="category">
        /// The category for which to get the products.
        /// </param>
        /// <param name="subcategory">
        /// The subcategory for which to get the products.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the retrieval of the products.
        /// </returns>
        public Task<HttpResponseMessage> GetEmployeeProducts(string mainCategory, string category, string subcategory)
        {
            return GetAsync($"product/employee/{mainCategory}/{category}/{subcategory}");
        }

        /// <summary>
        /// Get products for end users.
        /// </summary>
        /// <param name="mainCategory">
        /// The main category for which to get the products.
        /// </param>
        /// <param name="category">
        /// The category for which to get the products.
        /// </param>
        /// <param name="subcategory">
        /// The subcategory for which to get the products.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/> representing the result of the retrieval of the products.
        /// </returns>
        public Task<HttpResponseMessage> GetProducts(string mainCategory, string category, string subcategory)
        {
            return GetAsync($"product/enduser/{mainCategory}/{category}/{subcategory}");
        }

        /// <summary>
        /// Convert a <see cref="FileType"/> to a <see cref="StreamContent"/> for forms.
        /// </summary>
        /// <param name="type">
        /// The <see cref="FileType"/> to convert.
        /// </param>
        /// <returns>
        /// The resulting <see cref="StreamContent"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// An exception thrown when the <see cref="FileType"/> is invalid.
        /// </exception>
        private StreamContent? TypeToContent(FileType type)
        {
            StreamContent? content;
            switch (type)
            {
                case FileType.Dark:
                    content = new StreamContent(new FileStream("Images/Dark.jpg", FileMode.Open));
                    content.Headers.ContentType = new MediaTypeHeaderValue("images/jpeg");
                    break;
                case FileType.Light:
                    content = new StreamContent(new FileStream("Images/Light.jpg", FileMode.Open));
                    content.Headers.ContentType = new MediaTypeHeaderValue("images/jpeg");
                    break;
                case FileType.Jellyfish:
                    content = new StreamContent(new FileStream("Images/Jellyfish.png", FileMode.Open));
                    content.Headers.ContentType = new MediaTypeHeaderValue("images/png");
                    break;
                case FileType.Empty:
                    content = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("files");
            }

            return content;
        }
    }
}