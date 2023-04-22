using Butique.Data;
using Microsoft.AspNetCore.Mvc;
using Butique.Models.Entity;
using X.PagedList;

namespace Butique.Controllers
{
    public class CategoryController : Controller
    { 
        private DbConnection Db;
        public CategoryController(DbConnection Db)
        {
            this.Db = Db;
        }
        [HttpGet]
        public IActionResult Index(string? q,int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 2;
            List<Category> categories = Db.Category.ToList();
            List<Category> b = new List<Category>();
            if (!String.IsNullOrEmpty(q))
            {
                ViewData["Keyword"] = q;
                string[] each = q.Split(' ');
                if(each.Length > 0)
                {
                    foreach (string a in each)
                    {
                        categories.Where(x => x.Name.ToLower().Contains(a.ToLower()))
                       .ToList().ForEach(z => b.Add(z));
                    }
                    HashSet<Category>  categori = new HashSet<Category>();
                    b.ForEach(z => categori.Add(z));
                    categories = new List<Category>();
                    foreach(Category cat in categori)
                    {
                        categories.Add(cat);
                    }
                }
            }
            return View(categories.ToPagedList(pageNumber,pageSize));
        }

        //public bool IsExisted(string id)
        //{
        //    bool Flag = false;
        //    categories.ForEach(t =>
        //    {
        //        if (t.Id.Equals(id))
        //        {
        //            Flag = true;
        //            return;
        //        }
        //    });
        //    return Flag;
        //}
        public ActionResult Add()
        {
            return View("Add");
        }
        [HttpPost]
        public ActionResult Add(Category category)
        {
            if(ModelState.IsValid)
            {
                Db.Category.Add(category);
                Db.SaveChanges();
                return RedirectToAction(nameof(Index));

            }
            return View(nameof(Add));
        }
        public ActionResult Edit(int id)
        {
            Category cat = Db.Category.Find(id);
            return View(cat);
        }
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                Db.Category.Update(category);
                Db.SaveChanges();
                return RedirectToAction(nameof(Index));

            }
            return View(nameof(Edit));
        }
        public ActionResult Delete(int id)
        {
            Category cat = Db.Category.Find(id);
            if(cat != null)
            {
                Db.Category.Remove(cat);
                Db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
