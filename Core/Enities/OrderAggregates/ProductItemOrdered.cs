namespace Core.Enities.OrderAggregates;

public class ProductItemOrdered : BaseEntity
{
    public Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string PictureUrl { get; set; }
}