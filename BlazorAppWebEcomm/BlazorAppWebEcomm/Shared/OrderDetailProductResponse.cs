namespace BlazorAppWebEcomm.Shared
{
    public class OrderDetailProductResponse
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string  ProductType { get; set; }

        public int ProductTypeId { get; set; }
        public string  ProductImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

    }
}