using Komas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Komas.Pages;

public class AddProductPageModel : PageModel
{
    [BindProperty]
    public required Product? Product { get; set; } = new Product() { ProductName = "", Description = "", Price = 0 };

    private IHttpClientFactory _httpClientFactory;

    public AddProductPageModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (Product == null)
        {
            Console.WriteLine("Product is null");
            throw new Exception("Product is empty");
        }
        // validate Model
        if (Product.ProductName == null || string.IsNullOrEmpty(Product.ProductName.Trim()))
        {
            ModelState.AddModelError("ProductName", "Product Name can't be empty");
        }
        if (Product.Description == null || string.IsNullOrEmpty(Product.Description.Trim()))
        {
            ModelState.AddModelError("Description", "Product description should not be empty");
        }
        if (Product.Price <= 0)
        {
            ModelState.AddModelError("Price", "Product should cost more than zero");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }
        else
        {
            // send request to api
            var httpClient = _httpClientFactory.CreateClient("Product");
            var result = await httpClient.PostAsJsonAsync<Product>("Product", Product);
            if (result.IsSuccessStatusCode)
            {

                // redirect to catalogue
                return RedirectToPage("Index");
            }
            else
            {
                return BadRequest();
            }

        }


        // Console.WriteLine(Product.ProductName);
        // Console.WriteLine(Product.Description);
        // Console.WriteLine(Product.Price);
    }
}