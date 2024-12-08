namespace Core.Enities.Service.Interfaces
{
    public interface ICartRepository
    {
        public Task<ShoppingCart?> GetCartAsync(string key);
        public Task<ShoppingCart?> SetCartAsync(ShoppingCart cart);
        public Task<bool> DeleteCartAsync(string key);
    }
}
