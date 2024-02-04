namespace Order.API.Controllers.CategoryController.Models
{
    /// <summary>
    /// All the information for a subcategory.
    /// </summary>
    public class WholeSubcategory
    {
        /// <summary>
        /// If the subcategory is deleted.
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// The name of the subcategory.
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
        /// Initialize a new <see cref="WholeSubcategory"/>.
        /// </summary>
        public WholeSubcategory() {}

        /// <summary>
        /// Initialize a new <see cref="WholeSubcategory"/> based on a <see cref="Context.Subcategory"/>.
        /// </summary>
        /// <param name="subcategory">
        /// The <see cref="Context.Subcategory"/> to use.
        /// </param>
        public WholeSubcategory(Context.Subcategory subcategory)
        {
            Deleted = subcategory.Deleted;
            Name = subcategory.Name;
            ClosedSpecifications = subcategory.ClosedSpecifications
                .Select(closedSpecification => new ClosedSpecification(closedSpecification))
                .OrderBy(closedSpecification => closedSpecification.Name).ToArray();
            OpenSpecifications = subcategory.OpenSpecifications
                .Select(openSpecification => new OpenSpecification(openSpecification))
                .OrderBy(openSpecification => openSpecification.Name)
                .ToArray();
        }
    }
}