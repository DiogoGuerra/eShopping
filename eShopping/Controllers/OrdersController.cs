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
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = RoleName.Admin)]
        // GET: Orders
        public ActionResult Index()
        {
            var pedidos = db.Pedidos.Include(o => o.Entrega);
            return View(pedidos.ToList());
        }
        [Authorize(Roles = RoleName.User)]
        public ActionResult UserOrders()
        {
            string userid = User.Identity.GetUserId();
            var pedidos = db.Pedidos.Include(o => o.Entrega).Where(p => p.ID_Cliente  == userid).Where(p => p.PedidoEmAberto == true);
            return View(pedidos.ToList());
        }
        [Authorize(Roles = RoleName.Employee)]
        public ActionResult ListOrdersEmployee()
        {
            string employeeid = User.Identity.GetUserId();
            var employee = db.Users.FirstOrDefault(u => u.Id == employeeid);
            var emp = employee.CompanyId;
            Company aux = null;
            int idc = 0;
            foreach(var i in db.Empresas)
            {
                if(i.CompanyId == emp)
                {
                    aux = i;
                    idc = i.CompanyId;
                }
            }
            var pedidos = db.Pedidos.Include(o => o.Entrega).Include(e => e.Empresa).Include(c => c.Estado).Where(p => p.PedidoEmAberto == false).Where(c => c.Empresa.CompanyId == idc);
            
            return View(pedidos.ToList());
        }
        [Authorize(Roles = RoleName.Employee)]
        public ActionResult ListOrdersPending()
        {
            string employeeid = User.Identity.GetUserId();
            var employee = db.Users.FirstOrDefault(u => u.Id == employeeid);
            var emp = employee.CompanyId;
            Company aux = null;
            int idc = 0;
            foreach (var i in db.Empresas)
            {
                if (i.CompanyId == emp)
                {
                    aux = i;
                    idc = i.CompanyId;
                }
            }
            var pedidos = db.Pedidos.Include(o => o.Entrega).Include(e => e.Empresa).Include(c => c.Estado).Where(p => p.PedidoEmAberto == false).Where(c => c.Empresa.CompanyId == idc).Where(c => c.Estado.ID != 3);

            return View(pedidos.ToList());
        }

        //[ValidateAntiForgeryToken]
        public ActionResult FUserOrders()
        {
            Status est = null;
           foreach(var i in db.Estados) {
                if (i.Descricao == "Pending")
                {
                    est = i;
                }
            }
            foreach ( var i in db.Pedidos) {
                if(i.PedidoEmAberto == true && i.ID_Cliente == User.Identity.GetUserId())
                {
                    i.Estado = est;
                    i.PedidoEmAberto = false; 
                }
            }
            db.SaveChanges();
            return RedirectToAction("ListCostumerProducts","Products");
        }
        [Authorize(Roles = RoleName.User)]
        public ActionResult OrdersHistory()
        {
            //fALTA LISTAR OS PRODUTOS
            string userid = User.Identity.GetUserId();
            var pedidos = db.Pedidos.Include(e => e.Estado).Include(o => o.Entrega).Where(p => p.ID_Cliente == userid).Where(c => c.PedidoEmAberto == false);
            return View(pedidos.ToList());
        }
        [Authorize(Roles = RoleName.Admin)]
        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Pedidos.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        [Authorize(Roles = RoleName.Admin)]
        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.EntregaID = new SelectList(db.Entregas, "ID", "Tipo");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,ID_Cliente,Data_Venda,EstaFinalizado,EntregaID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Pedidos.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EntregaID = new SelectList(db.Entregas, "ID", "Tipo", order.EntregaID);
            return View(order);
        }
        [Authorize(Roles = RoleName.AdminOrUser)]
        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Pedidos.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.EntregaID = new SelectList(db.Entregas, "ID", "Tipo", order.EntregaID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,ID_Cliente,Data_Venda,EntregaID,Preco_Total,EstadoID,Estado")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.PedidoEmAberto = true;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserOrders");
            }
            ViewBag.EntregaID = new SelectList(db.Entregas, "ID", "Tipo", order.EntregaID);
            return View(order);
        }
        [Authorize(Roles = RoleName.Employee)]
        public ActionResult EditStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Pedidos.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.EstadoID = new SelectList(db.Estados, "ID", "Descricao", order.EstadoID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStatus([Bind(Include = "OrderID,ID_Cliente,Data_Venda,EntregaID,Preco_Total,EstadoID")] Order order)
        {
            if (ModelState.IsValid)
            {
                int est = order.EstadoID;
                foreach(var i in db.Estados)
                {
                    if(i.ID == est)
                    {
                        order.Estado = i;
                    }
                }
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListOrdersPending");
            }
            ViewBag.Estado = new SelectList(db.Estados, "ID", "Descricao", order.Estado);
            return View(order);
        }
        [Authorize(Roles = RoleName.Admin)]
        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Pedidos.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Pedidos.Find(id);
            db.Pedidos.Remove(order);
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
    }
}
