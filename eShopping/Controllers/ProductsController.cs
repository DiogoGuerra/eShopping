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
using eShopping.ViewModels;
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
            var productQuant = new ProductQuantity();
            productQuant.ID = id;
            return View(productQuant); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ButtonBuy([Bind(Include = "ID,Quantidade")] ProductQuantity productQuantity)
        {
            if (ModelState.IsValid)
            {
                var cart = db.Carrinho.SingleOrDefault();
               // Se nao houver carrinho
                if (cart == null)
                {
                    //Criar um novo product order
                        var productOrder = new ProductsOrder();
                    //Guardar o product ID
                        productOrder.ProductID = productQuantity.ID;
                    //Guardar a Quantidade
                        productOrder.Quantidade = productQuantity.Quantidade;
                        //Criamos o carrinho
                        cart = new Cart();
                    //Inicializar a lista de products order
                        var lista = new Collection<ProductsOrder>();
                    //Adicionamos o porductOrder a lista 
                        lista.Add(productOrder);
                    //Guardamos a lista no carrinho
                        cart.productsOrders = lista;
                    //Adicionamos o product order à BD
                        db.ProdutosPedidos.Add(productOrder);
                        db.Carrinho.Add(cart);
                }
                else //Se ja houver um carrinho
                {
                    var productOrder = new ProductsOrder();
                    //Guardar o product ID
                    productOrder.ProductID = productQuantity.ID;
                    //Guardar a Quantidade
                    productOrder.Quantidade = productQuantity.Quantidade;
                    //Adicionar o productOrder ao carrinho
                    cart.productsOrders.Add(productOrder);
                    
                    //Tenho de verificar se o product ja existir incremento apenas a quantidade ao produto que la esta

                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
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
                if(flag == false) { 
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
