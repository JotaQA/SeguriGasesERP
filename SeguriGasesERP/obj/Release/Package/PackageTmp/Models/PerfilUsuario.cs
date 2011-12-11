using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SeguriGasesERP.Models
{
    [DecimalPrecision(18, 2)] 
	public class PerfilUsuario
	{
        public int ID { get; set; }
        [Required(ErrorMessage = "Es Obligatorio Relacionarse con un usuario registrado")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Es Obligatorio Indicar el o los nombres del usuario")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Es Obligatorio indicar los apellidos del usuario")]
        public string Apellidos { get; set; }

        public virtual List<Sucursal> Sucursales { get; set; }
	}
}