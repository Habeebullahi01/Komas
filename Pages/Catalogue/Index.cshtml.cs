using Komas.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Komas.Pages;
public class CatalogueIndexModel : PageModel
{
    private readonly HttpClient _httpClient;
    public List<Product>? Products;
    public CatalogueIndexModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task OnGetAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<Product>>("https://localhost:7007/Product");
        if (response != null)
            Products = response;
    }
}