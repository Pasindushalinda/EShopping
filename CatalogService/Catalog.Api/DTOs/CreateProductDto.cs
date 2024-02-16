namespace Catalog.Api.DTOs;

public class CreateProductDto
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string PictureUrl { get; set; }
    public Guid ProductBrandId { get; set; }
    public Guid ProductTypeId { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime AuctionEnd { get; set; }
}