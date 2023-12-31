namespace Order.API.Controllers.CategoryController.Models
{
    public class ClosedSpecificationValue
    {
        public string Value { get; set; }
        public bool Deleted { get; set; }

        public ClosedSpecificationValue() {}

        public ClosedSpecificationValue(Context.ClosedSpecificationValue closedSpecificationValue)
        {
            Value = closedSpecificationValue.Value;
            Deleted = closedSpecificationValue.Deleted;
        }
    }
}