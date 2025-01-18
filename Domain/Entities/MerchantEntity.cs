namespace Domain.Entities;

public class MerchantEntity : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? WebSite { get; set; }

    public List<UserEntity> User { get; set; }
}