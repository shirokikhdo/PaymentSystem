using Application.Models.Merchants;

namespace Application.Abstractions;

public interface IMerchantsService
{
    Task<MerchantDto> Create(MerchantDto createMerchantDto);
}