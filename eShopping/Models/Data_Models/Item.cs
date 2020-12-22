using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;


namespace eShopping.Models.Data_Models
{
    public class Item
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int Quantidade { get; set; }
        [Required]
        public Products Produto { get; set; }

        [Required]
        public double UniPreco
        {
            get { return Produto.Preco_Produto; }
        }
        [Required]
        public double TotalPrice
        {
            get { return UniPreco * Quantidade; }
        }
        public Cart Carrinho { get; set; }
        public int CarrinhoID { get; set; }

    }
}