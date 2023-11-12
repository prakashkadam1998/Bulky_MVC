using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
       private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db; 
        }
        public IActionResult Index()
        {
            List<Category> categoryObj = _db.Categories.ToList();
            return View(categoryObj);
        }

        public IActionResult Create()
        {
            return View(); //can pass other View name as well
           // return View(new Category());
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            //custom Validation
            if(category.Name == category.DisplayOrder.ToString())
            { 
                ModelState.AddModelError("name", "The Display Order cannot be exactly match Name"); 
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Category Created Successfully.";
                return RedirectToAction("Index");
                //  return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            Category categoryFromDb = _db.Categories.Find(id);
            // Category? categoryFromDb = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category categoryFromDb = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (categoryFromDb == null)
                return NotFound();
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["success"] = "Category Updated Successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            Category? categoryFromDb = _db.Categories.Find(id);
            
            if (categoryFromDb == null)
                return NotFound();
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? category = _db.Categories.Find(id);
            if (category == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                _db.Categories.Remove(category);
                _db.SaveChanges();
                TempData["success"] = "Category Deleted Successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
