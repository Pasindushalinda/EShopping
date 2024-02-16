namespace Search.Api.RequestHelpers;

public class SearchParams
{
    public string SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 4;
    public string ProductBrand { get; set; }
    public string ProductType { get; set; }
    public string OrderBy { get; set; }
    public string FilterBy { get; set; }
}