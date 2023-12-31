namespace Order.API.Controllers.CategoryController.Models
{
    public class ClosedSpecification
    {
        public string Name { get; set; }
        public ClosedSpecificationValue[] Values { get; set; }
        public string? Filter { get; set; }
        public bool Deleted { get; set; }

        public ClosedSpecification() {}

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