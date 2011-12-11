using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguriGasesERP.Models
{
    [DecimalPrecision(18, 2)] 
    public class MovimientoAlmacen
    {
        /*
         * Este modelo almacena los movimientos realizados en los almacenes
         */
        public int ID { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string TipoMovimiento { get; set; }
        public string DescripcionMovimiento { get; set; }
        [DecimalPrecision(18, 2)] 
        public decimal Count { get; set; }
        public int IdSucursal { get; set; }
        public int IdProducto { get; set; }
        public int IdOrednCompra { get; set; }
        public int IdVenta { get; set; }
        public string username { get; set; }

        public virtual Sucursal Sucursal { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual OrdenDeCompra OrdenDeCompra { get; set; }
        public virtual Venta Venta { get; set; }
        //public virtual MovimientoInterno MovimientoInterno { get; set; }


    }
}