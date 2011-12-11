using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguriGasesERP.Models
{
    
    public class Pago
    {
        public int ID { get; set; }

        [DecimalPrecision(18, 2)]  
        public decimal Monto { get; set; }
        public string TipoPago { get; set; }
        public DateTime FechaPago { get; set; }
        public int IdCliente { get; set; }
        public int IdVenta { get; set; }

        public virtual Venta Venta { get; set; }
        public virtual Cliente Cliente { get; set; }
        
    }
}