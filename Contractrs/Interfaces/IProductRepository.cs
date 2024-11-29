using Contracts.DTO.Products;
using Core.Enities;

namespace Contracts.Interfaces
{
    public interface IProductRepository
    {
        public Task AddProduct(ProductAddRequest product);
        public Task<IEnumerable<Product>> GetProducts();
        public Task<Product> GetProductById(Guid id);
        public Task UpdateProduct(Product product);
        public Task DeleteProduct(Guid id);
        public Task SaveChangesAsync();
        public Task<bool> ProductExistsId(Guid id);
        public Task<bool> ProductExistsName(string name);
    }
}
