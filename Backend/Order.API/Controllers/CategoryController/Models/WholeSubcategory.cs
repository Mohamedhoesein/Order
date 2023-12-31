namespace Order.API.Controllers.CategoryController.Models
{
    public class WholeSubcategory
    {
        public bool Deleted { get; set; }
        public string Name { get; set; }
        public ClosedSpecification[] ClosedSpecifications { get; set; }
        public OpenSpecification[] OpenSpecifications { get; set; }

        public WholeSubcategory() {}

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