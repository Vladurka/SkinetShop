using Contracts.DTO.Products;
using Contracts.Interfaces;
using Core.Enities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _storeContext;
        public ProductRepository(StoreContext storeContext) 
        {
            _storeContext = storeContext;
        }

        public async Task AddProduct(ProductAddRequest productAdd)
        {
            if (await ProductExistsName(productAdd.Name))
                throw new InvalidOperationException("Product with this name already exists");

            var product = productAdd.ToProduct();
            _storeContext.Products.Add(product);

            await SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts() =>
            await _storeContext.Products.ToListAsync();

        public async Task<Product> GetProductById(Guid id)
        {
            var product = await _storeContext.Products.FindAsync(id);

            if (!await ProductExistsId(id) || product == null)
                throw new InvalidOperationException("Product was not found");

            return product;
        }

        public async Task UpdateProduct(Product product)
        {
            if (!await ProductExistsId(product.Id))
                throw new InvalidOperationException("Can not update this product");

            _storeContext.Entry(product).State = EntityState.Modified;
            await SaveChangesAsync();
        }
        public async Task DeleteProduct(Guid id)
        {
            var product = await GetProductById(id);

            _storeContext.Remove(product);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            if (await _storeContext.SaveChangesAsync() <= 0)
                throw new InvalidOperationException("Could not save changes");
        }
            
        public async Task<bool> ProductExistsId(Guid id) =>
             await _storeContext.Products.AnyAsync(x => x.Id == id);

        public async Task<bool> ProductExistsName(string name) =>
            await _storeContext.Products.AnyAsync(x => x.Name == name);
    }
}
