using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
    
    public class Product:Entity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; } = 0;

        public string? filename { get; set; }
        [Required]
        public long? contact { get; set; }
        
    }
}
