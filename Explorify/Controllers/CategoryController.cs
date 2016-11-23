using Explorify.Web.Models;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Explorify.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        readonly ApplicationDbContext _db = new ApplicationDbContext();
        //
        // GET: /Category/
        public async Task<ActionResult> Index()
        {
            return View(await _db.Categories.ToListAsync());
        }

        //
        // GET: /Category/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);

            return View(category);
        }

        //
        // GET: /Category/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            //ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View(new CategoryViewModel());
        }

        //
        // POST: /Category/Create
        [HttpPost]
        public async Task<ActionResult> Create(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Id = SqlGuidUtil.NewSequentialId(),
                    CategoryName = categoryViewModel.CategoryName,
                    IsActive = categoryViewModel.IsActive
                };
                

                // Then create:
                _db.Categories.Add(category);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        //
        // GET: /Category/Edit/1
        public async Task<ActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = await _db.Categories.FirstOrDefaultAsync(c=>c.Id == id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(new CategoryViewModel()
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                IsActive = category.IsActive
            });
        }

        //
        // POST: /Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryViewModel editcategory)
        {
            if (ModelState.IsValid)
            {
                var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == editcategory.Id);
                if (category == null)
                {
                    return HttpNotFound();
                }

                category.CategoryName = editcategory.CategoryName;
                category.IsActive = editcategory.IsActive;
                _db.Entry(category).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View(editcategory);
        }

        //
        // GET: /Category/Delete/5
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var category = await _db.Categories.FirstOrDefaultAsync(c=>c.Id == id);
                if (category == null)
                {
                    return HttpNotFound();
                }

                _db.Categories.Remove(category);

                
                await _db.SaveChangesAsync();
               
                return RedirectToAction("Index");
            }
            return View();
        }
	}
}