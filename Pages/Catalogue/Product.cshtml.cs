using Komas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Komas.Pages;

public class ViewProductPageModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    [BindProperty]
    public required Product Product { get; set; } = new Product() { ProductName = "", Description = "", Price = 0 };

    [BindProperty]
    public Product ProductFormModel { get; set; } = new Product() { ProductName = "", Description = "", Price = 0 };
    public ViewProductPageModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("Product");
        Product = await client.GetFromJsonAsync<Product>($"Product/{id}") ?? new Product() { ProductName = "", Description = "", Price = 0 };
        if (Product == null)
        {
            return NotFound();
        }
        ProductFormModel = new Product
        {
            ProductId = Product.ProductId,
            ProductName = Product.ProductName,
            Description = Product.Description,
            Price = Product.Price
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Products have to be populated again because after posting to this route, and returning Page(), the whole thing is re-rendered.
        // Solution is to call this route and refetch the origina product from the API, validate the input ProductFormModel, populate ModelErrors if any, return Page(). That way, the newly rendered page will have both a Product and a ProductFormModel.
        if (ProductFormModel.ProductName == null || string.IsNullOrEmpty(ProductFormModel.ProductName.Trim()))
        {
            ModelState.AddModelError("ProductFormModel.ProductName", "Product Name can't be empty");
        }
        if (ProductFormModel.Description == null || string.IsNullOrEmpty(ProductFormModel.Description.Trim()))
        {
            ModelState.AddModelError("ProductFormModel.Description", "Product description should not be empty");
        }
        if (ProductFormModel.Price <= 0)
        {
            ModelState.AddModelError("ProductFormModel.Price", "Product should cost more than zero");
        }
        if (!ModelState.IsValid)
        {
            return Page();
            // return
        }
        else
        {
            var client = _httpClientFactory.CreateClient("Product");

            var updated = await client.PatchAsJsonAsync<Product>($"Product/{Product.ProductId}", ProductFormModel);
            if (!updated.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Failed to update the product.");
                return Page();
            }

            return RedirectToPage("/Catalogue/Index");
        }
    }
}