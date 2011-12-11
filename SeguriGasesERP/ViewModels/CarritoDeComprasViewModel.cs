using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeguriGasesERP.Models;

namespace SeguriGasesERP.ViewModels
{
    public class CarritoDeComprasViewModel
    {
        public List<Carrito> ElementosCarrito { get; set; }
        [DecimalPrecision(18, 2)]  
        public decimal TotalCarrito { get; set; }
        public Cliente cliente { get; set; }

        
    }
}