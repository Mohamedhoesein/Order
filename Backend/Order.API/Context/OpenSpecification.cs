using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// Specifications for a product which has an open set of possible values.
    /// </summary>
    public class OpenSpecification
    {
        /// <summary>
        /// The id of the specification.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of the specification.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// If the specification is deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; }

        /// <summary>
        /// The id of the category this specification is associated with.
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// The category this specification is associated with.
        /// </summary>
        public Category Category { get; set; }
        /// <summary>
        /// The values for products associated with this specification.
        /// </summary>
        public ICollection<OpenSpecificationValue> OpenSpecificationValues { get; set; }
    }
}