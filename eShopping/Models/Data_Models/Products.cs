using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace eShopping.Models
{
    public class Products
    {
        public int ID { get; set; }
        [StringLength(128)]
        public string Nome_Produto { get; set; }
        [Range(0,100)]
        public int Stock { get; set; }
        [Required]
        [Range(0.1,100)]
        public Promotion Promo { get; set; } // Passar para classe
        public int PromocaoID { get; set; }
        [Required]
        public string ID_Empresa { get; set; }
        [Required]
        [Range(0.1,100)]
        public double Preco_Produto; // Na vista, nao mostrava o preco, por causa da promo 

        public bool EstaNoCatalogo { get; set; }

        public Category Categoria { get; set; }
        public int CategoriaID { get; set; }

    }

}