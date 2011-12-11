using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/*
 * Incluirá Management
 */

namespace SeguriGasesERP.Models
{
    [DecimalPrecision(18, 2)] 
    public class Unidad
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
    }
}