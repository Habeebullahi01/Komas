namespace Komas.Models;

public class ProductResponse
{
    public int PageNumber;

    public int PageSize;

    public int TotalCount;

    public int TotalPages;

    public required List<Product> Items { get; set; }
}