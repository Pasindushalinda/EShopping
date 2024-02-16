using MongoDB.Entities;
using Search.Api.Models;

namespace Search.Api.Services;

public class AuctionSvcHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<List<Product>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Product, string>()
            .Sort(x => x.Descending(x => x.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        return await _httpClient
            .GetFromJsonAsync<List<Product>>(
                _config["CatalogServiceUrl"]
                + "/api/products?date=" + lastUpdated);
    }
}