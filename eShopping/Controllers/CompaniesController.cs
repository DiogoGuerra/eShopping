using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eShopping.Models;
using Microsoft.AspNet.Identity;

namespace eShopping.Controllers
{
    public class CompaniesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = RoleName.Admin)]
        // GET: Companies
        public ActionResult Index()
        {
            return View(db.Empresas.Where(e => e.EstaEliminado == false).ToList());
        }
        [Authorize(Roles = RoleName.Admin)]
        // GET: Companies/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Empresas.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userID,Email,Nome")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Empresas.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company);
        }
        [Authorize(Roles = RoleName.Admin)]
        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Empresas.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyId,Email,Nome")] Company company)
        {
            if (NameCompanyRepeted(company))
            {
                ModelState.AddModelError("Nome", "This name already exists!");
            }
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }
        [Authorize(Roles = RoleName.Admin)]
        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Empresas.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company aux = null;
            Company apaga = null;
            var company = db.Empresas.Include(p => p.Produtos);
            foreach (var w in company)
            {
                if (w.CompanyId == id)
                    aux = w;
            }
            if (company == null)
                return RedirectToAction("Index");

            foreach (var i in aux.Produtos)
            {
                i.Company = apaga;
                i.EstaEliminado = true;
            }

            if (aux.EstaEliminado == false)
                aux.EstaEliminado = true;
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
        private bool NameCompanyRepeted(Company empresa)
        {
            if (db.Empresas.Where(e => e.Nome == empresa.Nome).FirstOrDefault() == null)
                return false;
            return true;
        }
        [NonAction]
        private bool NameClientRepeted(ApplicationUser user)
        {
            if (db.Users.Where(e => e.UserName == user.UserName).FirstOrDefault() == null)
                return false;
            return true;
        }
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult Listaclientes()
        {

            var usr_role = db.Roles.Single(x => x.Name == "user").Id;
            //var clientes = db.Users.Include(r => r.Roles));
            //var u = clientes.Where(c => c.Roles == usr_role);
            var users = db.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(usr_role)).ToList();
            return View(users);
        }
        [Authorize(Roles = RoleName.Company)]
        public ActionResult Listafuncionarios()
        {
            int aux = 0;
            var UserID = User.Identity.GetUserId();
            foreach (var i in db.Users)
            {
                if (i.Id == UserID)
                {
                    aux = (int)i.CompanyId;
                }
            }

            var usr_role = db.Roles.Single(x => x.Name == "employee").Id;
            //var clientes = db.Users.Include(r => r.Roles));
            //var u = clientes.Where(c => c.Roles == usr_role);
            var users = db.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(usr_role)).Where(u => u.CompanyId == aux).ToList();
            return View(users);
        }
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult EditaCliente(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditaCliente([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnable,LockoutEndDateUtc,LockoutEnable,AcessFailedCount,UserName,CompanyId")] ApplicationUser user)
        {
            if (NameClientRepeted(user))
            {
                ModelState.AddModelError("UserName", "This name already exists!");
            }
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                if (user.CompanyId == null)
                {
                    return RedirectToAction("listaclientes");
                }
                else
                {
                    return RedirectToAction("Listafuncionarios");
                }
            }
            return View(user);
        }
        [Authorize(Roles = RoleName.Company)]
        public ActionResult Estatistica()
        {
            int aux = 0;
            var UserID = User.Identity.GetUserId();
            foreach (var i in db.Users)
            {
                if (i.Id == UserID)
                {
                    aux = (int)i.CompanyId;
                }
            }
            Company co = db.Empresas.Find(aux);
            double soma_valortotal = 0;
            float valordiario = 0;
            int contador_pedidos = 0;
            int contador_pedidos_mes = 0;
            int contadordodia = 0;
            double media_valortotal = 0;
            foreach (var i in db.Pedidos)
            {
                if (i.Empresa == co)
                {
                    if(i.Data_Venda == DateTime.Now)
                    {
                        contadordodia++;
                    }
                    if((i.Data_Venda - DateTime.Now).TotalDays < 31)
                    {
                        contador_pedidos_mes++;
                    }
                    contador_pedidos++;
                    soma_valortotal += i.Preco_Total;
                    
                }

            }
            media_valortotal = soma_valortotal / contador_pedidos;
            valordiario = (float)(contador_pedidos_mes / 31);

            ViewBag.ValorMediaTotal = media_valortotal;
            ViewBag.VendasHoje = contadordodia;
            ViewBag.ValorMedioDeVendasPorDias = valordiario;
            return View();
        }

    }
}


