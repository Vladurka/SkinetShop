using Core.Enities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        public Task AddProductAsync(Product product);
        public Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, 
            string? type, string? sort);
        public Task<IReadOnlyList<string>> GetBrandsAsync();
        public Task<IReadOnlyList<string>> GetTypesAsync();
        public Task<Product> GetProductByIdAsync(Guid id);
        public Task UpdateProductAsync(Product product);
        public Task DeleteProductAsync(Guid id);
        public Task SaveChangesAsync();
        public Task<bool> ProductExistsIdAsync(Guid id);
        public Task<bool> ProductExistsName(string name);
    }
}
