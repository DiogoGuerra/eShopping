using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eShopping.Models.Data_Models
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }

        public ICollection<Item> Items { get; set; }
        [Required]
        public string ID_Cliente { get; set; }
    }
}