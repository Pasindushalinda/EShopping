using System.Text.Json;
using Catalog.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        SeedData(scope.ServiceProvider.GetService<ProductDbContext>());
    }

    private static void SeedData(ProductDbContext context)
    {
        context.Database.Migrate();

        if (!context.ProductBrands.Any())
        {
            var brandsData = File.ReadAllText("../Catalog.Api/Data/SeedData/brands.json");
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
            context.ProductBrands.AddRange(brands);
        }

        if (!context.ProductTypes.Any())
        {
            var typesData = File.ReadAllText("../Catalog.Api/Data/SeedData/types.json");
            var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
            context.ProductTypes.AddRange(types);
        }

        if (!context.Products.Any())
        {
            var productsData = File.ReadAllText("../Catalog.Api/Data/SeedData/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            context.Products.AddRange(products);
        }

        if (context.ChangeTracker.HasChanges()) context.SaveChanges();
    }
}