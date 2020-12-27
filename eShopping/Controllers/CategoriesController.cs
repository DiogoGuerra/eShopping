using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eShopping.Models;

namespace eShopping.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categorias.Where(c => c.EstaEliminado == false).OrderBy(c => c.Nome_Categoria).ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categorias.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome_Categoria")] Category category)
        {
            if (NameCategoryRepeted(category))
            {
                if (!CategoryIsEliminated(category))
                {
                    ModelState.AddModelError("Nome_Categoria", "This name already exists!");
                }
                else 
                {
                    category.EstaEliminado = false;
                }
            }

            if (ModelState.IsValid)
            {
                category.EstaEliminado = false;
                db.Categorias.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categorias.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome_Categoria")] Category category)
        {
            if (NameCategoryRepeted(category))
            {
                ModelState.AddModelError("Nome_Categoria", "This name already exists!");
            }
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categorias.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Category category = db.Categorias.Include("Produtos").FirstOrDefault(c => c.ID == id);
            
            if(category == null)
                return RedirectToAction("Index");

            foreach (var i in category.Produtos)
                i.CategoriaID = null;

            if (category.EstaEliminado == false)
                category.EstaEliminado = true;
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [NonAction]
        private bool NameCategoryRepeted(Category categoria)
        {
            if (db.Categorias.Where(n => n.Nome_Categoria == categoria.Nome_Categoria).FirstOrDefault() == null)
                return false;
            return true;
        }
        [NonAction]
        private bool CategoryIsEliminated(Category categoria)
        {
            if (categoria.EstaEliminado == false)
                return false;
            return true;
        }
    }
}
