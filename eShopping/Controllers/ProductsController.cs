using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Products
        public ActionResult Index()
        {
            var produtos = db.Produtos.Include(p => p.Categoria);
            return View(produtos.ToList());
        }

        public ActionResult ListCostumerProducts(string categoria)
        {
            var produtos = db.Produtos.Include(p => p.Categoria);

            if (!string.IsNullOrEmpty(categoria))
                produtos = produtos.Where(p => p.Categoria.Nome_Categoria == categoria);
            return View(produtos.ToList());
        }
        [AllowAnonymous]
        public ActionResult ListAnonymous(string categoria)
        {
            var produtos = db.Produtos.Include(p => p.Categoria);

            if (!string.IsNullOrEmpty(categoria))
                produtos = produtos.Where(p => p.Categoria.Nome_Categoria == categoria);
            return View(produtos.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Para aparecer a categoria nos detalhes temos de incluir
            Products products = db.Produtos.Include(p => p.Categoria).Where(p => p.ProductID == id).SingleOrDefault();
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        public ActionResult ButtonBuy(int id)
        {
            ProductsOrder produtoPedido = new ProductsOrder();

            produtoPedido.ProductID = id;

            return View(produtoPedido);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ButtonBuy([Bind(Include = "ProductID,Quantidade")] ProductsOrder ProdPedido)
        {
            //Products produto = new Products();
            Products produto = db.Produtos.Find(ProdPedido.ProductID);
            if (produto.Stock < ProdPedido.Quantidade)
            {
                ModelState.AddModelError("Quantidade", "This company already have this product!");
            }
            if (ModelState.IsValid)
            {
                int flag = 0;
                //ProdPedido.ProductID = produto.ProductID;
                var pedi = db.Pedidos.Include(p => p.Empresa);
                foreach (var i in pedi)
                {
                    // Se encontrar algum pedido da empresa
                    if (i.Empresa.userID == produto.ID_Empresa)
                    {
                        //Se o pedido tiver aberto, adicionamos
                        if (i.PedidoEmAberto == true)
                        {
                            ProdPedido.OrderID = i.OrderID;
                            ProdPedido.Preco_Produto = produto.Preco_Produto;
                            flag = 2; //verificar se o produto ja existe nos pedidos em aberto
                        }
                        //Caso contrario criamos um 
                        else
                        {
                            Order novopedido = new Order();
                            Company empresa = db.Empresas.Find(produto.ID_Empresa);
                            novopedido.Empresa = empresa;
                            novopedido.ID_Cliente = User.Identity.GetUserId();
                            novopedido.PedidoEmAberto = true;
                            novopedido.Data_Venda = DateTime.Now;
                            novopedido.EntregaID = 2;
                            ProdPedido.OrderID = novopedido.OrderID;
                            ProdPedido.Preco_Produto = produto.Preco_Produto;
                            db.Pedidos.Add(novopedido);
                            db.ProdutosPedidos.Add(ProdPedido);
                            flag = 1;
                        }
                    }
                }
                //Se nao houver nenhum pedido da empresa desejada
                if (flag == 0)
                {
                    Order novopedido = new Order();
                    Company empresa = db.Empresas.Find(produto.ID_Empresa);
                    novopedido.Empresa = empresa;
                    novopedido.ID_Cliente = User.Identity.GetUserId();
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
                            db.Entry(d).State = EntityState.Modified; 
                        }
                    }
                    if (existe == 0)
                    {
                        //Temos de adicionar o produto se nao houver o que ele quer adicionar

                    }
                }

                produto.Stock -= ProdPedido.Quantidade;
                db.Entry(produto).State = EntityState.Modified;
                db.SaveChanges();
            }
            
            return RedirectToAction("ListCostumerProducts");
        }
        public void setupCompanyIdViewBag()
        {
            var roles = db.Roles.Where(r => r.Name == RoleName.Company);
            if (roles.Any())
            {
                var roleId = roles.First().Id;
                var companyIds = db.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId));
                var company = companyIds.First();
                ViewBag.ID_Empresa = new SelectList(companyIds, "ID", "Email");
            }
        }
        // GET: Products/Create
        public ActionResult Create()
        {
            setupCompanyIdViewBag();
            ViewBag.CategoriaID = new SelectList(db.Categorias.Where(c => c.EstaEliminado == false), "ID", "Nome_Categoria");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Nome_Produto,Stock,ID_Empresa,Preco_Produto,EstaNoCatalogo,CategoriaID")] Products products)
        {
            bool flag = false;
            foreach (var i in db.Produtos)
            {
                if (i.Nome_Produto == products.Nome_Produto && i.ID_Empresa == products.ID_Empresa)
                {
                    flag = true;
                    ModelState.AddModelError("ID_Empresa", "This company already have this product!");
                }
            }
            if (User.IsInRole(RoleName.Company))
            {
                products.ID_Empresa = User.Identity.GetUserId();
            }
            if (ModelState.IsValid)
            {
                if (flag == false)
                {
                    db.Produtos.Add(products);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            setupCompanyIdViewBag();
            ViewBag.CategoriaID = new SelectList(db.Categorias.Where(c => c.EstaEliminado == false), "ID", "Nome_Categoria", products.CategoriaID);
            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Produtos.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            setupCompanyIdViewBag();
            ViewBag.CategoriaID = new SelectList(db.Categorias.Where(c => c.EstaEliminado == false), "ID", "Nome_Categoria", products.CategoriaID);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Nome_Produto,Stock,ID_Empresa,Preco_Produto,EstaNoCatalogo,CategoriaID")] Products products)
        {
            bool flag = false;
            //FALTA VERIFICACAO DO NOME
            //foreach (var i in db.Produtos)
            //{
            //    if (i.Nome_Produto == products.Nome_Produto && i.ID_Empresa == products.ID_Empresa)
            //    {
            //        flag = true;
            //        ModelState.AddModelError("ID_Empresa", "This company already have this product!");
            //    }
            //}
            if (ModelState.IsValid)
            {
                if (flag == false)
                {
                    db.Entry(products).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            setupCompanyIdViewBag();
            ViewBag.CategoriaID = new SelectList(db.Categorias.Where(c => c.EstaEliminado == false), "ID", "Nome_Categoria", products.CategoriaID);
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Produtos.Include(p => p.Categoria).Where(p => p.ProductID == id).SingleOrDefault();
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Produtos.Find(id);
            db.Produtos.Remove(products);
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

    //public bool NameProductRepeted(Products produto)
    //{
    //    foreach (var i in db.Produtos)
    //    {
    //        if (i.Nome_Produto == products.Nome_Produto && i.ID_Empresa == products.ID_Empresa)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}
