using InvoiceManagerApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceManagerApi.Configurations;

public class InvoiceRowConfiguration : IEntityTypeConfiguration<InvoiceRow>
{
    public void Configure(EntityTypeBuilder<InvoiceRow> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Quantity)
        .IsRequired();

        builder.Property(i => i.Rate)
        .IsRequired();

        builder.Property(i => i.Sum)
        .IsRequired();

        builder.Property(i => i.Service)
        .HasMaxLength(100)
        .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_InvoiceRow_Quantity_Positive", "[Quantity] > 0"
            ));

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_InvoiceRow_Rate_Positive", "[Rate] > 0"
            ));

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_InvoiceRow_Sum_Positive", "[Sum] > 0"
            ));

        builder.HasOne(i => i.Invoice)
        .WithMany(i => i.Rows)
        .HasForeignKey(i => i.InvoiceId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired();
    }
}
