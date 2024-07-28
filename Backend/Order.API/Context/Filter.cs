using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// A filter for products.
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// The id of the filter.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The title for the filter.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The id of the category this filter is associated with.
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// The id of the category this filter is associated with.
        /// </summary>
        public Category Category { get; set; }
        /// <summary>
        /// The id of the closed specification this filter is associated with.
        /// </summary>
        public int SpecificationId { get; set; }
        /// <summary>
        /// The closed specification this filter is associated with.
        /// </summary>
        public ClosedSpecification ClosedSpecification { get; set; }
    }
}