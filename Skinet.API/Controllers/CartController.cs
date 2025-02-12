using Core.Enities;
using Core.Enities.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Skinet.Controllers
{
    public class CartController(ICartRepository repo) : BaseApiController
    {
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ShoppingCart>> GetCart(Guid id)
        {
            var cart = await repo.GetCartAsync(id.ToString());
            
            return Ok(cart ?? new ShoppingCart { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> SetCart(ShoppingCart cart)
        {
            var updatedCart = await repo.SetCartAsync(cart);

            if (updatedCart == null)
                return BadRequest("Problem with the cart");

            return Ok(updatedCart);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCart(Guid id)
        {
            var result = await repo.DeleteCartAsync(id.ToString());

            if (!result)
                return BadRequest("Problem with deleting the cart");
            
            return Ok();
        }
    }
}
