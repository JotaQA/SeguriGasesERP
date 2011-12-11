using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;

namespace SeguriGasesERP.Controllers
{
    public class VentasController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /Ventas/

        public ActionResult Index()
        {
            List<Venta> ventas = db.Ventas.OrderBy(v => v.ClaveCFD).ToList();
            foreach (Venta venta in ventas)
            {
                venta.Cliente = db.Clientes.Find(venta.IdCliente);
                venta.Sucursal = db.Sucursales.Find(venta.IdSucursal);
            }
            return View(ventas);
        }

        public ActionResult Cancelar(int id)
        {
            /*Recuperamos los datos de la venta*/
            var venta = db.Ventas.Find(id);            
                venta.Cliente = db.Clientes.Find(venta.IdCliente);
                venta.Sucursal = db.Sucursales.Find(venta.IdSucursal);
            
            return View(venta);
        }

        [HttpPost]
        public ActionResult Cancelar(int id, int caca = 0)
        {
            /*Primero debemos recuperar la venta*/
            var venta = db.Ventas.Find(id);
            Venta Venta = venta;
            /*Debemos averiguar si está en crédito, si está a credito hay que quitarla*/
            var CuentaCredito = db.CuentasCredito.Where(c => c.IdCliente == venta.IdCliente);
            
            if (CuentaCredito.Count() == 1)
            {
                List<Venta> ventasCuenta = CuentaCredito.First().Ventas;
                foreach (var ventaCuenta in ventasCuenta)
                {
                    //SI encontramos la venta en la cuenta de credito, la quitamos
                    if (ventaCuenta.ID == id)
                    {
                        
                        ventasCuenta.Remove(ventaCuenta);
                        break;
                    }
                }

                CuentaCredito.First().Ventas = ventasCuenta;
            }
            if (CuentaCredito.Count() > 1)
                return RedirectToAction("Error", new { Error = "El cliente actual tiene más de una cuenta de credito" });
            

            /*Ahora debemos reingresar al inventario los productos*/
            List<ProductoVenta> productosVenta = db.ProductosVenta.Where(pv => pv.VentaId == id).ToList();
            foreach (var productoVenta in productosVenta)
            {
                //Buscamos la entrada de cada producto para sumarle la cantidad que se descargo
                var productoSucursal = db.ProductosSucursal.Single(p => p.IdProducto == productoVenta.ProductoId && p.IdSucursal == Venta.IdSucursal);
                productoSucursal.cantidad += productoVenta.Count;
                db.ProductosVenta.Remove(productoVenta);
            }

            /*Ahora quitaremos la venta de la base de datos*/

            db.Ventas.Remove(venta);

            db.SaveChanges();

            return RedirectToAction("Index");

        }

        public ActionResult Details(int id)
        {
            Venta venta = db.Ventas.Find(id);
            venta.Cliente = db.Clientes.Find(venta.IdCliente);
            venta.Sucursal = db.Sucursales.Find(venta.IdSucursal);
            venta.ProductosVenta = db.ProductosVenta.Where(p => p.VentaId == venta.ID).ToList();

            return View(venta);

        }

        public ActionResult Edit(int id)
        {
            var venta = db.Ventas.Find(id);
            
            return View(venta);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var venta = db.Ventas.Find(id);
            if (TryUpdateModel(venta))
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(venta);
        }
    }
}
