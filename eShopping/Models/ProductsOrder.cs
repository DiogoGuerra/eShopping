using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eShopping.Models
{
    public class ProductsOrder
    {
        [Key, Column(Order = 1)]
        public int ProductID { get; set; }

        [Key, Column(Order = 2)]
        public int OrderID { get; set;
        }
        [ForeignKey("ProductID")]
        public Products Produto { get; set; }
        [ForeignKey("OrderID")]
        public Order Pedido { get; set; }

        [Required]
        public int Quantidade { get; set; }

        public double Preco_Produto { get; set; } //Vamos guardar este preço para depois de haver promoções, mantermos o registo do preço certo
    
    }
}