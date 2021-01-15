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

        [Authorize(Roles = RoleName.AdminOrCompany)]
        // GET: Products
        public ActionResult Index()
        {
            int com = 0;
            if (User.IsInRole(RoleName.Company))
            {
                var userID = User.Identity.GetUserId();

                foreach (var i in db.Users)
                {
                    if(i.Id == userID)
                    {
                        com = (int)i.CompanyId;
                    }
                }
                
                var c = db.Produtos.Include(p => p.Categoria).Include(e => e.Company).Where(r => r.EstaEliminado == false).Where(p => p.Company.CompanyId == com);
                return View("CompanyIndex",c.ToList());
            }

            var produtos = db.Produtos.Include(p => p.Categoria).Include(e => e.Company).Where(r => r.EstaEliminado == false);
            return View(produtos.ToList());
        }

        [Authorize(Roles = RoleName.User)]
        public ActionResult ListCostumerProducts(string categoria)
        {
            var produtos = db.Produtos.Include(p => p.Categoria).Include(e => e.Company).Where(r => r.EstaEliminado == false).Where(s => s.Stock > 0).Where( s => s.EstaNoCatalogo == true);

            if (!string.IsNullOrEmpty(categoria))
                produtos = produtos.Where(p => p.Categoria.Nome_Categoria == categoria).Where(r => r.EstaEliminado == false);
            return View(produtos.ToList());
        }
        [AllowAnonymous]
        public ActionResult ListAnonymous(string categoria)
        {
            var produtos = db.Produtos.Include(p => p.Categoria).Include(e => e.Company).Where(r => r.EstaEliminado == false).Where(s => s.Stock > 0).Where(s => s.EstaNoCatalogo == true);

            if (!string.IsNullOrEmpty(categoria))
                produtos = produtos.Where(p => p.Categoria.Nome_Categoria == categoria).Where(r => r.EstaEliminado == false);
            return View(produtos.ToList());
        }

        [Authorize(Roles = RoleName.Employee)]
        public ActionResult ListProdPromotions()
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
            var produtos = db.Produtos.Include(c => c.Company).Where(c => c.EstaEliminado == false).Where(c => c.CompanyId == idc);
            return View(produtos.ToList());
        }

        [Authorize(Roles = RoleName.Employee)]
        public ActionResult EditPromo(int? id)
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
            
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPromo([Bind(Include = "ProductID,Nome_Produto,Stock,CompanyId,Preco_Produto,EstaNoCatalogo,CategoriaID,Taxa_Promocao")] Products products)
        {
            if (ModelState.IsValid)
            {
               
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListProdPromotions");
            }
            return View(products);
        }

        [Authorize(Roles = RoleName.AdminOrCompany)]
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Para aparecer a categoria nos detalhes temos de incluir
            Products products = db.Produtos.Include(c => c.Company).Include(p => p.Categoria).Where(p => p.ProductID == id).SingleOrDefault();
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }
        [Authorize(Roles = RoleName.User)]
        public ActionResult ButtonAddCart(int id)
        {
            ProductsOrder produtoPedido = new ProductsOrder();

            produtoPedido.ProductID = id;

            return View(produtoPedido);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ButtonAddCart([Bind(Include = "ProductID,Quantidade")] ProductsOrder ProdPedido)
        {
            //Products produto = db.Produtos.Find(ProdPedido.ProductID);
            var produto = db.Produtos.Include(p => p.Company).FirstOrDefault(x => x.ProductID == ProdPedido.ProductID);
           
            Order aux = null;
            if (produto.Stock < ProdPedido.Quantidade)
            {
                ModelState.AddModelError("Quantidade", "This company already have this product!");
            }
            if (ModelState.IsValid)
            {
                int flag = 0;
                //ProdPedido.ProductID = produto.ProductID;
                var pedi = db.Pedidos.Where(p => p.PedidoEmAberto == true).Include(p => p.Empresa);

                var existePedidoParaEmpresa = pedi.Any(p => p.Empresa.CompanyId == produto.CompanyId);
                
                if (pedi != null)
                {
                    foreach (var i in pedi) // percorrer os pedidos em aberto
                    {
                        // Se encontrar algum pedido da empresa
                        if (i.Empresa.CompanyId == produto.Company.CompanyId)
                        {
                            //Se o pedido tiver aberto, adicionamos
                            aux = i;
                            Order ped = i;
                            ped.EstadoID = 1;
                            ProdPedido.Pedido = ped;
                   
                            flag = 2; //verificar se o produto ja existe nos pedidos em aberto
                        }
                    }
                }
                double pr = 0;
                //Se nao houver nenhum pedido da empresa desejada
                if (flag == 0)
                {
                    Order novopedido = new Order();
                    Company empresa = db.Empresas.Find(produto.CompanyId);
                    novopedido.Empresa = empresa;
                    novopedido.ID_Cliente = User.Identity.GetUserId();
                    pr = produto.Preco_Produto * (1 - produto.Taxa_Promocao);
                    novopedido.Preco_Total += pr * ProdPedido.Quantidade;
                    novopedido.PedidoEmAberto = true;
                    novopedido.Data_Venda = DateTime.Now;
                    novopedido.EntregaID = 2;
                    novopedido.EstadoID = 1;
                    ProdPedido.OrderID = novopedido.OrderID;
                    ProdPedido.Preco_Produto = pr; //Guardar o preço c/ promoção
                    db.Pedidos.Add(novopedido);
                    db.ProdutosPedidos.Add(ProdPedido);
                }
                if (flag == 2)
                {
                    int existe = 0;
                    var prodped = db.ProdutosPedidos.Include(p => p.Produto).Include(p => p.Pedido).Where(p => p.Pedido.PedidoEmAberto == true);
                    foreach (var d in prodped)
                    {
                        if (d.Produto == produto)
                        {
                            existe = 1;
                            d.Quantidade += ProdPedido.Quantidade;
                            Order p = d.Pedido;
                            pr = produto.Preco_Produto * (1 - produto.Taxa_Promocao);
                            p.Preco_Total += pr * ProdPedido.Quantidade;
                           // p.Preco_Total += produto.Preco_Produto * ProdPedido.Quantidade;
                            d.Pedido = p;
                            //d.Pedido.Preco_Total += produto.Preco_Produto * ProdPedido.Quantidade;
                            db.Entry(d).State = EntityState.Modified; 
                        }
                    }
                    if (existe == 0)
                    {
                        ProdPedido.Produto = produto;
                        ProdPedido.Preco_Produto = produto.Preco_Produto * (1 - produto.Taxa_Promocao); 
                        ProdPedido.Pedido.Preco_Total += produto.Preco_Produto * ProdPedido.Quantidade;
                        db.ProdutosPedidos.Add(ProdPedido);
                        //Temos de adicionar o produto se nao houver o que ele quer adicionar
                    }
                }

                produto.Stock -= ProdPedido.Quantidade;
                db.Entry(produto).State = EntityState.Modified;
                db.SaveChanges();
            }
            
            return RedirectToAction("ListCostumerProducts");
        }
        [Authorize(Roles = RoleName.AdminOrCompany)]
        // GET: Products/Create
        public ActionResult Create()
        {
            int com = 0;
            if (User.IsInRole(RoleName.Company))
            {
                var userID = User.Identity.GetUserId();

                foreach (var i in db.Users)
                {
                    if (i.Id == userID)
                    {
                        com = (int)i.CompanyId;
                    }
                }
                ViewBag.CategoriaID = new SelectList(db.Categorias.Where(c => c.EstaEliminado == false), "ID", "Nome_Categoria");
                Products novo = new Products { CompanyId = com, EstaEliminado = false};
                return View("CompanyCreate", novo);
            }       
            ViewBag.CompanyId = new SelectList(db.Empresas.Where(c => c.EstaEliminado == false), "CompanyId", "Nome");
            ViewBag.CategoriaID = new SelectList(db.Categorias.Where(c => c.EstaEliminado == false), "ID", "Nome_Categoria");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Nome_Produto,Stock,CompanyId,Preco_Produto,EstaNoCatalogo,CategoriaID")] Products products)
        {
            bool flag = false;
            foreach (var i in db.Produtos)
            {
                if (i.Nome_Produto == products.Nome_Produto && i.CompanyId == products.CompanyId)
                {
                    flag = true;
                    ModelState.AddModelError("Nome_Produto", "This company already have this product!");
                }
            }

            Company empresa = db.Empresas.Find(products.CompanyId);
            products.Company = empresa;
            //products.Empresa.Nome = empresa.Nome;
            if (ModelState.IsValid)
            {
                if (flag == false)
                {
                    db.Produtos.Add(products);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            int com = 0;
            if (User.IsInRole(RoleName.Company))
            {
                var userID = User.Identity.GetUserId();

                foreach (var i in db.Users)
                {
                    if (i.Id == userID)
                    {
                        com = (int)i.CompanyId;
                    }
                }
                ViewBag.CategoriaID = new SelectList(db.Categorias.Where(c => c.EstaEliminado == false), "ID", "Nome_Categoria");
                Products novo = new Products { CompanyId = com, EstaEliminado = false };
                return View("CompanyCreate", novo);
            }

            ViewBag.CompanyId = new SelectList(db.Empresas.Where(c => c.EstaEliminado == false), "CompanyId", "Nome");
            ViewBag.CategoriaID = new SelectList(db.Categorias.Where(c => c.EstaEliminado == false), "ID", "Nome_Categoria", products.CategoriaID);
            return View(products);
        }
        [Authorize(Roles = RoleName.AdminOrCompany)]
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
            ViewBag.CompanyId = new SelectList(db.Empresas.Where(c => c.EstaEliminado == false), "CompanyId", "Nome");
            ViewBag.CategoriaID = new SelectList(db.Categorias.Where(c => c.EstaEliminado == false), "ID", "Nome_Categoria", products.CategoriaID);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Nome_Produto,Stock,CompanyId,Preco_Produto,EstaNoCatalogo,CategoriaID")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Empresas.Where(c => c.EstaEliminado == false), "CompanyId", "Nome");
            ViewBag.CategoriaID = new SelectList(db.Categorias.Where(c => c.EstaEliminado == false), "ID", "Nome_Categoria", products.CategoriaID);
            return View(products);
        }
        [Authorize(Roles = RoleName.AdminOrCompany)]
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Produtos.Include(c=> c.Company).Include(p => p.Categoria).Where(p => p.ProductID == id).SingleOrDefault();
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
            //db.Produtos.Remove(products);
            products.EstaEliminado = true;
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
