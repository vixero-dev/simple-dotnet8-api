using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Products
{
    public class AddProductRequest
    {
        [Required]
        public required string Name { get; set; }

        public Dictionary<string, string>? Data { get; set; }
    }
}
