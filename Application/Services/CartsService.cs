using Application.Abstractions;
using Application.Mappers;
using Application.Models.Carts;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class CartsService : ICartsService
{
    private readonly PaymentSystemDbContext _dbContext;

    public CartsService(PaymentSystemDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CartDto> Create(CartDto cart)
    {
        var cartEntity = new CartEntity();
        var cartSaveResult = await _dbContext.Carts.AddAsync(cartEntity);
        await _dbContext.SaveChangesAsync();
        var id = cartSaveResult.Entity.Id;

        var cartItems = cart.CartItems
            .Select(item => new CartItemEntity
            {
                Name = item.Name,
                Price = item.Price,
                Quantity = item.Quantity,
                CartId = id
            });

        await _dbContext.CartItems.AddRangeAsync(cartItems);
        await _dbContext.SaveChangesAsync();

        var result = await _dbContext.Carts
            .Include(x => x.CartItems)
            .FirstAsync(x => x.Id == id);

        return result.ToDto();
    }
}