using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using MobileProgramming.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Data.Persistence.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Configure the primary key
            builder.HasKey(p => p.ProductID);

            // Configure properties
            builder.Property(p => p.ProductName)
                .HasMaxLength(255)
                .IsRequired(false);  // Optional field, allow null

            builder.Property(p => p.ProductDescription)
                .HasColumnType("text")
                .IsRequired(false);  // Optional field, allow null

            builder.Property(p => p.ProductImage)
                .HasMaxLength(255)
                .IsRequired(false);  // Optional field, allow null

            builder.Property(p => p.Price)
                .HasColumnType("decimal(10, 2)")
                .IsRequired();  // Required field

            builder.Property(p => p.Category)
                .HasMaxLength(100)
                .IsRequired(false);  // Optional field, allow null

            builder.Property(p => p.Brand)
                .HasMaxLength(100)
                .IsRequired(false);  // Optional field, allow null

            builder.Property(p => p.Rating)
                .HasColumnType("decimal(3, 2)")
                .IsRequired(false);  // Optional field, allow null

            // Configure CreatedAt as a long (representing a timestamp, for example)
            builder.Property(p => p.CreatedAt)
                .IsRequired();  // Required field

            // Configure UpdateAt as a long (representing a timestamp, for example)
            builder.Property(p => p.UpdateAt)
                .IsRequired();  // Required field
        }
    }
}
