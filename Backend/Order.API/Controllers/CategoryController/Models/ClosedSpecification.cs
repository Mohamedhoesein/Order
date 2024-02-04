namespace Order.API.Controllers.CategoryController.Models
{
    /// <summary>
    /// A closed specification.
    /// </summary>
    public class ClosedSpecification
    {
        /// <summary>
        /// The name of the specification.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The values of the specification.
        /// </summary>
        public ClosedSpecificationValue[] Values { get; set; }
        /// <summary>
        /// The filter of the specification.
        /// </summary>
        public string? Filter { get; set; }
        /// <summary>
        /// If the specification is deleted.
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Initialize a new <see cref="ClosedSpecification"/>.
        /// </summary>
        public ClosedSpecification() {}

        /// <summary>
        /// Initialize a new <see cref="ClosedSpecification"/> based on a <see cref="Context.ClosedSpecification"/>.
        /// </summary>
        /// <param name="closedSpecification">
        /// The <see cref="Context.ClosedSpecification"/> to use.
        /// </param>
        public ClosedSpecification(Context.ClosedSpecification closedSpecification)
        {
            Name = closedSpecification.Name;
            Values = closedSpecification.ClosedSpecificationValues
                .Select(closedSpecificationValue => new ClosedSpecificationValue(closedSpecificationValue))
                .OrderBy(closedSpecificationValue => closedSpecificationValue.Value)
                .ToArray();
            Filter = closedSpecification.Filter?.Title;
            Deleted = closedSpecification.Deleted;
        }
    }
}