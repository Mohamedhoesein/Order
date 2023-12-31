namespace Order.API.Controllers.CategoryController.Models
{
    public class OpenSpecification
    {
        public string Name { get; set; }
        public bool Deleted { get; set; }

        public OpenSpecification() {}

        public OpenSpecification(Context.OpenSpecification closedSpecification)
        {
            Name = closedSpecification.Name;
            Deleted = closedSpecification.Deleted;
        }
    }
}