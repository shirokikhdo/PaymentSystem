namespace Application.Models.Merchants;

public class MerchantDto
{
    public long? Id { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? WebSite { get; set; }
}