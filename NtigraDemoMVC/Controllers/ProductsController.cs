using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NtigraDemoMVC.Data;
using NtigraDemoMVC.Models;
using NtigraDemoMVC.Models.Domain;

namespace NtigraDemoMVC.Controllers
{
    public class ProductsController : Controller
    {
        private NtigraDemoDBContext _dbContext { get; }
        public ProductsController(NtigraDemoDBContext dbContext) {
            _dbContext = dbContext;
        }


        /// <summary>
        /// This action is called for showing list of products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _dbContext.Products.Select(p => new ProductViewModel()
            {
                Id = p.Id,
                Created = p.Created,
                Description = p.Description,
                Discount = p.Discount,
                Name = p.Name,
                Price = p.Price
            }).ToListAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// This action will be called when user clicked on add new product
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(ProductViewModel productRequest)
        {
            if (productRequest != null)
            {
                await _dbContext.Products.AddAsync(new Product()
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.Now,
                    Name = productRequest.Name,
                    Description = productRequest.Description,
                    Discount = productRequest.Discount,
                    Price = productRequest.Price,
                });
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// THis Action is called when user click on edit product button
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _dbContext.Products.Where(product => product.Id == id).AsNoTracking().Select(p => new ProductViewModel()
            {
                Id = p.Id,
                Created = p.Created,
                Description = p.Description,
                Discount = p.Discount,
                Name = p.Name,
                Price = p.Price
            }).SingleOrDefaultAsync();

            //If Product is not available redirect to product list
            if(product == null)
            {
                return RedirectToAction("Index");
            }

            return View(product);
        }


        /// <summary>
        /// Save Product to the database
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel productRequest)
        {
            var product = await _dbContext.Products.Where(s => s.Id == productRequest.Id).FirstOrDefaultAsync();
            product.Name = productRequest.Name;
            product.Description = productRequest.Description;
            product.Discount = productRequest.Discount;
            product.Price = productRequest.Price;
            
            _dbContext.Entry<Product>(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// This Action is used to Delete the product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                _dbContext.Entry<Product>(new Product()
                {
                    Id = id
                }).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();

                return Json(new { success = true, message = "Item deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }


        }     
    }
}
