using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using Search.Api.Models;
using Search.Api.RequestHelpers;

namespace Search.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController: ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Product>>> SearchItems([FromQuery] SearchParams searchParams)
    {
        var query = DB.PagedSearch<Product, Product>();

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(MongoDB.Entities.Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        query = searchParams.OrderBy switch
        {
            "brand" => query.Sort(x => x.Ascending(a => a.ProductBrand)),
            "type" => query.Sort(x => x.Ascending(a => a.ProductType)),
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
        };
        
        // query = searchParams.FilterBy switch
        // {
        //     "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
        //     "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6)
        //                                      && x.AuctionEnd > DateTime.UtcNow),
        //     _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
        // };
        
        // if (!string.IsNullOrEmpty(searchParams.Seller))
        // {
        //     query.Match(x => x.Seller == searchParams.Seller);
        // }
        //
        // if (!string.IsNullOrEmpty(searchParams.Winner))
        // {
        //     query.Match(x => x.Winner == searchParams.Winner);
        // }

        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        return Ok(new
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}