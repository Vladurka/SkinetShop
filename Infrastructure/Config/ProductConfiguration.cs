using Core.Enities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Descriptor).IsRequired();
            builder.Property(x => x.PictureUrl).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Brand).IsRequired();
            builder.Property(x => x.QuantityInStock).IsRequired();
        }
    }
}
