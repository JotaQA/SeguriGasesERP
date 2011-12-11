using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguriGasesERP.Models
{
    [DecimalPrecision(18, 2)] 
    public class ProductoSucursal
    {
        public int ID { get; set; }
        [DecimalPrecision(18, 2)]  
        public decimal cantidad { get; set; }
        public int IdProducto { get; set; }
        public int IdSucursal { get; set; }

        public virtual Producto Producto { get; set; }
        public virtual Sucursal Sucursal { get; set; }
    }
}