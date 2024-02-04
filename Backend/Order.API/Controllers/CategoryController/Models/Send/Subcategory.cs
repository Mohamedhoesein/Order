namespace Order.API.Controllers.CategoryController.Models.Send
{
    /// <summary>
    /// A subcategory.
    /// </summary>
    public class Subcategory
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
        /// Initialize a new <see cref="Subcategory"/>.
        /// </summary>
        public Subcategory() {}

        /// <summary>
        /// Initialize a new <see cref="Subcategory"/> based on a <see cref="Context.Subcategory"/>.
        /// </summary>
        /// <param name="subcategory">
        /// The <see cref="Context.Subcategory"/> to use.
        /// </param>
        public Subcategory(Context.Subcategory subcategory)
        {
            Deleted = subcategory.Deleted;
            Name = subcategory.Name;
        }
    }
}