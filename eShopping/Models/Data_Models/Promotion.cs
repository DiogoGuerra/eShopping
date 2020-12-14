using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace eShopping.Models
{
    public class Promotion
    {
        public int ID { get; set; }
        [Required]
        [Range(0.1,0.9)] //Verificar, pq como e taxa nunca vai ser 1, nao ha taxas de promocao de 100%
        public double TaxaPromocao { get; set; }
    }
}