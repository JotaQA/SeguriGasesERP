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
    public class Sucursal
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Es Obligatorio indicar un nombre de sucursal")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Es Obligatorio indicar el domicilio fiscal")]
        public string DomicilioFiscal { get; set; }
        public string Telefono { get; set; }

        //Datos para el Login en FEL.MX
        [Required(ErrorMessage = "Es Obligatorio indicar el RFC")]
        public string RFC { get; set; }
        [Required(ErrorMessage = "Es Obligatorio indicar la cuenta de FEL")]
        public string Cuenta { get; set; }
        [Required(ErrorMessage = "Es Obligatorio indicar la contraseña del FEL")]
        public string Password { get; set; }

        public virtual List<ProductoSucursal> ProductoSucursal { get; set; }
        public virtual List<PerfilUsuario> Usuarios { get; set; }
    }
}