namespace Order.API.Controllers.ProductController.Models.Send
{
    /// <summary>
    /// An image for a product.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// The version of the associated product.
        /// </summary>
        public long VersionNumber { get; set; }
        /// <summary>
        /// The name of the image.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The path to the image.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// Initialize a new empty <see cref="Image"/>.
        /// </summary>
        public Image() {}

        /// <summary>
        /// Initialize a new <see cref="Image"/> based on a <see cref="Context.Image"/>.
        /// </summary>
        /// <param name="image">
        /// The <see cref="Context.ProductImage"/> to use.
        /// </param>
        public Image(Context.ProductImage image)
        {
            VersionNumber = image.ProductVersionNumber;
            Name = image.Name;
            File = image.File.TrimStart('.');
        }
    }
}