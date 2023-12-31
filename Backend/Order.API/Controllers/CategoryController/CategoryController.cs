using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly OrderContext _orderContext;

        /// <summary>
        /// Initialize a new <see cref="CategoryController"/> with the required information.
        /// </summary>
        /// <param name="orderContext">
        /// The <see cref="OrderContext"/> used to handle database access.
        /// </param>
        public CategoryController(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        /// <summary>
        /// Get the categories stored in the database.
        /// </summary>
        /// <returns>
        /// An <see cref="OkObjectResult"/> with the stored categories.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpGet("employee")]
        public IActionResult GetSubCategoryEmployee()
        {
            var data = _orderContext.MainCategories
                .Include(mainCategory => mainCategory.Categories)
                .ThenInclude(category => category.Subcategories)
                .AsSplitQuery()
                .ToList()
                .Select(mainCategory => new Models.Send.MainCategory(mainCategory))
                .OrderBy(mainCategory => mainCategory.Name)
                .ToArray();
            return Ok(data);
        }

        /// <summary>
        /// Get the information of a single subcategory.
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
        /// An <see cref="OkObjectResult"/> with the associated information,
        /// or an <see cref="NotFoundResult"/> if the subcategory does not exist.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpGet("employee/{mainCategory}/{category}/{subcategory}")]
        public IActionResult GetSubcategoryEmployee([FromRoute] string mainCategory, [FromRoute] string category, [FromRoute] string subcategory)
        {
            var data = _orderContext.Subcategories
                .Include(subcategory => subcategory.ClosedSpecifications)
                .ThenInclude(closedSpecification => closedSpecification.Filter)
                .Include(subcategory => subcategory.ClosedSpecifications)
                .ThenInclude(closedSpecification => closedSpecification.ClosedSpecificationValues)
                .Include(subcategory => subcategory.OpenSpecifications)
                .AsSplitQuery()
                .FirstOrDefault(currentSubcategory => currentSubcategory.Name == subcategory &&
                                                      currentSubcategory.CategoryName == category &&
                                                      currentSubcategory.MainCategoryName == mainCategory);

            if (data == null)
                return NotFound();

            return Ok(new Models.WholeSubcategory(data));
        }

        /// <summary>
        /// Get the categories stored in the database.
        /// </summary>
        /// <returns>
        /// An <see cref="OkObjectResult"/> with the stored categories.
        /// </returns>
        [EnableCors(Cors.AllowFrontend)]
        [AllowAnonymous]
        [HttpGet("enduser")]
        public IActionResult GetCategories()
        {
            var data = _orderContext.MainCategories
                .Include(mainCategory => mainCategory.Categories)
                .ThenInclude(category => category.Subcategories)
                .AsSplitQuery()
                .Where(mainCategory => !mainCategory.Deleted)
                .ToList()
                .Select(mainCategory => new Models.Send.MainCategory(mainCategory))
                .OrderBy(mainCategory => mainCategory.Name)
                .Select(mainCategory =>
                {
                    mainCategory.Categories = mainCategory.Categories
                        .Where(category => !category.Deleted)
                        .Select(category =>
                        {
                            category.Subcategories = category.Subcategories
                                .Where(subcategory => !subcategory.Deleted)
                                .ToArray();
                            return category;
                        }).ToArray();
                    return mainCategory;
                })
                .ToArray();
            return Ok(data);
        }

        /// <summary>
        /// Get the information of a single subcategory.
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
        /// An <see cref="OkObjectResult"/> with the associated information,
        /// or an <see cref="NotFoundResult"/> if the subcategory does not exist.
        /// </returns>
        [EnableCors(Cors.AllowFrontend)]
        [AllowAnonymous]
        [HttpGet("enduser/{mainCategory}/{category}/{subcategory}")]
        public IActionResult GetSubCategory([FromRoute] string mainCategory, [FromRoute] string category, [FromRoute] string subcategory)
        {
            var data = _orderContext.Subcategories
                .Include(subcategory => subcategory.ClosedSpecifications)
                .ThenInclude(closedSpecification => closedSpecification.Filter)
                .Include(subcategory => subcategory.ClosedSpecifications)
                .ThenInclude(closedSpecification => closedSpecification.ClosedSpecificationValues)
                .Include(subcategory => subcategory.OpenSpecifications)
                .AsSplitQuery()
                .FirstOrDefault(currentSubcategory => currentSubcategory.Name == subcategory &&
                                                      currentSubcategory.CategoryName == category &&
                                                      currentSubcategory.MainCategoryName == mainCategory &&
                                                      !currentSubcategory.Deleted);

            if (data == null)
                return NotFound();

            var wholeSubcategory = new Models.WholeSubcategory(data);
            wholeSubcategory.ClosedSpecifications = wholeSubcategory.ClosedSpecifications
                .Where(specification => !specification.Deleted)
                .Select(specification =>
                {
                    specification.Values = specification.Values.Where(value => !value.Deleted).ToArray();
                    return specification;
                }).ToArray();
            wholeSubcategory.OpenSpecifications = wholeSubcategory.OpenSpecifications
                .Where(specification => !specification.Deleted).ToArray();
            return Ok(wholeSubcategory);
        }

        /// <summary>
        /// Create a main category, if a main category with the same name was previously deleted mark it as not deleted.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the main category to create.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the main category is saved,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpPost("{mainCategory}")]
        public IActionResult AddMainCategory([FromRoute] string mainCategory)
        {
            var currentMainCategory = _orderContext.MainCategories
                .Include(mainCategory => mainCategory.Categories)
                .ThenInclude(subcategory => subcategory.Subcategories)
                .FirstOrDefault(currentMainCategory => currentMainCategory.Name == mainCategory);
            if (currentMainCategory == null)
            {
                _orderContext.MainCategories.Add(new MainCategory
                {
                    Name = mainCategory
                });
                return Save();
            }

            if (!currentMainCategory.Deleted)
                return BadRequest();
            currentMainCategory.Deleted = false;
            return Save();
        }

        /// <summary>
        /// Create a category, if a category with the same name was previously deleted mark it as not deleted.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the associated main category.
        /// </param>
        /// <param name="category">
        /// The name of the category to create.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the category is saved,
        /// an <see cref="NotFoundResult"/> if the main category does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpPost("{mainCategory}/{category}")]
        public IActionResult AddCategory([FromRoute] string mainCategory, [FromRoute] string category)
        {
            if (!_orderContext.MainCategories.Any(category => category.Name == mainCategory))
                return NotFound();

            var currentCategory = _orderContext.Categories
                .Include(category => category.Subcategories)
                .FirstOrDefault(currentCategory => currentCategory.MainCategoryName == mainCategory && currentCategory.Name == category);
            if (currentCategory == null)
            {
                _orderContext.Categories.Add(new Category
                {
                    MainCategoryName = mainCategory,
                    Name = category
                });
                return Save();
            }

            if (!currentCategory.Deleted)
                return BadRequest();
            currentCategory.Deleted = false;
            return Save();
        }

        /// <summary>
        /// Create a sub category, if a sub category with the same name was previously deleted mark it as not deleted.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the associated main category.
        /// </param>
        /// <param name="category">
        /// The name of the associated category.
        /// </param>
        /// <param name="subcategory">
        /// The name of the subcategory to create.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the subcategory is saved,
        /// an <see cref="NotFoundResult"/> if the main category or category does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpPost("{mainCategory}/{category}/{subcategory}")]
        public IActionResult AddSubcategory([FromRoute] string mainCategory, [FromRoute] string category, [FromRoute] string subcategory)
        {
            if (!_orderContext.MainCategories.Any(category => category.Name == mainCategory))
                return NotFound();
            if (!_orderContext.Categories.Any(tempCategory => tempCategory.Name == category))
                return NotFound();
            var currentSubcategory = _orderContext.Subcategories
                .FirstOrDefault(currentSubcategory => currentSubcategory.Name == subcategory &&
                                                      currentSubcategory.CategoryName == category &&
                                                      currentSubcategory.MainCategoryName == mainCategory);
            if (currentSubcategory == null)
            {
                _orderContext.Subcategories.Add(new Subcategory
                {
                    MainCategoryName = mainCategory,
                    CategoryName = category,
                    Name = subcategory
                });
                return Save();
            }

            if (!currentSubcategory.Deleted)
                return BadRequest();
            currentSubcategory.Deleted = false;
            return Save();
        }

        /// <summary>
        /// Update a sub category.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the associated main category.
        /// </param>
        /// <param name="category">
        /// The name of the associated category.
        /// </param>
        /// <param name="subcategory">
        /// The name of the associated subcategory.
        /// </param>
        /// <param name="wholeSubcategory">
        /// The new information of the subcategory.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the subcategory was updated,
        /// an <see cref="BadRequestResult"/> if the name in <see cref="wholeSubcategory"/> and <see cref="subcategory"/> are different,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the saving fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpPost("{mainCategory}/{category}/{subcategory}/update")]
        public IActionResult UpdateSubcategory([FromRoute] string mainCategory, [FromRoute] string category, [FromRoute] string subcategory, [FromBody] Models.WholeSubcategory wholeSubcategory)
        {
            if (subcategory != wholeSubcategory.Name)
                return BadRequest();
            var currentSubcategory = _orderContext.Subcategories
                .Include(subcategory => subcategory.OpenSpecifications)
                .Include(subcategory => subcategory.ClosedSpecifications)
                .ThenInclude(closedSpecification => closedSpecification.ClosedSpecificationValues)
                .Include(subcategory => subcategory.ClosedSpecifications)
                .ThenInclude(closedSpecification => closedSpecification.Filter)
                .FirstOrDefault(currentSubcategory => currentSubcategory.Name == subcategory &&
                                                      currentSubcategory.CategoryName == category &&
                                                      currentSubcategory.MainCategoryName == mainCategory);
            if (currentSubcategory == null)
                return NotFound();
            currentSubcategory.Deleted = false;
            var openSpecifications = currentSubcategory.OpenSpecifications.Select(specification => specification.Name).ToList();
            currentSubcategory.OpenSpecifications = currentSubcategory.OpenSpecifications.Select(specification =>
            {
                var currentSpecification = wholeSubcategory.OpenSpecifications.FirstOrDefault(updatedSpecification =>
                    updatedSpecification.Name == specification.Name);
                specification.Deleted = currentSpecification == null || currentSpecification.Deleted;
                return specification;
            }).Concat(
                wholeSubcategory.OpenSpecifications.Where(updatedSpecification =>
                    !openSpecifications.Contains(updatedSpecification.Name) && !updatedSpecification.Deleted
                ).Select(updatedSpecification => new OpenSpecification
                {
                    Name = updatedSpecification.Name,
                    CategoryName = category,
                    SubcategoryName = subcategory,
                    MainCategoryName = mainCategory
                })
            ).ToList();
            var closedSpecifications = currentSubcategory.ClosedSpecifications.Select(specification => specification.Name).ToList();
            currentSubcategory.ClosedSpecifications = currentSubcategory.ClosedSpecifications.Select(specification =>
            {
                var currentSpecification = wholeSubcategory.ClosedSpecifications.FirstOrDefault(currentSpecification =>
                    currentSpecification.Name == specification.Name && !currentSpecification.Deleted);
                if (currentSpecification == null)
                {
                    specification.Deleted = true;
                    return specification;
                }
                specification.Deleted = currentSpecification.Deleted;
                var values = specification.ClosedSpecificationValues.Select(specificationValue => specificationValue.Value).ToList();
                specification.ClosedSpecificationValues = specification.ClosedSpecificationValues.Select(
                    specificationValue =>
                    {
                        var currentValue = currentSpecification.Values.FirstOrDefault(currentSpecificationValue =>
                            currentSpecificationValue.Value == specificationValue.Value);
                        specificationValue.Deleted = currentValue == null || currentValue.Deleted;
                        return specificationValue;
                    }).Concat(
                        currentSpecification.Values
                            .Where(currentSpecificationValue => !values.Contains(currentSpecificationValue.Value) &&
                                                                !currentSpecificationValue.Deleted)
                            .Select(currentSpecificationValue => new ClosedSpecificationValue
                            {
                                Value = currentSpecificationValue.Value,
                                SpecificationName = specification.Name,
                                CategoryName = specification.CategoryName,
                                SubcategoryName = specification.SubcategoryName,
                                MainCategoryName = mainCategory
                            }).ToList()
                    ).ToList();
                if (currentSpecification.Filter == null && specification.Filter != null)
                {
                    _orderContext.Remove(specification.Filter);
                }
                else if (currentSpecification.Filter != null)
                {
                    if (specification.Filter == null)
                    {
                        _orderContext.Filters.Add(new Filter
                        {
                            Title = currentSpecification.Filter,
                            ClosedSpecificationName = specification.Name,
                            CategoryName = category,
                            SubcategoryName = subcategory,
                            MainCategoryName = mainCategory
                        });
                    }
                }
                return specification;
            }).Concat(
                wholeSubcategory.ClosedSpecifications.Where(
                    specification => !closedSpecifications.Contains(specification.Name) && !specification.Deleted
                ).Select(specification =>
                {
                    var newSpecification = new ClosedSpecification
                    {
                        Name = specification.Name,
                        SubcategoryName = subcategory,
                        CategoryName = category,
                        MainCategoryName = mainCategory,
                        ClosedSpecificationValues = specification.Values.Where(value => !value.Deleted).Select(value => new ClosedSpecificationValue
                        {
                            Value = value.Value,
                            SpecificationName = specification.Name,
                            SubcategoryName = subcategory,
                            CategoryName = category,
                            MainCategoryName = mainCategory
                        }).ToList()
                    };
                    if (specification.Filter != null)
                    {
                        newSpecification.Filter = new Filter
                        {
                            Title = specification.Filter,
                            ClosedSpecificationName = specification.Name,
                            CategoryName = category,
                            SubcategoryName = subcategory,
                            MainCategoryName = mainCategory
                        };
                    }

                    return newSpecification;
                })
            ).ToList();
            return Save();
        }

        /// <summary>
        /// Mark a main category as deleted.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the main category to delete.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the main category is deleted,
        /// an <see cref="NotFoundResult"/> if the main category does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the deletion fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpDelete("{mainCategory}")]
        public IActionResult DeleteMainCategory([FromRoute] string mainCategory)
        {
            var currentMainCategory = _orderContext.MainCategories
                .Include(mainCategory => mainCategory.Categories)
                .ThenInclude(category => category.Subcategories)
                .ThenInclude(subcategory => subcategory.OpenSpecifications)
                .Include(mainCategory => mainCategory.Categories)
                .ThenInclude(category => category.Subcategories)
                .ThenInclude(subcategory => subcategory.ClosedSpecifications)
                .ThenInclude(subcategory => subcategory.ClosedSpecificationValues)
                .AsSplitQuery()
                .FirstOrDefault(currentMainCategory => currentMainCategory.Name == mainCategory);
            if (currentMainCategory == null)
                return NotFound();
            currentMainCategory.Deleted = true;
            currentMainCategory.Categories = currentMainCategory.Categories.Select(MarkCategoryDeleted).ToList();
            return Save();
        }

        /// <summary>
        /// Mark a category as deleted.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the associated main category.
        /// </param>
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
        [HttpDelete("{mainCategory}/{category}")]
        public IActionResult DeleteCategory([FromRoute] string mainCategory, [FromRoute] string category)
        {
            var currentCategory = _orderContext.Categories
                .Include(category => category.Subcategories)
                .ThenInclude(subcategory => subcategory.OpenSpecifications)
                .Include(category => category.Subcategories)
                .ThenInclude(subcategory => subcategory.ClosedSpecifications)
                .ThenInclude(subcategory => subcategory.ClosedSpecificationValues)
                .AsSplitQuery()
                .FirstOrDefault(currentCategory => currentCategory.MainCategoryName == mainCategory && currentCategory.Name == category);
            if (currentCategory == null)
                return NotFound();
            MarkCategoryDeleted(currentCategory);
            return Save();
        }

        /// <summary>
        /// Mark a subcategory as deleted.
        /// </summary>
        /// <param name="mainCategory">
        /// The name of the associated main category.
        /// </param>
        /// <param name="category">
        /// The name of the associated category.
        /// </param>
        /// <param name="subcategory">
        /// The name of the subcategory to delete.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the subcategory is deleted,
        /// an <see cref="NotFoundResult"/> if the subcategory does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the deletion fails.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.CategoryManageClaim)]
        [HttpDelete("{mainCategory}/{category}/{subcategory}")]
        public IActionResult DeleteSubcategory([FromRoute] string mainCategory, [FromRoute] string category, [FromRoute] string subcategory)
        {
            var currentSubcategory = _orderContext.Subcategories
                .Include(subcategory => subcategory.OpenSpecifications)
                .Include(subcategory => subcategory.ClosedSpecifications)
                .ThenInclude(subcategory => subcategory.ClosedSpecificationValues)
                .AsSplitQuery()
                .FirstOrDefault(currentSubcategory => currentSubcategory.Name == subcategory &&
                                                      currentSubcategory.CategoryName == category &&
                                                      currentSubcategory.MainCategoryName == mainCategory);
            if (currentSubcategory == null)
                return NotFound();
            MarkSubcategoryDeleted(currentSubcategory);
            return Save();
        }

        /// <summary>
        /// Save the database changes, and return an appropriate <see cref="IActionResult"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="OkResult"/> if the save was successful,
        /// or an <see cref="ObjectResult"/> with a 500 status code if the save failed due to an exception.
        /// </returns>
        private IActionResult Save()
        {
            try
            {
                _orderContext.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return NewStatusCode(500);
            }
        }

        /// <summary>
        /// Mark a category and its subcategories as deleted.
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
            category.Subcategories = category.Subcategories.Select(MarkSubcategoryDeleted).ToList();
            return category;
        }

        /// <summary>
        /// Mark a subcategory, its specification, and filters as deleted.
        /// </summary>
        /// <param name="subcategory">
        /// The subcategory to delete.
        /// </param>
        /// <returns>
        /// The new subcategory.
        /// </returns>
        private Subcategory MarkSubcategoryDeleted(Subcategory subcategory)
        {
            subcategory.Deleted = true;
            subcategory.OpenSpecifications = subcategory.OpenSpecifications.Select(specification =>
            {
                specification.Deleted = true;
                return specification;
            }).ToList();
            subcategory.ClosedSpecifications = subcategory.ClosedSpecifications.Select(specification =>
            {
                specification.Deleted = true;
                specification.ClosedSpecificationValues = specification.ClosedSpecificationValues.Select(specificationValue =>
                {
                    specificationValue.Deleted = true;
                    return specificationValue;
                }).ToList();
                return specification;
            }).ToList();
            return subcategory;
        }
    }
}