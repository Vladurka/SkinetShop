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

            if (cart == null)
            {
                cart = new ShoppingCart { Id = id };
                await repo.SetCartAsync(cart);
            }
            return Ok(cart);
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
            return !result ? BadRequest("Problem deleting the cart") : Ok();
        }
    }
}
