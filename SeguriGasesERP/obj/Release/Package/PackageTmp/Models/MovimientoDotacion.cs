using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguriGasesERP.Models
{
    [DecimalPrecision(18, 2)] 
    public class MovimientoDotacion
    {
        public int ID { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string TipoMovimiento { get; set; }
        public string Descripcion { get; set; }
        public int cantidad { get; set; }
        public int IdDotacion { get; set; }

        public virtual DotacionCilindros Dotacion { get; set; }
    }
}