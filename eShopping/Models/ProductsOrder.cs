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
        [Key]
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public Products Produto { get; set; }

        [Required]
        public int Quantidade { get; set; }

        public int Preco_Produto { get; set; } //Vamos guardar este preço para depois de haver promoções, mantermos o registo do preço certo
    
    }
}