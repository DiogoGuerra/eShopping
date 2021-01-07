using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eShopping.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        //[Key]
        //public string userID { get; set; }
        [Required]
        public string Nome { get; set; }

        public bool EstaEliminado { get; set; }
        public ICollection<Products> Produtos { get; set; }
        public ICollection<Order> Pedidos { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}