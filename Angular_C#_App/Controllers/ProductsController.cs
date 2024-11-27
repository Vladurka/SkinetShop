using Contracts.DTO.Products;
using Core.Enities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shop_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _storeContext;
        public ProductsController(StoreContext storeContext) 
        {
            _storeContext = storeContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                return await _storeContext.Products.ToListAsync();
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
                var product = await _storeContext.Products.FindAsync(id);

                if (product == null)
                    return NotFound();

                return product;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductAddRequest productAdd)
        {
            var product = productAdd.ToProduct();
            _storeContext.Products.Add(product);

            await _storeContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
            if (!ProductExists(product.Id))
                return BadRequest("Can not update this product");

            _storeContext.Entry(product).State = EntityState.Modified;
            await _storeContext.SaveChangesAsync(); 

            return Ok(product);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var product = await _storeContext.Products.FindAsync(id);

            if(product == null)
                return NotFound();

            _storeContext.Remove(product);

            await _storeContext.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(Guid id) =>
            _storeContext.Products.Any(x => x.Id == id);
    }
}
