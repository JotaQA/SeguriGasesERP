using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Drawing;
using SeguriGasesERP.Models;

namespace SeguriGasesERP.Controllers
{
    [Authorize(Roles = "Gerente")]
    public class ReportesController : Controller
    {

        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /Reportes/

        /*
         * Aquí se incluyen los controladores para generar todos  los reportes del sistema
         *  1.- Catálogo de Productos
         *  2.- Lista de Precios
         *  3.- Catálogo de Proveedores
         *  4.- Reporte de ventas Por factura
         *  5.- Historial de ventas por cliente
         *  6.- Estado de cuenta por cliente con pagos
         *  7.- Reporte de Compras (Ordenes de compras)
         *  8.- Reporte de facturas vencidas a N días
         *  9.- Reporte de Pagos
         *  10.- Reporte general de cartera
         *  11.- Reporte general de cartera vencida
         *  12.- Reporte de inventario general
         *  13.- Reporte de inventario por sucursal
         *  14.- Consulta de precios por producto (Se enlistan los precios para cada cliente)
         *  15.- Consulta de precios por cliente (Se enlistan los productos con los precios de venta)
         *  16.- Reporte de Compras por producto
         *  17.- Reporte de Compras por proveedor
         */

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CatalogoProductos()
        {
            
            /*Este mostrará un catalogo de productos, mostrando su Clave, Nombre, Precio de lista y Costo*/

            /* La página principal será un desglose general de los productos con opcion a ser filtrados por categoria, proveedor o nombre */

            // Recuperamos una lista de todos los productos existentes
            List<Producto> Productos = db.Productos.Include("Categoria").Include("Unidad").OrderBy(p => p.Nombre).ToList();

            //Cargamos la informacion de unidad y Cartegoria para cada procucto

            foreach (Producto Prod in Productos)
            {
                if (Prod.IdUnidad != 0)
                    Prod.Unidad = db.Unidades.Find(Prod.IdUnidad);

                if (Prod.IdCategoria != 0 )
                    Prod.Categoria = db.Categorias.Find(Prod.IdCategoria);

            }
            

            /* Incluimos la lista de proveedores para la selección */
            List<Proveedor> Proveedores = db.Proveedores.OrderBy(P => P.RazonSocial).ToList();
            /* Incluimos la lista de categorías */
            List<Categoria> Categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();

            ViewBag.Productos = Productos;
            ViewBag.Preveedores = Proveedores;
            ViewBag.Categorias = Categorias;

            return View(Productos);

        }

        [HttpPost]
        public ActionResult CatalogoProductos(int tipo, int id, string word)
        {
            /*Separaremos en dos tipos para regresar los reportes por proveedor o por categoria*/
            
            /* Incluimos la lista de proveedores para la selección */
            List<Proveedor> Proveedores = db.Proveedores.OrderBy(P => P.RazonSocial).ToList();
            /* Incluimos la lista de categorías */
            List<Categoria> Categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();


            switch (tipo)
            {
                    
                case 1:
                    List<Producto> Productos = new List<Producto>();
                    /*Si es por Categoria*/
                    /*Buscamos los productos que coincidan con el Id de la categoria buscada */
                    Productos = db.Productos.Where(p => p.IdCategoria == id).ToList();
                    foreach (Producto Prod in Productos)
                    {
                        if (Prod.IdUnidad != 0)
                            Prod.Unidad = db.Unidades.Find(Prod.IdUnidad);

                        if (Prod.IdCategoria != 0)
                            Prod.Categoria = db.Categorias.Find(Prod.IdCategoria);

                    }

                    ViewBag.Productos = Productos;
                    ViewBag.Preveedores = Proveedores;
                    ViewBag.Categorias = Categorias;

                    return View(Productos);
                    
                case 2:
                    /*Si es por proveedor*/
                    /*Recuperamos todos los productos del proveedor con ID id*/
                    Proveedor Prove = db.Proveedores.Include("Productos").Single( p => p.ID == id);
                    var x = Prove.Productos;

                    Productos = Prove.Productos;
                    foreach (Producto Prod in Productos)
                    {
                        if (Prod.IdUnidad != 0)
                            Prod.Unidad = db.Unidades.Find(Prod.IdUnidad);

                        if (Prod.IdCategoria != 0)
                            Prod.Categoria = db.Categorias.Find(Prod.IdCategoria);

                    }

                    ViewBag.Productos = Productos;
                    ViewBag.Preveedores = Proveedores;
                    ViewBag.Categorias = Categorias;

                    return View(Productos);   
            }

            List<Producto> Productos1 = db.Productos.Where(p => p.Nombre.ToLower().Contains(word.ToLower()) || p.Clave.ToLower().Contains(word.ToLower())).ToList();
            foreach (Producto Prod in Productos1)
            {
                if (Prod.IdUnidad != 0)
                    Prod.Unidad = db.Unidades.Find(Prod.IdUnidad);

                if (Prod.IdCategoria != 0)
                    Prod.Categoria = db.Categorias.Find(Prod.IdCategoria);

            }

            ViewBag.Productos = Productos1;
            ViewBag.Preveedores = Proveedores;
            ViewBag.Categorias = Categorias;

            return View(Productos1);

            return View();
        }



    }
}
