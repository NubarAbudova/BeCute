using EnchantElegance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnchantElegance.Persistence.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
