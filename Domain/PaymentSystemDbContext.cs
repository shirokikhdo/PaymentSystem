using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public sealed class PaymentSystemDbContext : IdentityDbContext<UserEntity, IdentityRoleEntity, long>
{
    public DbSet<CustomerEntity> Customers { get; set; } = null!;

    public DbSet<CartEntity> Carts { get; set; } = null!;

    public DbSet<CartItemEntity> CartItems { get; set; } = null!;

    public DbSet<OrderEntity> Orders { get; set; } = null!;

    public DbSet<MerchantEntity> Merchants { get; set; } = null!;

    public PaymentSystemDbContext(DbContextOptions<PaymentSystemDbContext> options)
        : base(options)
    {
        if (Database.GetPendingMigrations().Any())
            Database.Migrate();
    }
}