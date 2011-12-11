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
    public class Cilindro
    {
        public int ID { get; set; }
        
        public String TipoCilindro { get; set; }

        public virtual List<CilindroDotacion> Dotaciones { get; set; }
    }
}