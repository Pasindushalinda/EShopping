using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalog.Api.Data;
using Catalog.Api.DTOs;
using Catalog.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductDbContext _context;
    private readonly IMapper _mapper;

    public ProductsController(ProductDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // [HttpGet]
    // public async Task<List<ProductDto>> GetAuctions()
    // {
    //     var products = await _context.Products
    //         .Include(x => x.ProductBrand)
    //         .Include(x => x.ProductType)
    //         .ToListAsync();
    //
    //     return _mapper.Map<List<ProductDto>>(products);
    // }
    
    [HttpGet]
    public async Task<List<ProductDto>> GetAuctions(string date)
    {
        var query = _context
            .Products
            .OrderBy(x => x.ProductBrand)
            .ThenBy(x=>x.ProductType)
            .AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            query = query
                .Where(x => 
                    x.UpdatedAt
                        .CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
    {
        var product = await _context.Products
            .Include(x => x.ProductBrand)
            .Include(x => x.ProductType)
            .FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<ProductDto>(product);
    }
    
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateAuction(CreateProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
    
        _context.Products.Add(product);
        var result = await _context.SaveChangesAsync() > 0;
    
        if (!result) return BadRequest("Changes not applied to database");
    
        return CreatedAtAction(nameof(GetProductById), new { product.Id },
            _mapper.Map<ProductDto>(product));
    }

    // [HttpPut("{id}")]
    // public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    // {
    //     var auction = await _context.Auctions
    //         .Include(x => x.Item)
    //         .FirstOrDefaultAsync(x => x.Id == id);
    //
    //     if (auction == null) return NotFound();
    //
    //     //Todo check seller name
    //
    //     auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
    //     auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
    //     auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
    //     auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
    //     auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
    //
    //     var result = await _context.SaveChangesAsync() > 0;
    //
    //     if (result) return Ok();
    //
    //     return BadRequest("Changes not apply to database");
    // }
    //
    // [HttpDelete("{id}")]
    // public async Task<ActionResult> DeleteAuction(Guid id)
    // {
    //     var auction = await _context.Auctions.FindAsync(id);
    //
    //     if (auction == null) return NotFound();
    //
    //     //seller
    //
    //     _context.Auctions.Remove(auction);
    //     var result = await _context.SaveChangesAsync() > 0;
    //
    //     if (result) return Ok();
    //
    //     return BadRequest("changes not apply to database");
    // }
}