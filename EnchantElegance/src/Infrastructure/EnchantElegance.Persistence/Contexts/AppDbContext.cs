﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Persistence.Contexts
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Slider> Sliders { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<Color> Colors { get; set; }
		public DbSet<ProductColor> ProductColors { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			modelBuilder.Entity<Product>()
		.Property(p => p.CurrentPrice)
		.HasColumnType("decimal(18, 2)");

			modelBuilder.Entity<Product>()
				.Property(p => p.OldPrice)
				.HasColumnType("decimal(18, 2)");

			base.OnModelCreating(modelBuilder);
        }

    }
}
