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
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.StockQuantity).IsRequired();
            builder.Property(x => x.Description).IsRequired(false).HasColumnType("text");
            builder.Property(x => x.Brand).IsRequired(false).HasColumnType("text");
            builder.Property(x => x.SkinType).IsRequired().HasMaxLength(50).HasColumnType("text");
            builder.Property(x => x.SkinTone).IsRequired().HasMaxLength(50).HasColumnType("text");
            builder.Property(x => x.Color).IsRequired(false).HasMaxLength(50).HasColumnType("text");
            builder.Property(x => x.Usage).IsRequired().HasMaxLength(150).HasColumnType("text");
            builder.Property(x => x.Ingredients).IsRequired().HasColumnType("text");
        }
    }
}
