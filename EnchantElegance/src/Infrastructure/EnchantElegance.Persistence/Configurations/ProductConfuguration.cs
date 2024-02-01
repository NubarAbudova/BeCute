using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnchantElegance.Persistence.Configurations
{
    internal class ProductConfuguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.StockQuantity).IsRequired();
            builder.Property(p => p.Description).IsRequired(false).HasColumnType("text");
            builder.Property(p => p.Brand).IsRequired(false).HasColumnType("text");
            builder.Property(p => p.SkinType).IsRequired().HasMaxLength(50).HasColumnType("text");
            builder.Property(p => p.SkinTone).IsRequired().HasMaxLength(50).HasColumnType("text");
            builder.Property(p => p.Color).IsRequired(false).HasMaxLength(50).HasColumnType("text");
            builder.Property(p => p.Usage).IsRequired().HasMaxLength(150).HasColumnType("text");
            builder.Property(p => p.Ingredients).IsRequired().HasColumnType("text");
        }
    }
}
