using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SeguriGasesERP.Models
{
    [DecimalPrecision(18, 2)] 
    public class OrdenDeCompra
    {
        public int ID { get; set; }
        public DateTime FechaGenerada { get; set; }
        [Required(ErrorMessage = "Es Obligatorio relacionar la Orden de Compra con una Sucursal")]
        [DecimalPrecision(18, 2)]
        public int IdSucursal { get; set; }
        [Required(ErrorMessage = "Es Obligatorio relacionar la Orden de Compra con un Proveedor")]
        public int IdProveedor { get; set; }
        public bool Activa { get; set; }
        public string usuario { get; set; }

        public virtual Sucursal Sucursal { get; set; }
        public virtual List<ProductoOrden> Productos { get; set; }
        public virtual Proveedor Proveedor { get; set; }
        
    }
}