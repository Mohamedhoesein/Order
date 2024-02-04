using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.API.Context;
using Order.API.Controllers.ProductController.Models.Receive;
using Order.API.Util;
using ClosedSpecificationValue = Order.API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue;
using OpenSpecificationValue = Order.API.Controllers.ProductController.Models.Receive.OpenSpecificationValue;
using Product = Order.API.Context.Product;

namespace Order.API.Controllers.ProductController
{
    /// <summary>
    /// The controller to handle endpoints associated with products.
    /// </summary>
    [Route("product")]
    [ApiController]
    public class ProductController : BaseController
    {
        private string _path;
        private string _base;

        /// <summary>
        /// Initialize a new <see cref="ProductController"/> with the required information.
        /// </summary>
        /// <param name="orderContext">
        /// The <see cref="OrderContext"/> used to handle database access.
        /// </param>
        /// <param name="path">
        /// The location for the images.
        /// </param>
        public ProductController(OrderContext orderContext, string path, IWebHostEnvironment env) : base(orderContext)
        {
            _path = Path.Combine(env.ContentRootPath, path.TrimStart('/'));
            _base = path;
        }

        /// <summary>
        /// Get the products of a subcategory for employees.
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
        /// An <see cref="OkObjectResult"/> with the products.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.ProductManageClaim)]
        [HttpGet("employee/{mainCategory}/{category}/{subcategory}")]
        public IActionResult GetProductsEmployee([FromRoute] string mainCategory, [FromRoute] string category, [FromRoute] string subcategory)
        {
            if (_orderContext.MainCategories.All(currentMainCategory =>
                    currentMainCategory.Name != mainCategory ||
                    currentMainCategory.Categories.All(currentCategory =>
                        currentCategory.Name != category ||
                        currentCategory.Subcategories.All(currentSubcategory =>
                            currentSubcategory.Name != subcategory))))
                return NotFound();
            var products = _orderContext.Products
                .Include(product => product.ProductVersions)
                .ThenInclude(productVersion => productVersion.ProductImages)
                .Include(product => product.ProductVersions)
                .ThenInclude(productVersion => productVersion.ClosedSpecificationValues)
                .Include(product => product.ProductVersions)
                .ThenInclude(productVersion => productVersion.OpenSpecificationValues)
                .AsSingleQuery()
                .Where(product =>
                    product.SubcategoryName == subcategory &&
                    product.CategoryName == category &&
                    product.MainCategoryName == mainCategory
                ).ToList();
            return Ok(products.Select(product => new Models.Send.EmployeeProduct(product)).ToArray());
        }

        /// <summary>
        /// Get the products of a subcategory for end users.
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
        /// An <see cref="OkObjectResult"/> with the products.
        /// </returns>
        [EnableCors(Cors.AllowEndUser)]
        [AllowAnonymous]
        [HttpGet("enduser/{mainCategory}/{category}/{subcategory}")]
        public IActionResult GetProducts([FromRoute] string mainCategory, [FromRoute] string category, [FromRoute] string subcategory)
        {
            if (_orderContext.MainCategories.All(currentMainCategory =>
                    currentMainCategory.Name != mainCategory ||
                    currentMainCategory.Categories.All(currentCategory =>
                        currentCategory.Name != category ||
                        currentCategory.Subcategories.All(currentSubcategory =>
                            currentSubcategory.Name != subcategory))))
                return NotFound();
            var products = _orderContext.Products
                .Include(product => product.ProductVersions)
                .ThenInclude(productVersion => productVersion.ProductImages)
                .Include(product => product.ProductVersions)
                .ThenInclude(productVersion => productVersion.ClosedSpecificationValues)
                .Include(product => product.ProductVersions)
                .ThenInclude(productVersion => productVersion.OpenSpecificationValues)
                .AsSingleQuery()
                .Where(product =>
                    product.SubcategoryName == subcategory &&
                    product.CategoryName == category &&
                    product.MainCategoryName == mainCategory &&
                    !product.Deleted
                ).ToList();
            return Ok(products.Select(product => new Models.Send.EndUserProduct(product)).ToArray());
        }

        /// <summary>
        /// Update a product.
        /// </summary>
        /// <param name="product">
        /// The product information.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the update is successful,
        /// an <see cref="NotFoundResult"/> if the product does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.ProductManageClaim)]
        [HttpPost("employee")]
        public IActionResult UpdateProduct([FromForm]Models.Receive.Product product)
        {
            var existingProduct = _orderContext.Products
                .Include(product => product.ProductVersions)
                .ThenInclude(productVersion => productVersion.ProductImages)
                .FirstOrDefault(existingProduct => existingProduct.Id == product.Id);

            if (existingProduct == null)
                return NewStatusCode(404);

            var postedImages = product.Images ?? Array.Empty<Image>();
            var closedSpecificationValues =
                product.ClosedSpecificationValues ?? Array.Empty<ClosedSpecificationValue>();
            var openSpecificationValues =
                product.OpenSpecificationValues ?? Array.Empty<OpenSpecificationValue>();
            var existingClosedSpecificationValues = _orderContext.ClosedSpecificationValues
                .Where(existingClosedSpecificationValue =>
                    existingClosedSpecificationValue.MainCategoryName == existingProduct.MainCategoryName &&
                    existingClosedSpecificationValue.CategoryName == existingProduct.CategoryName &&
                    existingClosedSpecificationValue.SubcategoryName == existingProduct.SubcategoryName
                ).ToList();
            var existingOpenSpecifications = _orderContext.OpenSpecifications
                .Where(existingOpenSpecificationValue =>
                    existingOpenSpecificationValue.MainCategoryName == existingProduct.MainCategoryName &&
                    existingOpenSpecificationValue.CategoryName == existingProduct.CategoryName &&
                    existingOpenSpecificationValue.SubcategoryName == existingProduct.SubcategoryName
                ).ToList();
            if (closedSpecificationValues.Any(closedSpecificationValue =>
                    existingClosedSpecificationValues.All(existingClosedSpecificationValue =>
                        existingClosedSpecificationValue.SpecificationName != closedSpecificationValue.Specification ||
                        existingClosedSpecificationValue.Value != closedSpecificationValue.Value)) ||
                openSpecificationValues.Any(openSpecificationValue =>
                    existingOpenSpecifications.All(existingOpenSpecification =>
                        existingOpenSpecification.Name != openSpecificationValue.Specification)) ||
                postedImages.Any(image => !ValidImage(image.File)))
                return NewStatusCode(400);

            var existingImages = existingProduct.ProductVersions.MaxBy(version => version.VersionNumber)
                ?.ProductImages.Where(productImage =>
                    postedImages.Any(image => image.Name == productImage.Name && image.File == null)
                ) ?? new List<ProductImage>();

            var missingImages = postedImages.Where(image =>
                image.File == null &&
                existingImages.All(existingImage => existingImage.Name != image.Name)
            ).Select(image => image.Name).ToArray();
            if (missingImages.Length > 0)
                return base.BadRequest(new
                {
                    Missing = missingImages
                });

            var emptyFiles = postedImages.Where(image => image.File == null).ToList();
            var copyImages = existingProduct.ProductVersions.SelectMany(version =>
                    version.ProductImages.Select(image => new
                    {
                        image.Name,
                        image.File
                    }))
                .Where(image => emptyFiles.Exists(empty => empty.Name == image.Name))
                .Select(image => new ProductImage
                {
                    File = image.File,
                    Name = image.Name
                })
                .ToList();

            var newImages = postedImages.Where(image => image.File != null)
                .Select(image =>
                {
                    var randomName = Path.ChangeExtension(Path.GetRandomFileName(), "jpg");
                    using (var stream = System.IO.File.Create(Path.Combine(_path, randomName)))
                    {
                        image.File.CopyTo(stream);
                    }

                    return new ProductImage
                    {
                        File = Path.Combine(_base, randomName),
                        Name = image.Name
                    };
                }).ToList();

            var images = copyImages.Concat(newImages).ToList();
            var versionNumber = existingProduct.ProductVersions.Max(version => version.VersionNumber) + 1;
            existingProduct.ProductVersions.Add(new ProductVersion
            {
                VersionNumber = versionNumber,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ProductImages = images,
                ClosedSpecificationValues = existingClosedSpecificationValues.Where(existingClosedSpecificationValue =>
                    existingClosedSpecificationValue.MainCategoryName == existingProduct.MainCategoryName &&
                    existingClosedSpecificationValue.CategoryName == existingProduct.CategoryName &&
                    existingClosedSpecificationValue.SubcategoryName == existingProduct.SubcategoryName &&
                    closedSpecificationValues.Any(closedSpecification =>
                        existingClosedSpecificationValue.SpecificationName == closedSpecification.Specification &&
                        existingClosedSpecificationValue.Value == closedSpecification.Value)).ToList(),
                OpenSpecificationValues = openSpecificationValues.Select(openSpecificationValue =>
                    new Order.API.Context.OpenSpecificationValue
                    {
                        MainCategoryName = existingProduct.MainCategoryName,
                        CategoryName = existingProduct.CategoryName,
                        SubcategoryName = existingProduct.SubcategoryName,
                        Value = openSpecificationValue.Value,
                        SpecificationName = openSpecificationValue.Specification
                    }).ToList()
            });
            return Save();
        }

        /// <summary>
        /// Update a product.
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
        /// <param name="product">
        /// The product information.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the creation is successful,
        /// an <see cref="BadRequestResult"/> if there is an image missing or the open specification value or open specification does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.ProductManageClaim)]
        [HttpPost("employee/{mainCategory}/{category}/{subcategory}")]
        public IActionResult CreateProduct([FromRoute] string mainCategory, [FromRoute] string category, [FromRoute] string subcategory, [FromForm] NewProduct product)
        {
            if (_orderContext.MainCategories.All(currentMainCategory =>
                    currentMainCategory.Name != mainCategory ||
                    currentMainCategory.Categories.All(currentCategory =>
                        currentCategory.Name != category ||
                        currentCategory.Subcategories.All(currentSubcategory =>
                            currentSubcategory.Name != subcategory))))
                return NotFound();

            var postedImages = product.Images ?? Array.Empty<NewImage>();
            var closedSpecificationValues =
                product.ClosedSpecificationValues ?? Array.Empty<ClosedSpecificationValue>();
            var openSpecificationValues =
                product.OpenSpecificationValues ?? Array.Empty<OpenSpecificationValue>();
            var existingClosedSpecificationValues = _orderContext.ClosedSpecificationValues
                .Where(existingClosedSpecificationValue =>
                    existingClosedSpecificationValue.MainCategoryName == mainCategory &&
                    existingClosedSpecificationValue.CategoryName == category &&
                    existingClosedSpecificationValue.SubcategoryName == subcategory
                ).ToList();
            var existingOpenSpecifications = _orderContext.OpenSpecifications
                .Where(existingOpenSpecificationValue =>
                    existingOpenSpecificationValue.MainCategoryName == mainCategory &&
                    existingOpenSpecificationValue.CategoryName == category &&
                    existingOpenSpecificationValue.SubcategoryName == subcategory
                ).ToList();
            if (closedSpecificationValues.Any(closedSpecificationValue =>
                    existingClosedSpecificationValues.All(existingClosedSpecificationValue =>
                        existingClosedSpecificationValue.SpecificationName != closedSpecificationValue.Specification ||
                        existingClosedSpecificationValue.Value != closedSpecificationValue.Value)) ||
                openSpecificationValues.Any(openSpecificationValue =>
                    existingOpenSpecifications.All(existingOpenSpecification =>
                        existingOpenSpecification.Name != openSpecificationValue.Specification)) ||
                postedImages.Any(image => !ValidImage(image.File)))
                return BadRequest();

            var newProduct = new Product
            {
                MainCategoryName = mainCategory,
                CategoryName = category,
                SubcategoryName = subcategory,
                ProductVersions = new List<ProductVersion>
                {
                    new()
                    {
                        VersionNumber = 1,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        ProductImages = postedImages.Select(image =>
                        {
                            var randomName = Path.ChangeExtension(Path.GetRandomFileName(), "jpg");
                            using (var stream = System.IO.File.Create(Path.Combine(_path, randomName)))
                            {
                                image.File.CopyTo(stream);
                            }

                            return new ProductImage
                            {
                                File = Path.Combine(_base, randomName),
                                Name = image.Name
                            };
                        }).ToList(),
                        ClosedSpecificationValues = existingClosedSpecificationValues.Where(existingClosedSpecificationValue =>
                            existingClosedSpecificationValue.MainCategoryName == mainCategory &&
                            existingClosedSpecificationValue.CategoryName == category &&
                            existingClosedSpecificationValue.SubcategoryName == subcategory &&
                            closedSpecificationValues.Any(closedSpecification =>
                                existingClosedSpecificationValue.SpecificationName == closedSpecification.Specification &&
                                existingClosedSpecificationValue.Value == closedSpecification.Value)).ToList(),
                        OpenSpecificationValues = openSpecificationValues.Select(openSpecificationValue =>
                            new Order.API.Context.OpenSpecificationValue
                            {
                                MainCategoryName = mainCategory,
                                CategoryName = category,
                                SubcategoryName = subcategory,
                                Value = openSpecificationValue.Value,
                                SpecificationName = openSpecificationValue.Specification
                            }).ToList()
                    }
                }
            };
            _orderContext.Products.Add(newProduct);
            return Save();
        }

        /// <summary>
        /// Mark a product as deleted.
        /// </summary>
        /// <param name="id">
        /// The id of the product.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the update is successful,
        /// an <see cref="NotFoundResult"/> if the product does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.ProductManageClaim)]
        [HttpDelete("employee/{id:long}")]
        public IActionResult DeleteProduct([FromRoute] long id)
        {
            var product = _orderContext.Products.FirstOrDefault(product => product.Id == id);
            if (product == null)
                return NotFound();

            product.Deleted = true;
            return Save();
        }

        /// <summary>
        /// Mark a product as not deleted.
        /// </summary>
        /// <param name="id">
        /// The id of the product.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the update is successful,
        /// an <see cref="NotFoundResult"/> if the product does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.ProductManageClaim)]
        [HttpPost("employee/{id:long}")]
        public IActionResult RestoreProduct([FromRoute] long id)
        {
            var product = _orderContext.Products.FirstOrDefault(product => product.Id == id);
            if (product == null)
                return NotFound();

            product.Deleted = false;
            return Save();
        }

        /// <summary>
        /// Check if an image is a valid jpg.
        /// </summary>
        /// <param name="file">
        /// The file to check.
        /// </param>
        /// <returns>
        /// If an image is a valid jpg.
        /// </returns>
        private bool ValidImage(IFormFile? file)
        {
            if (file == null)
                return true;

            var signatures = new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
            };

            using (var reader = new BinaryReader(file.OpenReadStream()))
            {
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
            }
        }
    }
}