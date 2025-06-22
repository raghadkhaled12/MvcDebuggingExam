using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using MvcDebuggingExam.Models;

namespace MvcDebuggingExam.Controllers
{
    public class ProductsController : Controller
    {
        // Static list to simulate database
        private static List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Description = "Gaming Laptop", Price = 999.99m, Category = "Electronics" },
            new Product { Id = 2, Name = "Mouse", Description = "Wireless Mouse", Price = 29.99m, Category = "Electronics" },
            new Product { Id = 3, Name = "Keyboard", Description = "Mechanical Keyboard", Price = 79.99m, Category = "Electronics" }
        };

        public IActionResult Index()
        {
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            //Question 2 fix 
            //Here is the correction of the details view path. we can transfer the file to view/products/details.cshtml
            return View("~/Views/Home/Details.cshtml",product);
        }
        //Put [Http Get]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            //Change in Question 1 (not put ! before ModelState)
            if (ModelState.IsValid)
            {
                product.Id = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
                products.Add(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingProduct = products.FirstOrDefault(p => p.Id == id);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.Category = product.Category;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpPost]
        //Fix 1 in Question 5
        [ValidateAntiForgeryToken]

        // Fix 1 in Question 4 (Error in the Parameter name inside the Delete method .it should be id not productid , so that it matches the value in the form) 
        public IActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                products.Remove(product);
            }
            return RedirectToAction(nameof(Index));
        }

        // Fix 2 in Question 4 (we need to DeleteConfirmed action)
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    var product = products.FirstOrDefault(p => p.Id == id);
        //    if (product != null)
        //    {
        //        products.Remove(product);
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
