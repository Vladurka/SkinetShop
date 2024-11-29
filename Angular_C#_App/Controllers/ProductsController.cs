using Contracts.DTO.Products;
using Contracts.Interfaces;
using Core.Enities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shop_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepository) 
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductAddRequest productAdd)
        {
            try
            {
               await _productRepository.AddProduct(productAdd);
               return Ok();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetProducts();
                return Ok(products);
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
                var product = await _productRepository.GetProductById(id);

                return Ok(product);
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
                await _productRepository.UpdateProduct(product);
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
                await _productRepository.DeleteProduct(id);
                return NoContent();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
