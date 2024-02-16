using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using Search.Api.Models;
using Search.Api.Services;

namespace Search.Api.Data;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("SearchDb", MongoClientSettings
            .FromConnectionString(
                app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Product>()
            .Key(x => x.Name, KeyType.Text)
            .Key(x => x.ProductBrand, KeyType.Text)
            .Key(x => x.ProductType, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Product>();

        // if (count == 0)
        // {
        //     Console.WriteLine("No data - will attempt to seed");
        //     var productData = await File.ReadAllTextAsync("Data/product.json");
        //
        //     var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        //
        //     var products = JsonSerializer.Deserialize<List<Product>>(productData, options);
        //
        //     await DB.SaveAsync(products);
        // }

        using var scope = app.Services.CreateScope();

        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();
        
        var items = await httpClient.GetItemsForSearchDb();
        
        Console.WriteLine(items.Count + " returned from the auction service");
        
        if (items.Count > 0) await DB.SaveAsync(items);
    }
}