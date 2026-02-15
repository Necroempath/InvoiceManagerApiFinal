using InvoiceManagerApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace InvoiceManagerApi.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
        .IsRequired()
        .HasMaxLength(100);

        builder.Property(c => c.Email)
        .IsRequired()
        .HasMaxLength(100);

        builder.Property(c => c.CreatedAt)
        .IsRequired();

    }
}
