using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace eShopping.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        [MaxLength(50, ErrorMessage = "The maximum character limit is 50") ]
        public string Nome_Categoria { get; set; }

        public ICollection<Products> Produtos { get; set; }

        public bool EstaEliminado { get; set; }
    }
}