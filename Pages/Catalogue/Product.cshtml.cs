using Komas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Komas.Pages;

public class ViewProductPageModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public Product? Product;
    public ViewProductPageModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("Product");
        Product = await client.GetFromJsonAsync<Product>($"Product/{id}");
        if (Product == null)
        {
            return NotFound();
        }
        return Page();
    }
}