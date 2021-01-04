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
    public class ProductsOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProductsOrders
        public ActionResult Index()
        {
            var produtosPedidos = db.ProdutosPedidos.Include(p => p.Pedido).Include(p => p.Produto);
            return View(produtosPedidos.ToList());
        }

        public ActionResult Cart()
        {
            var UserID = User.Identity.GetUserId();
            var produtosPedidos = db.ProdutosPedidos.Include(p => p.Pedido).Where(p => p.Pedido.PedidoEmAberto == true).Include(p => p.Produto).Where(p => p.Pedido.ID_Cliente == UserID);
            return View(produtosPedidos.ToList());
        }

        public ActionResult OrderDetails(int id)
        {
            //var order = db.Pedidos.Find(id);
            var produtos = db.ProdutosPedidos.Include(p => p.Produto).Include(p => p.Pedido).Where(p => p.Pedido.OrderID == id);
            return View(produtos.ToList());
        }


        // GET: ProductsOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsOrder productsOrder = db.ProdutosPedidos.Find(id);
            if (productsOrder == null)
            {
                return HttpNotFound();
            }
            return View(productsOrder);
        }

        // GET: ProductsOrders/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(db.Pedidos, "OrderID", "ID_Cliente");
            ViewBag.ProductID = new SelectList(db.Produtos, "ProductID", "Nome_Produto");
            return View();
        }

        // POST: ProductsOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,OrderID,Quantidade,Preco_Produto")] ProductsOrder productsOrder)
        {
            if (ModelState.IsValid)
            {
                db.ProdutosPedidos.Add(productsOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Pedidos, "OrderID", "ID_Cliente", productsOrder.OrderID);
            ViewBag.ProductID = new SelectList(db.Produtos, "ProductID", "Nome_Produto", productsOrder.ProductID);
            return View(productsOrder);
        }

        // GET: ProductsOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsOrder productsOrder = db.ProdutosPedidos.Find(id);
            if (productsOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Pedidos, "OrderID", "ID_Cliente", productsOrder.OrderID);
            ViewBag.ProductID = new SelectList(db.Produtos, "ProductID", "Nome_Produto", productsOrder.ProductID);
            return View(productsOrder);
        }

        // POST: ProductsOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,OrderID,Quantidade,Preco_Produto")] ProductsOrder productsOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productsOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Pedidos, "OrderID", "ID_Cliente", productsOrder.OrderID);
            ViewBag.ProductID = new SelectList(db.Produtos, "ProductID", "Nome_Produto", productsOrder.ProductID);
            return View(productsOrder);
        }

        // GET: ProductsOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsOrder productsOrder = db.ProdutosPedidos.Find(id);
            if (productsOrder == null)
            {
                return HttpNotFound();
            }
            return View(productsOrder);
        }

        // POST: ProductsOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductsOrder productsOrder = db.ProdutosPedidos.Find(id);
            db.ProdutosPedidos.Remove(productsOrder);
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
