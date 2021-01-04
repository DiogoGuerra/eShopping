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

        // GET: Orders
        public ActionResult Index()
        {
            var pedidos = db.Pedidos.Include(o => o.Entrega);
            return View(pedidos.ToList());
        }

        public ActionResult UserOrders()
        {
            string userid = User.Identity.GetUserId();
            var pedidos = db.Pedidos.Include(o => o.Entrega).Where(p => p.ID_Cliente  == userid).Where(p => p.PedidoEmAberto == true);
            return View(pedidos.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FUserOrders([Bind(Include = "OrderID,ID_Cliente,Data_Venda,EstaFinalizado,EntregaID")] Order order)
        {
           
            if (ModelState.IsValid)
            {
                order.PedidoEmAberto = false;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EntregaID = new SelectList(db.Entregas, "ID", "Tipo", order.EntregaID);
            return View(order);
        }

        public ActionResult OrdersHistory()
        {
            //fALTA LISTAR OS PRODUTOS
            string userid = User.Identity.GetUserId();
            var pedidos = db.Pedidos.Include(o => o.Entrega).Where(p => p.ID_Cliente == userid).Where(c => c.PedidoEmAberto == false);
            return View(pedidos.ToList());
        }

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
        public ActionResult Edit([Bind(Include = "OrderID,ID_Cliente,Data_Venda,EstaFinalizado,EntregaID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EntregaID = new SelectList(db.Entregas, "ID", "Tipo", order.EntregaID);
            return View(order);
        }

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
