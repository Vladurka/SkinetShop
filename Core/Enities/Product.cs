using System.ComponentModel.DataAnnotations;

namespace Core.Enities
{
    public class Product : BaseEntity
    {
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Descriptor { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string PictureUrl { get; set; } = string.Empty;

        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        [StringLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        public Product(string name, string descriptor, 
            decimal price, string pictureUrl, 
            string type, string brand, int quantity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Descriptor = descriptor;
            Price = price;
            PictureUrl = pictureUrl;
            Type = type;
            Brand = brand;
            Quantity = quantity;
        }
    }
}
