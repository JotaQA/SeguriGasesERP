using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SeguriGasesERP.Models
{
    public class OrdenTraslado
    {
        public int ID { get; set; }
        public DateTime FechaGenerada { get; set; }
        [Required(ErrorMessage = "Es Obligatorio relacionar la Orden de Compra con una Sucursal de Origen")]
        public int IdSucursalOrigen { get; set; }
        [Required(ErrorMessage = "Es Obligatorio relacionar la Orden de Compra con una Sucursal Destino")]
        public int IdSucursalDestino { get; set; }
        [Required(ErrorMessage = "Es Obligatorio relacionar la Orden de Compra con un Proveedor")]
        public int IdProveedor { get; set; }
        public bool Activa { get; set; }
        public string usuario { get; set; }

        public virtual Sucursal SucursalOrigen { get; set; }
        public virtual Sucursal SucursalDestino { get; set; }
        public virtual List<ProductoTraslado> Productos { get; set; }
        public virtual Proveedor Proveedor { get; set; }
    }
}