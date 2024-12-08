using Core.Interfaces;
using Core.Enities;
using Microsoft.AspNetCore.Mvc;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Shop_App.Controllers
{
    public class ProductsController(IEntityRepository<Product> repo, StoreContext context) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            if(await ExistsNameAsync(product.Name))
                return BadRequest();

            await repo.AddAsync(product);
            return Ok();
        }

        [HttpPost("AddProducts")]
        public async Task<ActionResult> AddProducts(Product[] products)
        {
            foreach (var product in products)
                await repo.AddAsync(product);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts
            ([FromQuery] ProductSpecParams specParams)
        {
            try
            {
                var spec = new ProductSpecification(specParams);

                return await CreatePagedResult(repo, spec, specParams.PageIndex,
                    specParams.PageSize);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await repo.ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await repo.ListAsync(spec));
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await repo.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(Product product)
        {
            try
            {
                await repo.UpdateAsync(product);
                return Ok(product);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await repo.RemoveAsync(id);
                return NoContent();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<bool> ExistsNameAsync(string name) =>
            await context.Products.AnyAsync(x => x.Name == name);
    }
}
