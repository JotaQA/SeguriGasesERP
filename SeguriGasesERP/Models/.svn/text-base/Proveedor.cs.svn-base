using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/*
 * Incluirá Management
 */

namespace SeguriGasesERP.Models
{
    [DecimalPrecision(18, 2)] 
    public class Proveedor
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Es Obligatorio Proporcionar un RFC para el proveedor")]
        public string RFC { get; set; }
        [Required(ErrorMessage = "Es Obligatorio indicar la Razon Social o Nombre del proveedor")]
        public string RazonSocial { get; set; }
        public string Contacto { get; set; }
        public string Telefono { get; set; }
        public string Nextel { get; set; }
        [Required(ErrorMessage = "Es Obligatorio indicar la dirección del proveedor")]
        public string Direccion { get; set; }
        public string Email { get; set; }

        public virtual List<Producto> Productos { get; set; }
    }
}