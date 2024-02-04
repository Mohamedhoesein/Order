namespace Order.API.Controllers.CategoryController.Models
{
    /// <summary>
    /// An open specification.
    /// </summary>
    public class OpenSpecification
    {
        /// <summary>
        /// The name of the specification.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// If the specification is deleted.
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Initialize a new <see cref="OpenSpecification"/>.
        /// </summary>
        public OpenSpecification() {}

        /// <summary>
        /// Initialize a new <see cref="OpenSpecification"/> based on a <see cref="Context.OpenSpecification"/>.
        /// </summary>
        /// <param name="openSpecification">
        /// The <see cref="Context.OpenSpecification"/> to use.
        /// </param>
        public OpenSpecification(Context.OpenSpecification openSpecification)
        {
            Name = openSpecification.Name;
            Deleted = openSpecification.Deleted;
        }
    }
}