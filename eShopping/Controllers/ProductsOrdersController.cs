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

        [Authorize(Roles = RoleName.Admin)]
        // GET: ProductsOrders
        public ActionResult Index()
        {
            var produtosPedidos = db.ProdutosPedidos.Include(p => p.Pedido).Include(p => p.Produto);
            return View(produtosPedidos.ToList());
        }
        [Authorize(Roles = RoleName.User)]
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

        [Authorize(Roles = RoleName.Admin)]
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
        [Authorize(Roles = RoleName.Admin)]
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
        [Authorize(Roles = RoleName.User)]
        // GET: ProductsOrders/Edit/5
        public ActionResult Edit(int? id, int? idp)
        {
            ProductsOrder aux = null;
            if (id == null && idp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ProductsOrder productsOrder = db.ProdutosPedidos.Find(id,idp);
            foreach (var i in db.ProdutosPedidos)
            {
                if (i.ProductID == idp && i.OrderID == id)
                    aux = i;
            }
            if (aux == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Pedidos, "OrderID", "ID_Cliente", aux.OrderID);
            ViewBag.ProductID = new SelectList(db.Produtos, "ProductID", "Nome_Produto", aux.ProductID);
            return View(aux);
        }

        // POST: ProductsOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,OrderID,Quantidade,Preco_Produto")] ProductsOrder productsOrder)
        {
            int quantidade_anterior = 0;
            foreach (var i in db.ProdutosPedidos)
            {
                if(i.ProductID == productsOrder.ProductID && i.OrderID == productsOrder.OrderID)
                {
                    quantidade_anterior = i.Quantidade;
                }
            }
            foreach (var i in db.Produtos)
            {
                if (productsOrder.ProductID == i.ProductID)
                {   
                    if(productsOrder.Quantidade== 0)
                    {
                        ModelState.AddModelError("Quantidade", "Instead, please remove the product from the cart");
                    }
                    if(quantidade_anterior < productsOrder.Quantidade)
                    {
                        if(i.Stock < productsOrder.Quantidade)
                        {
                            ModelState.AddModelError("Quantidade", "Quantity invalid!");
                        }
                    }
                    else
                    {
                        if (i.Stock + quantidade_anterior < productsOrder.Quantidade)
                        {
                            ModelState.AddModelError("Quantidade", "Quantity invalid!");
                        }
                    }
                    
                }
            }
            if (ModelState.IsValid)
            {
                double aux = 0;
                double aux_tot = 0;
                foreach (var i in db.Produtos)
                {
                    if (productsOrder.ProductID == i.ProductID)
                    {
                        i.Stock += quantidade_anterior;
                        i.Stock -= productsOrder.Quantidade;
                        aux_tot = quantidade_anterior * (i.Preco_Produto *(1-i.Taxa_Promocao));
                        aux = (i.Preco_Produto * (1 - i.Taxa_Promocao)) * productsOrder.Quantidade;
                    }
                }
                foreach(var i in db.Pedidos)
                {
                    if(i.OrderID == productsOrder.OrderID)
                    {
                        i.Preco_Total -= aux_tot;
                        i.Preco_Total += aux;
                    } 
                }
                foreach (var i in db.ProdutosPedidos)
                {
                    if (i.ProductID == productsOrder.ProductID && i.OrderID == productsOrder.OrderID)
                    {
                        i.Quantidade = productsOrder.Quantidade;
                    }
                }
                //db.Entry(productsOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Cart");
            }
            ViewBag.OrderID = new SelectList(db.Pedidos, "OrderID", "ID_Cliente", productsOrder.OrderID);
            ViewBag.ProductID = new SelectList(db.Produtos, "ProductID", "Nome_Produto", productsOrder.ProductID);
            return View(productsOrder);
        }
        [Authorize(Roles = RoleName.User)]
        // GET: ProductsOrders/Delete/5
        public ActionResult Delete(int? id, int? idp)
        {
            ProductsOrder aux = null;
            if (id == null && idp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ProductsOrder productsOrder = db.ProdutosPedidos.Find(id,idp);
            foreach (var i in db.ProdutosPedidos)
            {
                if (i.ProductID == idp && i.OrderID == id)
                    aux = i;
            }
            if (aux == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Pedidos, "OrderID", "ID_Cliente", aux.OrderID);
            ViewBag.ProductID = new SelectList(db.Produtos, "ProductID", "Nome_Produto", aux.ProductID);
            return View(aux);
        }

        // POST: ProductsOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id,int? idp)
        {
            int np = 0;
            ProductsOrder aux = null;
            foreach (var i in db.ProdutosPedidos)
            {
                if(i.OrderID == id)
                {
                    np++;
                }
                if (i.ProductID == idp && i.OrderID == id)
                    aux = i;
            }
            foreach (var i in db.Produtos)
            {
                if(i.ProductID == aux.ProductID)
                {
                    i.Stock += aux.Quantidade;
                }
            }
            db.ProdutosPedidos.Remove(aux);
            db.SaveChanges();
            int cont = 0;
            var pedi = db.ProdutosPedidos.Include(c => c.Pedido).Where(model => model.Pedido.PedidoEmAberto == true);
            foreach(var i in pedi)
            {
                if(i.Pedido == aux.Pedido)
                {
                    cont++;
                }
            }
            if (cont == 0)
            {
                foreach (var i in db.Pedidos)
                {
                    if (i.OrderID == aux.OrderID && np < 2)
                    {
                        i.PedidoEmAberto = false;
                        db.Pedidos.Remove(i);
                    }
                }
            }
            db.SaveChanges();
            return RedirectToAction("Cart");
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
