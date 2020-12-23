using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShopping.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        [Required]
        public string ID_Cliente { get; set; } 
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Data_Venda { get; set; }
       
        public bool EstaFinalizado { get; set; } //o Pedido ja foi enviado pelo cliente a empresa
        public Status Estado { get; set; }

        public Delivery Entrega { get; set; } 
       [Required]
        public int EntregaID { get; set; }

        public Company Empresa { get; set; }
    }

}