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
    public class Producto
    {
        SeguriGasesEntities db = new SeguriGasesEntities();

        public int ID { get; set; }
        [Required(ErrorMessage = "Es Obligatorio asignar una clave o código al producto")]
        public string Clave { get; set; }
        [Required(ErrorMessage = "Es Obligatorio asignarle un nombre al producto")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string FotoUrl { get; set; }
        [Required(ErrorMessage = "Es Obligatorio Indicar un precio de lista para el producto")]
        [DecimalPrecision(18, 2)]  
        public decimal PrecioLista { get; set; }
        [DecimalPrecision(18, 2)]
        public decimal Costo { get; set; }
        public bool Activo { get; set; }
        public int IdUnidad { get; set; }
        public int IdCategoria { get; set; }
        
        

        public virtual Unidad Unidad { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual List<Proveedor> Proveedores { get; set; }
        public virtual List<ProductoSucursal> ProductoSucursal { get; set; }
        public virtual List<ProductoVenta> ProductosVenta { get; set; }

        /*
        * Verificamos la existencia de un producto!
        */
        public decimal GetExistencia(int IdSucursal)
        {
            var prod = from productos in db.ProductosSucursal
                       where ((productos.IdSucursal == IdSucursal && productos.IdProducto == ID))
                       select productos;

            try
            {
                decimal cantidad = prod.First().cantidad;
                return cantidad;
            }
            catch
            {
                return 0;
            }
        }
    }
}