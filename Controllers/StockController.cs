using inventory.Data;
using inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace inventory.Controllers
{
    public class StockController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StockController> _logger;

        public StockController(
            AppDbContext context,
            ILogger<StockController> logger)
        {
            _context = context;
            _logger = logger;
        }

     
        public IActionResult Index()
        {
            var data = _context.Stocks
                .Include(x => x.Variant)
                .ThenInclude(v => v.Product)
                .ToList();

            return View(data);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Variants = _context.Variants
                .Include(x => x.Product)
                .ToList();

            return View();
        }

 
        [HttpPost]
        public IActionResult Add(int variantId, int qty)
        {
            try
            {
          
                if (qty <= 0)
                {
                    ViewBag.Error = "Quantity must be greater than zero";

                    ViewBag.Variants = _context.Variants
                        .Include(x => x.Product)
                        .ToList();

                    return View();
                }

  
                var variantExists = _context.Variants
                    .Any(x => x.Id == variantId);

                if (!variantExists)
                {
                    ViewBag.Error = "Invalid Variant";

                    ViewBag.Variants = _context.Variants
                        .Include(x => x.Product)
                        .ToList();

                    return View();
                }

                var stock = _context.Stocks
                    .FirstOrDefault(x => x.VariantId == variantId);

   
                if (stock == null)
                {
                    stock = new Stock
                    {
                        VariantId = variantId,
                        Quantity = qty
                    };

                    _context.Stocks.Add(stock);
                }
                else
                {
          
                    stock.Quantity += qty;
                }

                _context.SaveChanges();

                _logger.LogInformation(
                    "Stock added successfully for VariantId: {Id}",
                    variantId);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding stock");

                ViewBag.Error = "Something went wrong";

                ViewBag.Variants = _context.Variants
                    .Include(x => x.Product)
                    .ToList();

                return View();
            }
        }

        [HttpPost]
        public IActionResult Remove(int variantId, int qty)
        {
            try
            {
                var stock = _context.Stocks
                    .FirstOrDefault(x => x.VariantId == variantId);

                if (stock == null)
                {
                    return BadRequest("Stock not found");
                }

                if (qty <= 0)
                {
                    return BadRequest("Quantity must be greater than zero");
                }

            
                if (stock.Quantity < qty)
                {
                    return BadRequest("Stock cannot become negative");
                }

                stock.Quantity -= qty;

                _context.SaveChanges();

                _logger.LogInformation(
                    "Stock removed successfully for VariantId: {Id}",
                    variantId);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while removing stock");

                return RedirectToAction("Index");
            }
        }
    }
}