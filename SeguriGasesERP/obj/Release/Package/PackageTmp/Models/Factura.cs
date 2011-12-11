using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguriGasesERP.Models
{
    [DecimalPrecision(18, 2)] 
    public class Factura
    {
        /*Este modelo solo almacenará el XML generado por el sistema FEL.MX junto con el número de folio en forma de clave primaria*/

        //Clave Primaria sera conformada por el número de folio y su serie ej. A234, extraido del XML
        public string ID { get; set; }

        //Factura en formato XML para poder ser parseada a nuestro gusto posteriormente
        public string Xml { get; set; }

        
    }
}