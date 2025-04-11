using Komas.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Komas.Pages;
public class CatalogueIndexModel : PageModel
{
    // private HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;
    public required List<Product> Products;
    public CatalogueIndexModel(IHttpClientFactory httpClientFactory)
    {
        // _httpClient = httpClient;
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        var _httpClient = _httpClientFactory.CreateClient("Product");
        var response = await _httpClient.GetFromJsonAsync<ProductResponse>("Product");
        if (response != null)
            Products = response.Items;
    }
}