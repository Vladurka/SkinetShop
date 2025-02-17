using Core.Contracts;
using Core.Enities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Skinet.Controllers;

public class PaymentsController(IPaymentService paymentService, 
    IUnitOfWork unit) : BaseApiController
{
    [Authorize]
    [HttpPost("{cartId:guid}")]
    public async Task<ActionResult> CreateOrUpdatePaymentIntent(Guid cartId)
    {
        var cart = await paymentService.CreateOrUpdatePaymentIntent(cartId.ToString());

        if (cart == null) return BadRequest("Problem with your cart");

        return Ok(cart);
    }

    [HttpGet("delivery-methods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
    {
        return Ok(await unit.Repository<DeliveryMethod>().ListAllAsync()); 
    }
}