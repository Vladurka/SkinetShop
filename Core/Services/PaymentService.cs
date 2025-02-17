using Core.Contracts;
using Core.Enities;
using Core.Enities.Service.Contracts;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Enities.Product;

namespace Core.Services;

public class PaymentService(IConfiguration config, ICartRepository cartRepo,
    IUnitOfWork unit) : IPaymentService
{
    public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
    {
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
        var cart = await cartRepo.GetCartAsync(cartId);

        if (cart == null) return null;

        decimal shippingPrice = 0m;

        if (cart.DeliveryMethodId.HasValue)
        {
            var deliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync(cart.DeliveryMethodId.Value);
            if (deliveryMethod == null) return null;

            shippingPrice = deliveryMethod.Price;
        }

        foreach (var item in cart.Items)
        {
            var productItem = await unit.Repository<Product>().GetByIdAsync(item.Id);
            if (productItem == null) return null;

            item.Price = productItem.Price;
        }

        var service = new PaymentIntentService();
        PaymentIntent? intent = null;

        var totalAmount = (long)(cart.Items.Sum(x => x.Price * 100 * x.Quantity)) + (long)shippingPrice * 100;

        if (string.IsNullOrWhiteSpace(cart.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = totalAmount,
                Currency = "usd",
                PaymentMethodTypes = ["card"]
            };

            intent = await service.CreateAsync(options);
            if (intent != null)
            {
                cart.PaymentIntentId = intent.Id;
                cart.ClientSecret = intent.ClientSecret;
            }
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = totalAmount
            };

            intent = await service.UpdateAsync(cart.PaymentIntentId, options);
        }

        await cartRepo.SetCartAsync(cart);
        return cart;
    }

}