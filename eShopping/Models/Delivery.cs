using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShopping.Models
{
    public class Delivery
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "The maximum character limit is 50")]
        [Display(Name = "Type")]
        public string Tipo { get; set; }

        public ICollection<Order> Compras { get; set; }
    }
}