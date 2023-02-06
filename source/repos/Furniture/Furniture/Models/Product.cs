namespace Furniture.Models
{
    public class Product:Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } = 0;

        public string? filename { get; set; }
        
    }
}
