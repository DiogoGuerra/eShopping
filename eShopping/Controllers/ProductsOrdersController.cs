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
                foreach (var i in db.Produtos)
                {
                    if (productsOrder.ProductID == i.ProductID)
                    {
                        i.Stock += quantidade_anterior;
                        i.Stock -= productsOrder.Quantidade;
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
            ProductsOrder aux = null;
            foreach (var i in db.ProdutosPedidos)
            {
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


        //---------------------------------------------------------------------------
        public ActionResult ButtonAddCart1(int id)
        {
            ProductsOrder produtoPedido = new ProductsOrder
            {
                ProductID = id
            };

            return View(produtoPedido);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ButtonAddCart1([Bind(Include = "ProductID,OrderID,Preco_Produto,Quantidade")] ProductsOrder ProdPedido)
        {
            //Products produto = new Products();
            Products produto = db.Produtos.Find(ProdPedido.ProductID);
            Order aux = null;
            if (produto.Stock < ProdPedido.Quantidade)
            {
                ModelState.AddModelError("Quantidade", "This company already have this product!");
            }
            if (ModelState.IsValid)
            {
                int flag = 0;
                //ProdPedido.ProductID = produto.ProductID;
                var pedi = db.Pedidos.Where(p => p.PedidoEmAberto == true)
                                     .Include(p => p.Empresa)/*.Where(p => p.PedidoEmAberto ==true)*/;

                var existePedidoParaEmpresa = pedi.Any(p => p.Empresa.CompanyId == produto.CompanyId);

                foreach (var i in pedi)
                {
                    // Se encontrar algum pedido da empresa
                    if (i.Empresa.CompanyId == produto.Company.CompanyId)
                    {
                        //Se o pedido tiver aberto, adicionamos
                        if (i.PedidoEmAberto == true)
                        {
                            aux = i;
                            Order ped = i;
                            ProdPedido.Pedido = ped;
                            flag = 2; //verificar se o produto ja existe nos pedidos em aberto
                        }
                        //Caso contrario criamos um 
                        else
                        {
                            //Order novopedido = new Order();
                            //Company empresa = db.Empresas.Find(produto.ID_Empresa);
                            //novopedido.Empresa = empresa;
                            //novopedido.ID_Cliente = User.Identity.GetUserId();
                            //novopedido.Preco_Total += produto.Preco_Produto * ProdPedido.Quantidade;
                            //novopedido.PedidoEmAberto = true;
                            //novopedido.Data_Venda = DateTime.Now;
                            //novopedido.EntregaID = 2;
                            //ProdPedido.OrderID = novopedido.OrderID;
                            //ProdPedido.Preco_Produto = produto.Preco_Produto;
                            //db.Pedidos.Add(novopedido);
                            //db.ProdutosPedidos.Add(ProdPedido);
                            flag = 0;
                        }
                    }
                }
                //Se nao houver nenhum pedido da empresa desejada
                if (flag == 0)
                {
                    Order novopedido = new Order();
                    Company empresa = db.Empresas.Find(produto.CompanyId);
                    novopedido.Empresa = empresa;
                    novopedido.ID_Cliente = User.Identity.GetUserId();
                    novopedido.Preco_Total += produto.Preco_Produto * ProdPedido.Quantidade;
                    novopedido.PedidoEmAberto = true;
                    novopedido.Data_Venda = DateTime.Now;
                    novopedido.EntregaID = 2;
                    ProdPedido.OrderID = novopedido.OrderID;
                    ProdPedido.Preco_Produto = produto.Preco_Produto;
                    db.Pedidos.Add(novopedido);
                    db.ProdutosPedidos.Add(ProdPedido);
                }
                if (flag == 2)
                {
                    int existe = 0;
                    var prodped = db.ProdutosPedidos.Include(p => p.Produto);
                    foreach (var d in prodped)
                    {
                        if (d.Produto == produto)
                        {
                            existe = 1;
                            d.Quantidade += ProdPedido.Quantidade;
                            d.Pedido.Preco_Total += produto.Preco_Produto * ProdPedido.Quantidade;
                            db.Entry(d).State = EntityState.Modified;
                        }
                    }
                    if (existe == 0)
                    {
                        ProdPedido.Produto = produto;
                        ProdPedido.Preco_Produto = produto.Preco_Produto;
                        ProdPedido.Pedido.Preco_Total += produto.Preco_Produto * ProdPedido.Quantidade;
                        db.ProdutosPedidos.Add(ProdPedido);
                        //Temos de adicionar o produto se nao houver o que ele quer adicionar

                    }
                }

                produto.Stock -= ProdPedido.Quantidade;
                db.Entry(produto).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("ListCostumerProducts", "Products");
        }
    }
}
