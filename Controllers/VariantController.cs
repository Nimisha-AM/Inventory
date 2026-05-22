using inventory.Data;
using inventory.Models;
using Microsoft.AspNetCore.Mvc;

namespace inventory.Controllers
{
    public class VariantController : Controller
    {
        private readonly AppDbContext _context;

        public VariantController(AppDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var data = _context.Variants.ToList();

            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Products = _context.Products.ToList();

            return View();
        }

  
        [HttpPost]
        public IActionResult Create(int productId, string sizes, string colors)
        {
         
            var productExists = _context.Products.Any(x => x.Id == productId);

            if (!productExists)
            {
                ViewBag.Products = _context.Products.ToList();

                ViewBag.Error = "Invalid Product";

                return View();
            }

            var sizeList = sizes.Split(',');
            var colorList = colors.Split(',');

            foreach (var size in sizeList)
            {
                foreach (var color in colorList)
                {
                    string finalSize = size.Trim();
                    string finalColor = color.Trim();

                    // PREVENT DUPLICATES
                    bool exists = _context.Variants.Any(x =>
                        x.ProductId == productId &&
                        x.Size == finalSize &&
                        x.Color == finalColor);

                    if (!exists)
                    {
                        _context.Variants.Add(new Variant
                        {
                            ProductId = productId,
                            Size = finalSize,
                            Color = finalColor
                        });
                    }
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}