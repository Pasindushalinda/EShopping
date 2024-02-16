using MongoDB.Entities;

namespace Search.Api.Models;

public class Product : Entity
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string PictureUrl { get; set; }
    public decimal Price { get; set; }
    public string ProductBrand { get; set; }
    public string ProductType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime AuctionEnd { get; set; }
}