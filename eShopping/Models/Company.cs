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
        public int ID { get; set; }
        public string Nome { get; set; }

        public ICollection<Order> Pedidos { get; set; }
    }
}