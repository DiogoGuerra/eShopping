﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace eShopping.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Category> Categorias { get; set; }
        public DbSet<Products> Produtos { get; set; }
        public DbSet<Delivery> Entregas { get; set; }
        public DbSet<Order> Pedidos { get; set; }
        public DbSet<Promotion> Promocoes { get; set; }

        public DbSet<Status> Estados { get; set; }
        public DbSet<ProductsOrder> ProdutosPedidos { get; set; }

        public DbSet<Company> Empresas { get; set; }

        //public System.Data.Entity.DbSet<eShopping.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public System.Data.Entity.DbSet<eShopping.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}