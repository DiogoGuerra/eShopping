using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShopping.Models
{
    public class Products
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "The maximum character limit is 50")]
        public string Nome_Produto { get; set; }
        [Range(0,100)]
        [Required]
        public int Stock { get; set; }
        public Promotion Promo { get; set; }
        [Required]
        public string ID_Empresa { get; set; }
        [Required]
        [Range(0,100)]
        public double Preco_Produto { get; set; } // Na vista, nao mostrava o preco, por causa da promo 
        [Required]
        public bool EstaNoCatalogo { get; set; }
       
        public Category Categoria { get; set; }
        [Required]
        public int CategoriaID { get; set; }

        public ICollection<Purchase> Compra { get; set; }

    }

}