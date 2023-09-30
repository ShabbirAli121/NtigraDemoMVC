using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NtigraDemoMVC.Data;
using NtigraDemoMVC.Models;
using System.Diagnostics;
using System.Text.Json;

namespace NtigraDemoMVC.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly ILogger<PurchaseController> _logger;
        private readonly NtigraDemoDBContext _dbContext;
        private readonly IConfiguration _configuration;

        public PurchaseController(ILogger<PurchaseController> logger, NtigraDemoDBContext dbContext, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var cart = new CartViewModel();
            cart.PurchasedProducts = await _dbContext.Products.Select(p => new PurchaseItemViewModel()
            {

                Id = p.Id,
                Created = p.Created,
                Description = p.Description,
                Discount = p.Discount,
                Name = p.Name,
                Price = p.Price,
                IsSelected = false
            }).ToListAsync();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(string modifiedData)
        {
            var modifiedDataDict = JsonSerializer.Deserialize<Dictionary<int, PurchaseItemViewModel>>(modifiedData);
            var cart = new CartViewModel();
            cart.PurchasedProducts = new List<PurchaseItemViewModel>();
            var purchasedProducts = await _dbContext.Products.Select(p => new PurchaseItemViewModel()
            {

                Id = p.Id,
                Created = p.Created,
                Description = p.Description,
                Discount = p.Discount,
                Name = p.Name,
                Price = p.Price,
                IsSelected = false
            }).ToListAsync();

            for (int i = 0; i< purchasedProducts.Count; i++)
            {
                foreach(var key in modifiedDataDict.Keys)
                {
                    if(key == i)
                    {
                        purchasedProducts[i].Quantity = modifiedDataDict[key].Quantity;
                        purchasedProducts[i].IsSelected = modifiedDataDict[key].IsSelected;
                        cart.PurchasedProducts.Add(purchasedProducts[i]);
                    }
                }
            }


            var discountCap = _configuration.GetValue<double>("DiscountOnPriceCap");

            //Set Discount cap
            if (cart.TotalPrice > discountCap)
                cart.IsEligibleForDiscount = true;

            return View(cart);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}