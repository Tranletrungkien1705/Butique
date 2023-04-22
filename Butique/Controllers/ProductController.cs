using Butique.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Butique.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Butique.Controllers
{
    public class ProductController : Controller
    {
        private DbConnection Db;
        public ProductController(DbConnection db)
        {
            Db = db;
        }
        public ActionResult Index()
        {
            List<Product> products = Db.Product.Include(x=>x.CategoryEntity).ToList();
            return View(products);
        }
        [HttpGet]
        public IActionResult Index(string sortOrder,string? q)
        {
            List<Product> products = Db.Product.Include(x => x.CategoryEntity).ToList();
            ViewData["SortByName"] = (String.IsNullOrEmpty(sortOrder)) ? "SortByNameASC":"";
            ViewData["SortByCode"] = (String.IsNullOrEmpty(sortOrder)) ? "SortByCodeASC" : "";
            switch (sortOrder)
            {
                case "SortByNameASC": products=products.OrderBy(x=>x.Name).ToList(); break;
                case "SortByCodeASC": products = products.OrderBy(x => x.Code).ToList(); break;
                default: products=products.OrderByDescending(x=>x.Name).ToList();break;
            }
            List<Product> b = new List<Product>();
            if (!String.IsNullOrEmpty(q))
            {
                ViewData["Keyword"] = q;
                string[] each = q.Split(' ');
                if (each.Length > 0)
                {
                    foreach (string a in each)
                    {
                        products.Where(x => x.Name.ToLower().Contains(a.ToLower()))
                       .ToList().ForEach(z => b.Add(z));
                    }
                    HashSet<Product> products1 = new HashSet<Product>();
                    b.ForEach(z => products1.Add(z));
                    products = new List<Product>();
                    foreach (Product cat in products1)
                    {
                        products.Add(cat);
                    }
                }
            }
            return View(products);
        }
        public ActionResult Add()
        {
            ViewData["CategoryId"] = new SelectList(Db.Category,"Id","Name");
            return View("Add");
        }
        [HttpPost]
        public ActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                Db.Product.Add(product);
                Db.SaveChanges();
                return RedirectToAction(nameof(Index));

            }

            ViewData["CategoryId"] = new SelectList(Db.Category, "Id", "Name",product.CategoryId);
            return View(nameof(Add));
        }
        public ActionResult Delete(int? id)
        {
            Product cat = Db.Product.Find(id);
            if (cat != null)
            {
                Db.Product.Remove(cat);
                Db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Edit(int id)
        {
            Product cat = Db.Product.Find(id);
            ViewData["CategoryId"] = new SelectList(Db.Category, "Id", "Name");
            return View(cat);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                Db.Product.Update(product);
                Db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(Db.Category, "Id", "Name", product.CategoryId);
            return View(nameof(Edit));
        }
    }
}
