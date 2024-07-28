using Order.API.Context;

namespace Order.API.Controllers.CategoryController.Models
{
    /// <summary>
    /// All the information for a category.
    /// </summary>
    public class WholeCategory
    {
        /// <summary>
        /// If the category is deleted.
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The associated closed specifications.
        /// </summary>
        public ClosedSpecification[] ClosedSpecifications { get; set; }
        /// <summary>
        /// The associated open specifications.
        /// </summary>
        public OpenSpecification[] OpenSpecifications { get; set; }

        /// <summary>
        /// Initialize a new <see cref="WholeCategory"/>.
        /// </summary>
        public WholeCategory() {}

        /// <summary>
        /// Initialize a new <see cref="WholeCategory"/> based on a <see cref="Category"/>.
        /// </summary>
        /// <param name="category">
        /// The <see cref="Category"/> to use.
        /// </param>
        public WholeCategory(Context.Category category)
        {
            Deleted = category.Deleted;
            Name = category.Name;
            ClosedSpecifications = category.ClosedSpecifications
                .Select(closedSpecification => new ClosedSpecification(closedSpecification))
                .OrderBy(closedSpecification => closedSpecification.Name).ToArray();
            OpenSpecifications = category.OpenSpecifications
                .Select(openSpecification => new OpenSpecification(openSpecification))
                .OrderBy(openSpecification => openSpecification.Name)
                .ToArray();
        }
    }
}