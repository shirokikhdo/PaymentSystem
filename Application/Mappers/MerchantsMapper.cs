using Application.Models.Merchants;
using Domain.Entities;

namespace Application.Mappers;

public static class MerchantsMapper
{
    public static MerchantDto ToDto(this MerchantEntity entity) =>
        new MerchantDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Phone = entity.Phone,
            WebSite = entity.WebSite
        };

    public static MerchantEntity ToEntity(this MerchantDto merchantDto) =>
        new MerchantEntity
        {
            Name = merchantDto.Name,
            Phone = merchantDto.Phone,
            WebSite = merchantDto.WebSite
        };
}