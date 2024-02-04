namespace Order.API.Controllers.ProductController.Models.Send
{
    /// <summary>
    /// A product for an employee.
    /// </summary>
    public class EmployeeProduct
    {
        /// <summary>
        /// The id of the product.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// The name of the product.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The description of the product.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The price of the product in cents.
        /// </summary>
        public ulong Price { get; set; }
        /// <summary>
        /// If the product is deleted.
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// The associated closed specification values.
        /// </summary>
        public ClosedSpecificationValue[] ClosedSpecificationValues { get; set; }
        /// <summary>
        /// The associated open specification values.
        /// </summary>
        public OpenSpecificationValue[] OpenSpecificationValues { get; set; }
        /// <summary>
        /// The images of the product.
        /// </summary>
        public Image[] Images { get; set; }

        /// <summary>
        /// Initialize a new empty <see cref="EmployeeProduct"/>.
        /// </summary>
        public EmployeeProduct() {}

        /// <summary>
        /// Initialize a new <see cref="EmployeeProduct"/> based on a <see cref="Context.Product"/>.
        /// </summary>
        /// <param name="product">
        /// The <see cref="Context.Product"/> to use.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// An exception thrown when there is no product version.
        /// </exception>
        public EmployeeProduct(Context.Product product)
        {
            var latestVersion = product.ProductVersions.MaxBy(version => version.VersionNumber);
            if (latestVersion == null)
                throw new NullReferenceException();
            Id = product.Id;
            Name = latestVersion.Name;
            Description = latestVersion.Description;
            Price = latestVersion.Price;
            Deleted = product.Deleted;
            ClosedSpecificationValues = latestVersion.ClosedSpecificationValues
                .Select(closedSpecificationValue => new ClosedSpecificationValue(closedSpecificationValue))
                .ToArray();
            OpenSpecificationValues = latestVersion.OpenSpecificationValues
                .Select(openSpecificationValue => new OpenSpecificationValue(openSpecificationValue))
                .ToArray();
            Images = latestVersion.ProductImages.Select(image => new Image(image))
                .ToArray();
        }
    }
}