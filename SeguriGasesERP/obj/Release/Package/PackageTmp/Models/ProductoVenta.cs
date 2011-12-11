using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguriGasesERP.Models
{
    [DecimalPrecision(18, 2)] 
    public class ProductoVenta
    {
        public int ID { get; set; }
        [DecimalPrecision(18, 2)]  
        public decimal Count { get; set; }
        [DecimalPrecision(18, 2)]  
        public decimal PrecioVenta { get; set; }
        public int ProductoId { get; set; }
        public int VentaId { get; set; }

        public virtual Venta Venta { get; set; }
        public virtual Producto Producto { get; set; }
    }
}