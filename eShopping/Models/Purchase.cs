using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eShopping.Models
{
    public class Purchase
    {
        public int ID_Venda { get; set; }
        public double Preco_Total { get; set; }
        public virtual ICollection<Products> Produtos { get; set; }

        public string ID_Cliente { get; set; } //Cliente que esta loggado

        public DateTime Data_Venda { get; set; }

        public bool EstaValidado { get; set; } // booleano para validar a compra
        public bool EstaEntregue { get; set; }

        public Delivery Entrega { get; set; } // Se der problemas na db , trocamos por bool
        public int ID_Entrega { get; set; }
    }

}