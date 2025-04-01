namespace WebApplication1.Products
{
    public class AddProductResponse
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public Dictionary<string, object>? Data { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
