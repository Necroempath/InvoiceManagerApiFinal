using InvoiceManagerApi.Models;
using InvoiceManagerApiFinal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagerApi.Data;

public class InvoiceManagerDbContext : IdentityDbContext<User>
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceRow> InvoiceRows => Set<InvoiceRow>();

    public InvoiceManagerDbContext(DbContextOptions<InvoiceManagerDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvoiceManagerDbContext).Assembly);
    }

}
