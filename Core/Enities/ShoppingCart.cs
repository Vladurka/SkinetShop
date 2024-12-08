namespace Core.Enities
{
    public class ShoppingCart : BaseEntity
    {
        public List<Product> Items { get; set; } = new List<Product>();
    }
}
