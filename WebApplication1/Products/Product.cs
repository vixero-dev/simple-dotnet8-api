namespace WebApplication1.Products
{
    public class Product
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public Dictionary<string, object>? Data { get; set; }
    }
}
