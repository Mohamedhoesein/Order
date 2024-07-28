using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// A category for the products.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// The id of the category.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of the category.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// If the category is deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; }

        /// <summary>
        /// The open specifications associated with this category.
        /// </summary>
        public ICollection<OpenSpecification> OpenSpecifications { get; set; }
        /// <summary>
        /// The open specifications associated with this category.
        /// </summary>
        public ICollection<ClosedSpecification> ClosedSpecifications { get; set; }
        /// <summary>
        /// The filters associated with this category.
        /// </summary>
        public ICollection<Filter> Filters { get; set; }
        /// <summary>
        /// The products associated with this category.
        /// </summary>
        public ICollection<Product> Products { get; set; }
    }
}