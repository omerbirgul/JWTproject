using AuthServer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Data.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id); // Id değerini PK olarak set eder. 
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200); 
        builder.Property(x => x.Stock).IsRequired();
        builder.Property(x => x.Price).HasColumnType("decimal(9,2)");
        builder.Property(x => x.UserId).IsRequired();
    }
}