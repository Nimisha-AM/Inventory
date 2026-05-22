using inventory.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Controllers;

public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var totalProducts = await _context.Products.CountAsync();
        var totalVariants = await _context.Variants.CountAsync();
        var totalStock = await _context.Stocks.SumAsync(x => x.Quantity);

        ViewBag.TotalProducts = totalProducts;
        ViewBag.TotalVariants = totalVariants;
        ViewBag.TotalStock = totalStock;

        return View();
    }
}