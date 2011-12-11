using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace SeguriGasesERP.Models
{
    [DecimalPrecision(18, 2)] 
    public class Carrito
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public int ProductoId { get; set; }
        [DecimalPrecision(18, 2)] 
        public decimal PrecioVenta { get; set; }     
        [DecimalPrecision(18, 2)]
        public decimal Count { get; set; }
        public System.DateTime DateCreated { get; set; }     
        [DecimalPrecision(18, 2)]  
        public decimal Existencia { get; set; }        
        public virtual Producto Producto { get; set; }
        


        public decimal GetCantidad(int IdSucursal)
        {

            return Producto.GetExistencia(IdSucursal);
        }

        
    }
}