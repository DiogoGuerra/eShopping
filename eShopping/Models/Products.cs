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
        public Company Company { get; set; }
        [Required]
        [Display(Name = "Company ID")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        [Required]
        [Range(0, 100)]
        [Display(Name = "Price")]
        public double Preco_Produto { get; set; }
        [Display(Name = "It's in the catalog")]
        public bool EstaNoCatalogo { get; set; }

        public Category Categoria { get; set; }
        public int? CategoriaID { get; set; }

        [Required]
        public bool EstaEliminado {get;set ;}
        [Display(Name = "Promotion")]
        public double Taxa_Promocao { get; set; }
    }

}