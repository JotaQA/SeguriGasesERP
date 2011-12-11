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

        [Authorize(Roles = "Gerente")]
        public ActionResult ReporteInventario(int sucursal = 0)
        {

            decimal valorCosto = 0, valorCostoIva = 0, valorVenta = 0, valorVentaIva = 0;

            List<ProductoSucursal> Inventario;
            if(sucursal == 0)
                Inventario = db.ProductosSucursal.Include("Producto").Include("Sucursal").OrderBy(p => p.Producto.Nombre).ToList();                
            else
                Inventario = db.ProductosSucursal.Include("Producto").Include("Sucursal").Where(p => p.IdSucursal == sucursal).OrderBy(p => p.Producto.Nombre).ToList();
               
            
            foreach (ProductoSucursal item in Inventario)
            {
                item.Sucursal = db.Sucursales.Find(item.IdSucursal);
                valorCosto += item.Producto.Costo;
                valorCostoIva += item.Producto.Costo * (decimal)1.16;
                valorVenta += item.Producto.PrecioLista;
                valorVentaIva += item.Producto.PrecioLista * (decimal)1.16;
            }

            ViewBag.Costo = valorCosto;
            ViewBag.CostoIva = valorCostoIva;
            ViewBag.Venta = valorVenta;
            ViewBag.VentaIva = valorVentaIva;

            return View(Inventario);
        }

        /*
         * Reporte de ventas por facura:
         *  Este reporte despliega una lista de las ventas realizadas, por default en el mes, pero puede se extendido al rango de fechas deseado
         *  al dar click en el numero de factura, se desplegaran los detalles de la venta. Tambien pueden filtrarse por sucursal, por default estaran presentes todas las sucursales
         */
        [Authorize(Roles = "Gerente")]
        public ActionResult ReporteVentasFactura()
        {
            /*
             * Listamos las ventas realizadas durante el mes en el que nos encontramos
             */
            decimal TotalIva = 0;
            decimal Total = 0;
            List<Venta> ventas = db.Ventas.Where(v => v.FechaVenta.Month == DateTime.Now.Month && v.FechaVenta.Year == DateTime.Now.Year).OrderBy(v => v.FechaVenta).ToList();
            foreach (Venta item in ventas)
            {
                item.Cliente = db.Clientes.Find(item.IdCliente);
                item.Sucursal = db.Sucursales.Find(item.IdSucursal);
                Total += item.Subtotal;
                TotalIva += item.Total;
            }
            List<SelectListItem> Meses = FillMeses();            
            List<SelectListItem> anios = FillAnios();
            

            ViewBag.Anios = anios;
            ViewBag.Meses = Meses;
            ViewBag.TotalIva = TotalIva;
            ViewBag.Total = Total;
            return View(ventas);
        }

        [HttpPost]
        public ActionResult ReporteVentasFactura(int caca = 1)
        {
            /*
            * Listamos las ventas realizadas durante el mes en el que nos encontramos
            */
            decimal TotalIva = 0;
            decimal Total = 0;
            int mes1 = int.Parse(Request.Form["mes1"]);
            int mes2 = int.Parse(Request.Form["mes2"]);

            int anio1 = int.Parse(Request.Form["anio1"]);
            int anio2 = int.Parse(Request.Form["anio2"]);
            List<Venta> ventas = db.Ventas.Where(v => (v.FechaVenta.Year >= anio1 && v.FechaVenta.Month >= mes1) && (v.FechaVenta.Year <= anio2 && v.FechaVenta.Month <= mes2)).OrderBy(v => v.FechaVenta).ToList();
            foreach (Venta item in ventas)
            {
                item.Cliente = db.Clientes.Find(item.IdCliente);
                item.Sucursal = db.Sucursales.Find(item.IdSucursal);
                Total += item.Subtotal;
                TotalIva += item.Total;
            }
            List<SelectListItem> Meses = FillMeses();
            List<SelectListItem> anios = FillAnios();


            ViewBag.Anios = anios;
            ViewBag.Meses = Meses;
            ViewBag.TotalIva = TotalIva;
            ViewBag.Total = Total;

            return View(ventas);
        }

        [Authorize(Roles = "Gerente")]
        public ActionResult ReporteVentasProducto()
        {
            decimal totalNoIVA = 0;
            List<SelectListItem> Meses = FillMeses();
            List<SelectListItem> anios = FillAnios();

            /*Sacamos primero la lista de productos vendidos este mes, es el comportamiento por default*/
            //List<ProductoVenta> ProductosVentas = db.ProductosVenta.OrderBy(pv => pv.VentaId).ToList();
            List<ProductoVenta> ProductosVentas = db.ProductosVenta.Include("Venta").Where(pv => pv.Venta.FechaVenta.Month == DateTime.Now.Month && pv.Venta.FechaVenta.Year == DateTime.Now.Year).OrderBy(pv => pv.VentaId).ToList();
            foreach (ProductoVenta item in ProductosVentas)
            {
                item.Venta = db.Ventas.Find(item.VentaId);
                item.Producto = db.Productos.Find(item.ProductoId);
                item.Producto.Categoria = db.Categorias.Find(item.Producto.IdCategoria);
                item.Venta.Cliente = db.Clientes.Find(item.Venta.IdCliente);
                totalNoIVA += item.PrecioVenta * item.Count;
            }
            
            ViewBag.Anios = anios;
            ViewBag.Meses = Meses;
            ViewBag.TotalIva = totalNoIVA * (decimal)1.16;
            ViewBag.Total = totalNoIVA;

            return View(ProductosVentas);
        }

        [HttpPost]
        public ActionResult ReporteVentasProducto(int casa = 1)
        {

            decimal totalNoIVA = 0;
            List<SelectListItem> Meses = FillMeses();
            List<SelectListItem> anios = FillAnios();

            int mes1 = int.Parse(Request.Form["mes1"]);
            int mes2 = int.Parse(Request.Form["mes2"]);

            int anio1 = int.Parse(Request.Form["anio1"]);
            int anio2 = int.Parse(Request.Form["anio2"]);

            /*Sacamos primero la lista de productos vendidos en el rango de fechas indicado*/
            //List<ProductoVenta> ProductosVentas = db.ProductosVenta.Include("Venta").OrderBy(pv => pv.VentaId).ToList();
            List<ProductoVenta> ProductosVentas = db.ProductosVenta.Include("Venta").Where(pv => (pv.Venta.FechaVenta.Month >= mes1 && pv.Venta.FechaVenta.Year >= anio1) && (pv.Venta.FechaVenta.Month <= mes2 && pv.Venta.FechaVenta.Year <= anio2)).OrderBy(pv => pv.VentaId).ToList();
            foreach (ProductoVenta item in ProductosVentas)
            {
                item.Producto = db.Productos.Find(item.ProductoId);
                item.Venta.Cliente = db.Clientes.Find(item.Venta.IdCliente);
                item.Producto.Categoria = db.Categorias.Find(item.Producto.IdCategoria);
                totalNoIVA += item.PrecioVenta * item.Count;
            }
            
            ViewBag.Anios = anios;
            ViewBag.Meses = Meses;
            ViewBag.TotalIva = totalNoIVA * (decimal)1.16;
            ViewBag.Total = totalNoIVA;

            return View(ProductosVentas);
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

            
        }
        [Authorize(Roles = "Creditista")]
        public ActionResult ReporteCreditos()
        {
            /*Recuperamos todas las cuentas de credito que hay en el sistema*/

            List<CuentaCredito> Cuentas = db.CuentasCredito.Include("Cliente").ToList();

            foreach (var item in Cuentas)
            {
                item.Cliente = db.Clientes.Find(item.IdCliente);                
            }
            Cuentas.OrderBy(c => c.Cliente.NombreCliente);
           

            
            return View(Cuentas);
        }


        #region Helper Methods

        private List<SelectListItem> FillMeses()
        {
            List<SelectListItem> Meses = new List<SelectListItem>();

            Meses.Add(new SelectListItem
            {
                Text = "Enero",
                Value = "1"
            });

            Meses.Add(new SelectListItem
            {
                Text = "Febrero",
                Value = "2"
            });

            Meses.Add(new SelectListItem
            {
                Text = "Marzo",
                Value = "3"
            });

            Meses.Add(new SelectListItem
            {
                Text = "Abril",
                Value = "4"
            });

            Meses.Add(new SelectListItem
            {
                Text = "Mayo",
                Value = "5"
            });

            Meses.Add(new SelectListItem
            {
                Text = "Junio",
                Value = "6"
            });

            Meses.Add(new SelectListItem
            {
                Text = "Julio",
                Value = "7"
            });

            Meses.Add(new SelectListItem
            {
                Text = "Agosto",
                Value = "8"
            });

            Meses.Add(new SelectListItem
            {
                Text = "Septiembre",
                Value = "9"
            });

            Meses.Add(new SelectListItem
            {
                Text = "Octubre",
                Value = "10"
            });

            Meses.Add(new SelectListItem
            {
                Text = "Noviembre",
                Value = "11"
            });

            Meses.Add(new SelectListItem
            {
                Text = "Diciembre",
                Value = "12"
            });

            return Meses;
        }

        private List<SelectListItem> FillAnios()
        {
            List<SelectListItem> anios = new List<SelectListItem>();
            int i = 2010;
            while (i <= DateTime.Now.Year)
            {
                anios.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                i++;
            }
            
            return anios;
        }

        #endregion

    }


}
