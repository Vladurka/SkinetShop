using System.ComponentModel.DataAnnotations;
using Core.Enities.OrderAggregates;

namespace Core.Interfaces.DTOs;

public class CreateOrderDto
{
    [Required]
    public Guid CartId { get; set; }

    [Required]
    public Guid DeliveryMethodId { get; set; }

    [Required]
    public ShippingAddress ShippingAddress { get; set; } = null!;

    [Required]
    public PaymentSummary PaymentSummary { get; set; } = null!;
}