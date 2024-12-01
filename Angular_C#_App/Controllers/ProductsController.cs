using Core.Interfaces;
using Core.Enities;
using Microsoft.AspNetCore.Mvc;
using Core.Specifications;

namespace Shop_App.Controllers
{
    public class ProductsController(IEntityRepository<Product> repo) : BaseApiController
    { 
        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            try
            {
               await repo.AddAsync(product);
               return Ok();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
            try
            {
                var spec = new BrandListSpecification();
                return Ok(await repo.ListAsync(spec));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetTypes()
        {
            try
            {
                var spec = new TypeListSpecification();
                return Ok(await repo.ListAsync(spec));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            try
            {
                return Ok(await repo.GetByIdAsync(id));
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
    }
}
