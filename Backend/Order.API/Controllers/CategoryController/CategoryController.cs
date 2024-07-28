using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Order.API.Context;
using Order.API.Util;

namespace Order.API.Controllers.CategoryController
{
    /// <summary>
    /// The controller to handle endpoints associated with categories.
    /// </summary>
    [Route("category")]
    [ApiController]
    public class CategoryController : BaseController
    {
        /// <summary>
        /// Initialize a new <see cref="CategoryController"/> with the required information.
        /// </summary>
        /// <param name="orderContext">
        /// The <see cref="OrderContext"/> used to handle database access.
        /// </param>
        public CategoryController(OrderContext orderContext) : base(orderContext){}

        /// <summary>
        /// Get the information of all categories.
        /// </summary>
        /// <returns>
        /// An <see cref="OkObjectResult"/> with the associated information,
        /// or an <see cref="NotFoundResult"/> if the category does not exist.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpGet("employee")]
        public IActionResult GetCategoriesEmployee()
        {
            var data = _orderContext.Categories
                .Include(category => category.OpenSpecifications)
                .Include(category => category.ClosedSpecifications)
                .ThenInclude(closedSpecifications => closedSpecifications.ClosedSpecificationValues)
                .Include(category => category.ClosedSpecifications)
                .ThenInclude(closedSpecifications => closedSpecifications.Filter)
                .ToList();
            return Ok(data.Select(category => new Models.WholeCategory(category)).ToArray());
        }

        /// <summary>
        /// Get the information of all categories.
        /// </summary>
        /// <returns>
        /// An <see cref="OkObjectResult"/> with the associated information,
        /// or an <see cref="NotFoundResult"/> if the category does not exist.
        /// </returns>
        [EnableCors(Cors.AllowFrontend)]
        [AllowAnonymous]
        [HttpGet("enduser")]
        public IActionResult GetCategories()
        {
            var data = _orderContext.Categories
                .Include(category => category.OpenSpecifications)
                .Include(category => category.ClosedSpecifications)
                .ThenInclude(closedSpecifications => closedSpecifications.ClosedSpecificationValues)
                .Include(category => category.ClosedSpecifications)
                .ThenInclude(closedSpecifications => closedSpecifications.Filter)
                .Where(currentCategory => !currentCategory.Deleted)
                .ToList();

            var categories = data.Select(category =>
            {
                var wholeCategory = new Models.WholeCategory(category);
                wholeCategory.ClosedSpecifications = wholeCategory.ClosedSpecifications
                    .Where(specification => !specification.Deleted)
                    .Select(specification =>
                    {
                        specification.Values = specification.Values.Where(value => !value.Deleted).ToArray();
                        return specification;
                    }).ToArray();
                wholeCategory.OpenSpecifications = wholeCategory.OpenSpecifications
                    .Where(specification => !specification.Deleted).ToArray();
                return wholeCategory;
            }).ToArray();
            return Ok(categories);
        }

        /// <summary>
        /// Create a category, if a category with the same name was previously deleted mark it as not deleted.
        /// </summary>
        /// <param name="category">
        /// The name of the category to create.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the category is saved,
        /// an <see cref="NotFoundResult"/> if the category does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpPost("employee/{category}")]
        public IActionResult AddCategory([FromRoute] string category)
        {
            var currentCategory = _orderContext.Categories
                .FirstOrDefault(currentCategory => currentCategory.Name == category);
            if (currentCategory == null)
            {
                _orderContext.Categories.Add(new Category
                {
                    Name = category
                });
                return Save();
            }

            if (!currentCategory.Deleted)
                return BadRequest();

            return Save();
        }

        /// <summary>
        /// Update the name of a category.
        /// </summary>
        /// <param name="oldName">
        /// The old name of the category.
        /// </param>
        /// <param name="newName">
        /// The new name of the category.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the category is saved,
        /// an <see cref="NotFoundResult"/> if the category does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpPost("employee/{oldName}/update/{newName}")]
        public IActionResult UpdateCategory([FromRoute] string oldName, [FromRoute] string newName)
        {
            
            var currentCategory = _orderContext.Categories
                .FirstOrDefault(currentCategory => currentCategory.Name == oldName);
            if (currentCategory == null)
                return NotFound();

            currentCategory.Name = newName;
            return Save();
        }


        /// <summary>
        /// Add or overwrite an closed specification. If it exists, it will be either deleted if the deleted flag is set,
        /// or it will no longer be marked as deleted. In the former case any updates to the values will be ignore, and
        /// in the latter case only those values will be be used that are present in the given data. If the specification
        /// does not exist, it will be created.
        /// </summary>
        /// <param name="category">
        /// The name of the category to update.
        /// </param>
        /// <param name="specification">
        /// The specification data to use.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the category is saved,
        /// an <see cref="NotFoundResult"/> if the category does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpPost("employee/{category}/closed")]
        public IActionResult AddOverwriteClosedSpecification([FromRoute] string category, [FromBody] Models.ClosedSpecification specification)
        {
            var currentCategory = _orderContext.Categories
                .Include(category => category.ClosedSpecifications)
                .ThenInclude(closedSpecification => closedSpecification.ClosedSpecificationValues)
                .FirstOrDefault(currentCategory => currentCategory.Name == category);
            if (currentCategory == null)
                return NotFound();

            var currentSpecification = currentCategory.ClosedSpecifications
                .FirstOrDefault(currentSpecification => currentSpecification.Name == specification.Name);
            if (currentSpecification != null)
            {
                if (specification.Deleted)
                {
                    currentSpecification.Deleted = true;
                    currentSpecification.ClosedSpecificationValues = currentSpecification.ClosedSpecificationValues
                        .Select(value =>
                        {
                            value.Deleted = true;
                            return value;
                        }).ToList();
                    return Save();
                }
                currentSpecification.Deleted = false;
                currentSpecification.ClosedSpecificationValues = currentSpecification.ClosedSpecificationValues
                    .Select(value =>
                    {
                        var newSpecification = specification.Values
                            .FirstOrDefault(newSpecification => newSpecification.Value == value.Value);
                        value.Deleted = newSpecification == null || newSpecification.Deleted;
                        return value;
                    }).ToList();
                currentSpecification.ClosedSpecificationValues.AddRange(
                    specification.Values.Where(newValue =>
                        !currentSpecification.ClosedSpecificationValues.Select(oldValue => oldValue.Value).Contains(newValue.Value)
                    ).Select(value => new ClosedSpecificationValue
                    {
                        Value = value.Value,
                        Deleted = false
                    })
                );
                return Save();
            }

            currentCategory.ClosedSpecifications.Add(new ClosedSpecification
            {
                Name = specification.Name,
                Filter = specification.Filter != null ? new Filter{Title = specification.Filter} : null,
                Deleted = false,
                ClosedSpecificationValues = specification.Values
                    .Where(value => !value.Deleted)
                    .Select(value => new ClosedSpecificationValue
                    {
                        Value = value.Value,
                        Deleted = false
                    }).ToList()
            });
            return Save();
        }

        /// <summary>
        /// Update the name of an closed specification.
        /// </summary>
        /// <param name="category">
        /// The name of the associated category.
        /// </param>
        /// <param name="oldName">
        /// The old name of the closed specification.
        /// </param>
        /// <param name="newName">
        /// The new name of the closed specification.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the closed specification is renamed,
        /// an <see cref="NotFoundResult"/> if the category does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpPost("employee/{category}/closed/update/{oldName}/{newName}")]
        public IActionResult UpdateClosedSpecificationName([FromRoute] string category, [FromRoute] string oldName, [FromRoute] string newName)
        {
            var closedSpecification = _orderContext.ClosedSpecifications
                .FirstOrDefault(specification => specification.Category.Name == category && specification.Name == oldName);
            if (closedSpecification == null)
                return NotFound();

            closedSpecification.Name = newName;
            return Save();
        }

        /// <summary>
        /// Update the name of an closed specification value.
        /// </summary>
        /// <param name="category">
        /// The name of the associated category.
        /// </param>
        /// <param name="specification">
        /// The name of the associated specification.
        /// </param>
        /// <param name="oldName">
        /// The old name of the closed specification value.
        /// </param>
        /// <param name="newName">
        /// The new name of the closed specification value.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the closed specification value is renamed,
        /// an <see cref="NotFoundResult"/> if the category does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpPost("employee/{category}/closed/value/{specification}/{oldName}/{newName}")]
        public IActionResult UpdateClosedSpecificationValueName(
            [FromRoute] string category,
            [FromRoute] string specification,
            [FromRoute] string oldName,
            [FromRoute] string newName
        )
        {
            var closedSpecificationValue = _orderContext.ClosedSpecificationValues
                .FirstOrDefault(value =>
                    value.ClosedSpecification.Category.Name == category &&
                    value.ClosedSpecification.Name == specification &&
                    value.Value == oldName
                );
            if (closedSpecificationValue == null)
                return NotFound();

            closedSpecificationValue.Value = newName;
            return Save();
        }

        /// <summary>
        /// Add or overwrite an open specification. If it exists, it will be either deleted if the deleted flag is set,
        /// or it will no longer be marked as deleted. If the specification does not exist, it will be created.
        /// </summary>
        /// <param name="category">
        /// The name of the category to update.
        /// </param>
        /// <param name="specification">
        /// The specification data to use.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the category is saved,
        /// an <see cref="NotFoundResult"/> if the category does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpPost("employee/{category}/open")]
        public IActionResult AddOverwriteOpenSpecification([FromRoute] string category, [FromBody] Models.OpenSpecification specification)
        {
            var currentCategory = _orderContext.Categories
                .Include(category => category.OpenSpecifications)
                .FirstOrDefault(currentCategory => currentCategory.Name == category);
            if (currentCategory == null)
                return NotFound();

            var currentSpecification = currentCategory.OpenSpecifications
                .FirstOrDefault(currentSpecification => currentSpecification.Name == specification.Name);
            if (currentSpecification != null)
            {
                currentSpecification.Deleted = specification.Deleted;
                return Save();
            }

            currentCategory.OpenSpecifications.Add(new OpenSpecification
            {
                Name = specification.Name
            });
            return Save();
        }

        /// <summary>
        /// Update the name of an open specification.
        /// </summary>
        /// <param name="category">
        /// The name of the associated category.
        /// </param>
        /// <param name="oldName">
        /// The old name of the open specification.
        /// </param>
        /// <param name="newName">
        /// The new name of the open specification.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the open specification is renamed,
        /// an <see cref="NotFoundResult"/> if the category does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpPost("employee/{category}/open/update/{oldName}/{newName}")]
        public IActionResult UpdateOpenSpecificationName([FromRoute] string category, [FromRoute] string oldName, [FromRoute] string newName)
        {
            var openSpecification = _orderContext.OpenSpecifications
                .FirstOrDefault(specification => specification.Category.Name == category && specification.Name == oldName);
            if (openSpecification == null)
                return NotFound();

            openSpecification.Name = newName;
            return Save();
        }

        /// <summary>
        /// Mark a category as deleted.
        /// </summary>
        /// <param name="category">
        /// The name of the category to delete.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the category is deleted,
        /// an <see cref="NotFoundResult"/> if the category does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the deletion fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpDelete("employee/{category}")]
        public IActionResult DeleteCategory([FromRoute] string category)
        {
            var currentCategory = _orderContext.Categories
                .Include(category => category.OpenSpecifications)
                .Include(category => category.ClosedSpecifications)
                .ThenInclude(category => category.ClosedSpecificationValues)
                .AsSplitQuery()
                .FirstOrDefault(currentCategory => currentCategory.Name == category);
            if (currentCategory == null)
                return NotFound();
            MarkCategoryDeleted(currentCategory);
            return Save();
        }

        /// <summary>
        /// Restore a deleted category.
        /// </summary>
        /// <param name="category">
        /// The name of the category to delete.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the category is restored,
        /// an <see cref="NotFoundResult"/> if the category does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the deletion fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpPost("employee/{category}/restore")]
        public IActionResult RestoreCategory([FromRoute] string category)
        {
            var currentCategory = _orderContext.Categories
                .FirstOrDefault(currentCategory => currentCategory.Name == category);

            if (currentCategory == null)
                return NotFound();
            currentCategory.Deleted = false;
            return Save();
        }

        /// <summary>
        /// Mark a category, its specification, and filters as deleted.
        /// </summary>
        /// <param name="category">
        /// The category to delete.
        /// </param>
        /// <returns>
        /// The new category.
        /// </returns>
        private Category MarkCategoryDeleted(Category category)
        {
            category.Deleted = true;
            category.OpenSpecifications = category.OpenSpecifications.Select(specification =>
            {
                specification.Deleted = true;
                return specification;
            }).ToList();
            category.ClosedSpecifications = category.ClosedSpecifications.Select(specification =>
            {
                specification.Deleted = true;
                specification.ClosedSpecificationValues = specification.ClosedSpecificationValues.Select(specificationValue =>
                {
                    specificationValue.Deleted = true;
                    return specificationValue;
                }).ToList();
                return specification;
            }).ToList();
            return category;
        }
    }
}