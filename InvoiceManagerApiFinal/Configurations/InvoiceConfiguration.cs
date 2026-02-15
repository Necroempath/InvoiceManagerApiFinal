using InvoiceManagerApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagerApi.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.StartDate)
        .IsRequired();

        builder.Property(i => i.EndDate)
        .IsRequired();

        builder.Property(i => i.CreatedAt)
        .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Invoice_StartDate_Less_Than_EndDate", "[StartDate] < [EndDate]"
            ));

        builder.HasOne(i => i.Customer)
        .WithMany(c => c.Invoices)
        .HasForeignKey(i => i.CustomerId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired();
    }
}