using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;

namespace SeguriGasesERP.Controllers
{
    [Authorize(Roles = "Almacenista")]
    public class OrdenesDeComprasController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /OrdenesDeCompras/

        /*Aqui desplegaremos las ordenes de compras pendientes para cada sucursal*/
        public ActionResult Index()
        {      
            var Ordenes = db.OrdenesDeCompra.Where(o => o.Activa == true );

            List<OrdenDeCompra> ordenes = Ordenes.ToList();

            foreach (var item in ordenes)
            {
                item.Proveedor = db.Proveedores.Find(item.IdProveedor);
                item.Sucursal = db.Sucursales.Find(item.IdSucursal);
            
            }
            return View(ordenes);
        }


        public ActionResult Create(int NoProductos)
        {
            var Sucursales = db.Sucursales.Where(s => s.Usuarios.Any(u => u.Username == HttpContext.User.Identity.Name));
            int noProductos = NoProductos;
            if (noProductos == 0)
            {
                return RedirectToAction("Error", new { Error = "Debes indicar el numero de productos a incluir en la orden de compra"});
            }

            ViewBag.Sucursales = Sucursales.ToList();
            ViewBag.Proveedores = db.Proveedores.OrderBy(p => p.RazonSocial);
            ViewBag.Productos = db.Productos.OrderBy(p => p.Nombre);
            ViewBag.noProductos = noProductos;
            var orden = new OrdenDeCompra();
            return View(orden);
        }

        [HttpPost]
        public ActionResult Create(OrdenDeCompra orden)
        {
            int IdSucursal = int.Parse(Request.Form["IdSucursal"]);
            int IdProveedor = int.Parse(Request.Form["IdProveedor"]);
            int noProductos = int.Parse(Request.Form["noProductos"]);

            if (IdSucursal == 0)
            {
                return RedirectToAction("Error", new { Error = "Debes indicar la sucursal de la cual se emitira la orden de compra" });
            }
            if (IdProveedor == 0)
            {
                return RedirectToAction("Error", new { Error = "Debes indicar el Proveedor al cual" });
            }


            orden.IdProveedor = IdProveedor;
            orden.IdSucursal = IdSucursal;
            orden.FechaGenerada = DateTime.Now;
            orden.Activa = true;


            if (ModelState.IsValid)
            {
                //Guardamos la orden
                db.OrdenesDeCompra.Add(orden);
                db.SaveChanges();
                int IdOrden = orden.ID;
                
                orden = db.OrdenesDeCompra.Find(IdOrden);
                int i = 0;
                
                /*
                 * Guardamos cada uno de los productos
                 */
                orden.Productos = new List<ProductoOrden>();
                for (i = 1; i <= noProductos; i++)
                {
                    string labelId = "IdProducto" + i;
                    string labelCantidad = "CantidadProducto" + i;
                    int idProd = int.Parse(Request.Form[labelId]);
                    decimal cantidad = decimal.Parse(Request.Form[labelCantidad]);

                    var PO = new ProductoOrden
                    {
                        cantidad = cantidad,
                        IdProducto = idProd,
                        IdOrden = orden.ID,
                        Producto = db.Productos.Find(idProd)
                        
                    };
                    
                    

                    orden.Productos.Add(PO);
                    db.ProductosOrden.Add(PO);
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
            var orden = db.OrdenesDeCompra.Find(id);
            ViewBag.Sucursal = db.Sucursales.Find(orden.IdSucursal);
            ViewBag.Proveedor = db.Proveedores.Find(orden.IdProveedor);
            ViewBag.Permiso = permisoSucursal(orden.IdSucursal);
                      
            
            return View(orden);
        }

        [HttpPost]
        public ActionResult Details(int id, int x = 1)
        {            
            var orden = db.OrdenesDeCompra.Find(id);
            if (permisoSucursal(orden.IdSucursal) == 0)
            {
                return RedirectToAction("Error", new { Error = "No puede aceptar el traslado ya que no cuenta con los permisos necesarios" });
            }


            //Actualizamos la orden de compra de acuerdo a la información dada
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
                    ViewBag.Sucursal = db.Sucursales.Find(orden.IdSucursal);
                    ViewBag.Proveedor = db.Proveedores.Find(orden.IdProveedor);
                    return View(orden);
                }
            }

            orden.Activa = false;
            orden.usuario = HttpContext.User.Identity.Name;
            OrdenDeCompra Orden = orden;
            /* Relacionamos al proveedor con el producto */
            foreach (ProductoOrden prod in Orden.Productos)
            {
               /*Buscamos si el producto ya había sido vendido por el proveedor*/
                List<Proveedor> Prove = db.Proveedores.Where(p => p.ID == Orden.ID && p.Productos.Any(pp => pp.ID == prod.ID)).ToList();

                if (Prove.Count == 0)
                {
                    //Creamos insertamos un proveedor a la lista de productos
                    var provedor = db.Proveedores.Find(Orden.IdProveedor);
                    var prodI = db.Productos.Find(prod.IdProducto);
                    if (provedor.Productos == null)
                        provedor.Productos = new List<Producto>();
                    if (prodI.Proveedores == null)
                        prodI.Proveedores = new List<Proveedor>();

                    provedor.Productos.Add(prodI);
                    prodI.Proveedores.Add(provedor);

                   
                    
                }
            }
            
            db.SaveChanges();

            //Ahora damos de alta los productos en el inventario

            foreach (var item in Orden.Productos)
            {
                try
                {

                    var Prod = db.ProductosSucursal.Single(p => p.IdProducto == item.IdProducto && p.IdSucursal == Orden.IdSucursal);
                    Prod.cantidad += item.cantidad;

                    //Generamos el movimiento

                    var Movimiento = new MovimientoAlmacen
                    {
                        Count = item.cantidad,
                        DescripcionMovimiento = "Alta por Orden de compra número " + Orden.ID,
                        FechaMovimiento = DateTime.Now,
                        IdOrednCompra = Orden.ID,
                        IdProducto = item.IdProducto,
                        IdSucursal = Orden.IdSucursal,
                        IdVenta = 0,
                        OrdenDeCompra = Orden,
                        Producto = item.Producto,
                        TipoMovimiento = "Ingreso"
                    };

                    db.MovimientosAlmacen.Add(Movimiento);

                    db.SaveChanges();

                }
                catch
                {
                    var Prod = new ProductoSucursal
                    {
                        cantidad = item.cantidad,
                        IdProducto = item.IdProducto,
                        IdSucursal = Orden.IdSucursal,
                        Producto = item.Producto
                    };

                    db.ProductosSucursal.Add(Prod);
                    //Generamos el movimiento

                    var Movimiento = new MovimientoAlmacen
                    {
                        Count = item.cantidad,
                        DescripcionMovimiento = "Alta por Orden de compra número " + Orden.ID,
                        FechaMovimiento = DateTime.Now,
                        IdOrednCompra = Orden.ID,
                        IdProducto = item.IdProducto,
                        IdSucursal = Orden.IdSucursal,
                        IdVenta = 0,
                        OrdenDeCompra = Orden,
                        Producto = item.Producto,
                        TipoMovimiento = "Ingreso",
                        username = HttpContext.User.Identity.Name
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
