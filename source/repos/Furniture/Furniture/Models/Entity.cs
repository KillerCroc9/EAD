namespace Furniture.Models
{
    public abstract class Entity
    {
        public string CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedByUser { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
