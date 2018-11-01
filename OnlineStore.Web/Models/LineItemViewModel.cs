namespace OnlineStore.Web.Models
{
    public class LineItemViewModel
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public int ProductId { get; set; }
        public ProductViewModel ProductViewModel { get; set; }

        public int OrderId { get; set; }
        public OrderViewModel OrderViewModel { get; set; }
    }
}