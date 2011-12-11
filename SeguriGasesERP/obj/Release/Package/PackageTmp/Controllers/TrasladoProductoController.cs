using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;

namespace SeguriGasesERP.Controllers
{
    [Authorize(Roles = "Almacenista")]
    public class TrasladoProductoController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /TrasladoProducto/

        public ActionResult Index()
        {
            List<OrdenTraslado> traslados = db.Traslados.Where(t=> t.Activa == true).OrderBy(t => t.FechaGenerada).ToList();
            foreach (var item in traslados)
            {
                item.SucursalDestino = new Sucursal();
                item.SucursalDestino = db.Sucursales.Find(item.IdSucursalDestino);
                item.SucursalOrigen = new Sucursal();
                item.SucursalOrigen = db.Sucursales.Find(item.IdSucursalOrigen);
            }
            var tt = traslados;
            return View(tt);
        }

        public ActionResult Create(int NoProductos)
        {
            var Sucursales = db.Sucursales.Where(s => s.Usuarios.Any(u => u.Username == HttpContext.User.Identity.Name));

            var SucursalesDestino = db.Sucursales.OrderBy(s => s.Nombre);
            int noProductos = NoProductos;
            if (noProductos == 0)
            {
                return RedirectToAction("Error", new { Error = "Debes indicar el numero de productos a incluir en la orden de compra" });
            }

            ViewBag.SucursalesOrigen = Sucursales.ToList();
            ViewBag.SucursalesDestino = SucursalesDestino.ToList();
            ViewBag.Productos = db.Productos.OrderBy(p => p.Nombre);
            ViewBag.noProductos = noProductos;
            var traslado = new OrdenTraslado();
            return View(traslado);
        }

        [HttpPost]
        public ActionResult Create(OrdenTraslado orden)
        {
            int IdSucursalOrigen = int.Parse(Request.Form["IdSucursalOrigen"]);
            int IdSucursalDestino = int.Parse(Request.Form["IdSucursalDestino"]);
            int noProductos = int.Parse(Request.Form["noProductos"]);

            if (IdSucursalOrigen == 0 || IdSucursalDestino == 0)
            {
                return RedirectToAction("Error", new { Error = "Debes indicar las sucursales involucradas en el traslado" });
            }

            orden.IdSucursalDestino = IdSucursalDestino;
            orden.IdSucursalOrigen = IdSucursalOrigen;
            orden.FechaGenerada = DateTime.Now;
            orden.Activa = true;


            if (ModelState.IsValid)
            {
                //Guardamos la orden
                db.Traslados.Add(orden);
                db.SaveChanges();
                int IdOrden = orden.ID;

                orden = db.Traslados.Find(IdOrden);
                int i = 0;

                /*
                 * Guardamos cada uno de los productos
                 */
                orden.Productos = new List<ProductoTraslado>();
                for (i = 1; i <= noProductos; i++)
                {
                    string labelId = "IdProducto" + i;
                    string labelCantidad = "CantidadProducto" + i;
                    int idProd = int.Parse(Request.Form[labelId]);
                    decimal cantidad = decimal.Parse(Request.Form[labelCantidad]);

                    /*Checamos que haya en existencia para la sucursal origen el producto a trasladar*/
                    try
                    {
                        var ProdSuc = db.ProductosSucursal.Single(p => p.IdProducto == idProd && p.IdSucursal == IdSucursalOrigen);

                        if (ProdSuc.cantidad < cantidad)
                        {
                            db.Traslados.Remove(orden);
                            return RedirectToAction("Error", new { Error = "No existe la suficiente cantidad de productos para ser trasladada" });
                        }
                    }
                    catch
                    {
                        db.Traslados.Remove(orden);
                        return RedirectToAction("Error", new { Error = "No hay existencia de el producto " + idProd + " - No se guardará el traslado" });
                    }
                    /**/

                    var PO = new ProductoTraslado
                    {
                        cantidad = cantidad,
                        IdProducto = idProd,
                        Producto = db.Productos.Find(idProd),
                        IdTraslado = IdOrden

                    };



                    orden.Productos.Add(PO);
                    db.ProductosTraslado.Add(PO);
                    db.SaveChanges();

                }

                return RedirectToAction("Index");
            }

            return View(orden);

        }

        private int permisoSucursal(int IdSucursal)
        {
            List<Sucursal> SucAsig = db.PerfilUsuarios.Single(u => u.Username == HttpContext.User.Identity.Name).Sucursales;
            
            foreach (var item in SucAsig)
            {
                if (item.ID == IdSucursal)
                    return 1;
            }

            return 0;
        }

        public ActionResult Details(int id)
        {
            var orden = db.Traslados.Find(id);
            ViewBag.SucursalOrigen = db.Sucursales.Find(orden.IdSucursalOrigen);
            Sucursal s = db.Sucursales.Find(orden.IdSucursalDestino);
            ViewBag.SucursalDestino = s ;

            ViewBag.Permiso = permisoSucursal(s.ID); 


            return View(orden);
        }

        [HttpPost]
        public ActionResult Details(int id, int x = 1)
        {
            var orden = db.Traslados.Find(id);

            if (permisoSucursal(orden.IdSucursalDestino) == 0)
            {
                return RedirectToAction("Error", new { Error = "No puede aceptar el traslado ya que no cuenta con los permisos necesarios" });
            }

            //Actualizamos la orden de traslado de acuerdo a la información dada
            foreach (var item in orden.Productos)
            {
                string label = "cantidad" + item.IdProducto;
                try
                {
                    decimal cantidad = decimal.Parse(Request.Form[label]);
                    item.cantidad = cantidad;
                }
                catch
                {
                    ViewBag.SucursalOrigen = db.Sucursales.Find(orden.IdSucursalOrigen);
                    ViewBag.SucursalOrigen = db.Sucursales.Find(orden.IdSucursalDestino);
                    ViewBag.Proveedor = db.Proveedores.Find(orden.IdProveedor);
                    return View(orden);
                }
            }


            /*Registramos como procesada la orden*/
            orden.Activa = false;
            orden.usuario = HttpContext.User.Identity.Name;
            OrdenTraslado Orden = orden;

            db.SaveChanges();

            //Ahora damos de alta los productos en el inventario de sucursal destino y de baja en la sucursal origen

            foreach (var item in Orden.Productos)
            {
                /*Verificamos la existencia en el origen*/

                try
                {
                    //Averiguamos la existencia en la sucursal de origen
                    var existencia = db.ProductosSucursal.Single(p => p.IdProducto == item.IdProducto && p.IdSucursal == Orden.IdSucursalOrigen);

                    if (existencia.cantidad < item.cantidad)
                    {
                        orden.Activa = true;
                        db.SaveChanges();
                        return RedirectToAction("Error", new { Error = "No existe la suficiente cantidad de productos para ser trasladada" });
                    }
                }
                catch
                {
                    orden.Activa = true;
                    db.SaveChanges();
                    return RedirectToAction("Error", new { Error = "No existe la suficiente cantidad de productos para ser trasladada" });
                }

                /****************************************/

                try
                {
                    //Esta parte de ty se ejecutara si solo hay que actualizar cantidad
                    var ProdD = db.ProductosSucursal.Single(p => p.IdProducto == item.IdProducto && p.IdSucursal == Orden.IdSucursalDestino);
                    ProdD.cantidad += item.cantidad;

                    //Generamos el movimiento

                    var Movimiento = new MovimientoAlmacen
                    {
                        Count = item.cantidad,
                        DescripcionMovimiento = "Alta por Traslado desde sucursal " + Orden.IdSucursalOrigen,
                        FechaMovimiento = DateTime.Now,
                        IdOrednCompra = 0,
                        IdProducto = item.IdProducto,
                        IdSucursal = Orden.IdSucursalDestino,
                        IdVenta = 0,
                        Producto = item.Producto,
                        TipoMovimiento = "Ingreso"
                    };

                    db.MovimientosAlmacen.Add(Movimiento);
                    

                    var ProdO = db.ProductosSucursal.Single(p => p.IdProducto == item.IdProducto && p.IdSucursal == Orden.IdSucursalOrigen);
                    ProdO.cantidad -= item.cantidad;

                    //Generamos el movimiento

                    Movimiento = new MovimientoAlmacen
                    {
                        Count = item.cantidad,
                        DescripcionMovimiento = "Baja por Traslado hacia sucursal " + Orden.IdSucursalDestino,
                        FechaMovimiento = DateTime.Now,
                        IdOrednCompra = 0,
                        IdProducto = item.IdProducto,
                        IdSucursal = Orden.IdSucursalOrigen,
                        IdVenta = 0,
                        Producto = item.Producto,
                        TipoMovimiento = "Salida"
                    };

                    db.SaveChanges();

                }
                catch
                {
                    //Creamos un nuevo producto Sucursal, para darlo de alta en el inventario de la sucursal
                    var ProdD = new ProductoSucursal
                    {
                        cantidad = item.cantidad,
                        IdProducto = item.IdProducto,
                        IdSucursal = Orden.IdSucursalDestino,
                        Producto = db.Productos.Find(item.IdProducto),
                        Sucursal = db.Sucursales.Find(Orden.IdSucursalDestino)
                    };

                    db.ProductosSucursal.Add(ProdD);

                    //Generamos el movimiento

                    var Movimiento = new MovimientoAlmacen
                    {
                        Count = item.cantidad,
                        DescripcionMovimiento = "Alta por Traslado desde sucursal " + Orden.IdSucursalOrigen,
                        FechaMovimiento = DateTime.Now,
                        IdOrednCompra = 0,
                        IdProducto = item.IdProducto,
                        IdSucursal = Orden.IdSucursalDestino,
                        IdVenta = 0,
                        Producto = item.Producto,
                        TipoMovimiento = "Ingreso"
                    };

                    db.MovimientosAlmacen.Add(Movimiento);

                    //Damos de baja en el Origen
                    var ProdO = db.ProductosSucursal.Single(p => p.IdProducto == item.IdProducto && p.IdSucursal == Orden.IdSucursalOrigen);
                    ProdO.cantidad -= item.cantidad;

                    //Generamos el movimiento

                    Movimiento = new MovimientoAlmacen
                    {
                        Count = item.cantidad,
                        DescripcionMovimiento = "Baja por Traslado hacia sucursal " + Orden.IdSucursalDestino,
                        FechaMovimiento = DateTime.Now,
                        IdOrednCompra = 0,
                        IdProducto = item.IdProducto,
                        IdSucursal = Orden.IdSucursalOrigen,
                        IdVenta = 0,
                        Producto = item.Producto,
                        TipoMovimiento = "Salida"
                    };
                    db.MovimientosAlmacen.Add(Movimiento);
                    
                    db.SaveChanges();
                }
            }





            return RedirectToAction("Index");
        }

        public ActionResult Error(string Error)
        {
            ViewBag.Error = Error;
            return View();
        }

    }
}
