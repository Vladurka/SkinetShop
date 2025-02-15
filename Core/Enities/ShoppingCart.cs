namespace Core.Enities
{
    public class ShoppingCart : BaseEntity
    {
        public List<Product> Items { get; set; } = new();
        public Guid? DeliveryMethodId { get; set; }
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
    }
}
