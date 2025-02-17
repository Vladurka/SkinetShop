using Core.Contracts;
using Core.Enities;
using Microsoft.AspNetCore.Mvc;
using Core.Specifications;

namespace Skinet.Controllers
{
    public class ProductsController(IUnitOfWork unit) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            await unit.Repository<Product>().AddAsync(product);
            
            if(await unit.Complete())
                return Ok();

            return BadRequest("Not created");
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts
            ([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);

            return await CreatePagedResult(unit.Repository<Product>(), spec, specParams.PageIndex,
                specParams.PageSize);

        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await unit.Repository<Product>().ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await unit.Repository<Product>().ListAsync(spec));
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(id);
            return Ok(product);
        }

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(Product product)
        {
            await unit.Repository<Product>().UpdateAsync(product);
            
            if(await unit.Complete())
                return Ok(product);

            return BadRequest("Entity not updated");
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            await unit.Repository<Product>().RemoveAsync(id);
            
            if(await unit.Complete())
                return NoContent();

            return BadRequest("Product was not deleted");
        }
    }
}
