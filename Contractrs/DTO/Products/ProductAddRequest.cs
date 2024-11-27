using System.ComponentModel.DataAnnotations;
using Core.Enities;

namespace Contracts.DTO.Products
{
    public class ProductAddRequest
    {
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string Descriptor { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public string PictureUrl { get; set; } = string.Empty;

        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        [StringLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int QuantityInStock { get; set; }

        public Product ToProduct()
        {
            var product =  new Product(Guid.NewGuid(), Name, Descriptor,
                Price, PictureUrl, Type, Brand,
                QuantityInStock);

            return product;
        }
    }
}
