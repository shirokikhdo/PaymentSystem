using Application.Abstractions;
using Application.Mappers;
using Application.Models.Merchants;
using Domain;

namespace Application.Services;

public class MerchantsService : IMerchantsService
{
    private readonly PaymentSystemDbContext _dbContext;

    public MerchantsService(PaymentSystemDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MerchantDto> Create(MerchantDto createMerchantDto)
    {
        var entity = createMerchantDto.ToEntity();

        var result = await _dbContext.Merchants.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return result.Entity.ToDto();
    }
}