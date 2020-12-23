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
        public int ProductID { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "The maximum character limit is 50")]
        [Display(Name = "Product Name")]
        public string Nome_Produto { get; set; }
        [Range(0, 100)]
        [Required]
        public int Stock { get; set; }
        [Display(Name = "Promotion")]
        public Promotion Promo { get; set; }
        [Required]
        [Display(Name = "Company ID")]
        public string ID_Empresa { get; set; }
        [Required]
        [Range(0, 100)]
        [Display(Name = "Price")]
        public double Preco_Produto { get; set; }
        public bool EstaNoCatalogo { get; set; }

        public Category Categoria { get; set; }
        [Required]
        public int? CategoriaID { get; set; }

        public ICollection<Order> Pedido { get; set; }

        [Required]
        public bool EstaEliminado {get;set ;}
    }

}