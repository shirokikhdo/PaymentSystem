using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public sealed class PaymentSystemDbContext : DbContext
{
    public DbSet<CustomerEntity> Customers { get; set; } = null!;

    public DbSet<CartEntity> Carts { get; set; } = null!;

    public DbSet<CartItemEntity> CartItems { get; set; } = null!;

    public DbSet<OrderEntity> Orders { get; set; } = null!;

    public PaymentSystemDbContext(DbContextOptions<PaymentSystemDbContext> options)
        : base(options)
    {
        if (Database.GetPendingMigrations().Any())
            Database.Migrate();
    }
}