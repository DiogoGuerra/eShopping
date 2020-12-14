using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShopping.Models
{
    public class Purchase
    {
        [Key]
        public int ID_Venda { get; set; }
        [Required]
        public double Preco_Total { get; set; }
        [Required]
        public virtual ICollection<Products> Produtos { get; set; }
        [Required]
        public string ID_Cliente { get; set; } //Cliente que esta loggado
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Data_Venda { get; set; }
        [Required]
        public bool EstaValidado { get; set; } // booleano para validar a compra
        [Required]
        public bool EstaEntregue { get; set; }

        public Delivery Entrega { get; set; } // Se der problemas na db , trocamos por bool
       [Required]
        public int ID_Entrega { get; set; }
    }

}