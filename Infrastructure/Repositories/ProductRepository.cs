using Core.Interfaces;
using Core.Enities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository(StoreContext context) : IProductRepository
    {
        public async Task AddProductAsync(Product product)
        {
            if (await ProductExistsName(product.Name))
                throw new InvalidOperationException("Product with this name already exists");

            context.Products.Add(product);

            await SaveChangesAsync();
        }

        public async Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await context.Products.Select(x => x.Brand)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
            return await context.Products.Select(x => x.Type)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand,
            string? type, string? sort)
        {
            var query = context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand))
                query = query.Where(x => x.Brand == brand);

            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(x => x.Type == type);

            query = sort switch
            {
                "priceAsc" => query.OrderBy(x => x.Price),
                "priceDesc" => query.OrderByDescending(x => x.Price),
                _ => query.OrderBy(x => x.Name),
            };

            return await query.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var product = await context.Products.FindAsync(id);

            if (!await ProductExistsIdAsync(id) || product == null)
                throw new InvalidOperationException("Product was not found");

            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            if (!await ProductExistsIdAsync(product.Id))
                throw new InvalidOperationException("Can not update this product");

            context.Entry(product).State = EntityState.Modified;
            await SaveChangesAsync();
        }
        public async Task DeleteProductAsync(Guid id)
        {
            var product = await GetProductByIdAsync(id);

            context.Remove(product);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            if (await context.SaveChangesAsync() <= 0)
                throw new InvalidOperationException("Could not save changes");
        }

        public async Task<bool> ProductExistsIdAsync(Guid id) =>
             await context.Products.AnyAsync(x => x.Id == id);

        public async Task<bool> ProductExistsName(string name) =>
            await context.Products.AnyAsync(x => x.Name == name);
    }
}
