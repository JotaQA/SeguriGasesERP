using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguriGasesERP.Models
{
    public class CilindroDotacion
    {
        public int ID { get; set; }
        public int IdCilindro { get; set; }
        public int IdDotacion { get; set; }
        [DecimalPrecision(18, 2)]
        public decimal Deposito { get; set; }

        public virtual Cilindro Cilindro { get; set; }
        public virtual DotacionCilindros Dotacion { get; set; }
    }
}