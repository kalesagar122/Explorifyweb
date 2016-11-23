using Explorify.Web.Models;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Explorify.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SkillController : Controller
    {
        readonly ApplicationDbContext _db = new ApplicationDbContext();
        //
        // GET: /Skill/
        public async Task<ActionResult> Index()
        {
            return View(await _db.Skills.ToListAsync());
        }

        //
        // GET: /Skill/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var skill = await _db.Skills.FirstOrDefaultAsync(c => c.SkillId == id);

            return View(skill);
        }

        //
        // GET: /Skill/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            //ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View(new SkillViewModel());
        }

        //
        // POST: /Skill/Create
        [HttpPost]
        public async Task<ActionResult> Create(SkillViewModel skillViewModel)
        {
            if (ModelState.IsValid)
            {
                var skill = new Skill
                {
                    SkillId = SqlGuidUtil.NewSequentialId(),
                    SkillName = skillViewModel.SkillName,
                    IsActive = skillViewModel.IsActive
                };
                

                // Then create:
                _db.Skills.Add(skill);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        //
        // GET: /Skill/Edit/1
        public async Task<ActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var skill = await _db.Skills.FirstOrDefaultAsync(c=>c.SkillId == id);
            if (skill == null)
            {
                return HttpNotFound();
            }

            return View(new SkillViewModel()
            {
                SkillId = skill.SkillId,
                SkillName = skill.SkillName,
                IsActive = skill.IsActive
            });
        }

        //
        // POST: /Skill/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SkillViewModel editSkill)
        {
            if (ModelState.IsValid)
            {
                var skill = await _db.Skills.FirstOrDefaultAsync(c => c.SkillId == editSkill.SkillId);
                if (skill == null)
                {
                    return HttpNotFound();
                }

                skill.SkillName = editSkill.SkillName;
                skill.IsActive = editSkill.IsActive;
                _db.Entry(skill).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View(editSkill);
        }

        //
        // GET: /Skill/Delete/5
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var skill = await _db.Skills.FirstOrDefaultAsync(c => c.SkillId == id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            return View(skill);
        }

        //
        // POST: /Skill/Delete/5
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

                var skill = await _db.Skills.FirstOrDefaultAsync(c=>c.SkillId == id);
                if (skill == null)
                {
                    return HttpNotFound();
                }

                _db.Skills.Remove(skill);

                
                await _db.SaveChangesAsync();
               
                return RedirectToAction("Index");
            }
            return View();
        }
	}
}