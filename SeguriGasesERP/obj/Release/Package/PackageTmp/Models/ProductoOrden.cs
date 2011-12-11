using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguriGasesERP.Models
{
    public class ProductoOrden
    {
        public int ID { get; set; }
        public int IdProducto { get; set; }
        public int IdOrden { get; set; }
        [DecimalPrecision(18, 2)] 
        public decimal cantidad { get; set; }

        public virtual Producto Producto { get; set; }
        public virtual Sucursal Sucursal { get; set; }
    }
}