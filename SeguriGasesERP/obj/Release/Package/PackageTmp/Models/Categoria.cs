using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 * Incluirá Management
 */
namespace SeguriGasesERP.Models
{
    public class Categoria
    {
        public int ID { get; set; }
        public string IdPadre { get; set; }
        public string Nombre { get; set; }


        public virtual Categoria CategoriaPadre { get; set; }
        public virtual List<Producto> Productos { get; set; }

        
    }
}