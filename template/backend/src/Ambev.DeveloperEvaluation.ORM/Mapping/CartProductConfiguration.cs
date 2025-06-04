using Ambev.DeveloperEvaluation.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class CartProductConfiguration : IEntityTypeConfiguration<CartProduct>
    {
        public void Configure(EntityTypeBuilder<CartProduct> builder)
        {
            builder.ToTable("CartProducts");

            builder.HasKey(cp => cp.Id);
            builder.Property(cp => cp.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(cp => cp.ProductId)
                .IsRequired();

            builder.Property(cp => cp.Quantity)
                .IsRequired();

            // Cart Foreign Key
            builder.Property<Guid>("CartId")
                .IsRequired();

            builder.HasIndex("CartId");

            builder.HasOne(cp => cp.Product)
               .WithMany()
               .HasForeignKey(cp => cp.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}