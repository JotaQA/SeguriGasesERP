using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;

namespace SeguriGasesERP.Controllers
{
    [Authorize]
    public class ProductoManagerController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /ProductoManager/
        // Listado de productos

        public ActionResult Index()
        {
            var productos = db.Productos
                .Include("Categoria").Include("Unidad")
                .ToList();
            
            return View(productos);
        }

        //
        // GET: /ProductoManager/Create
        //
        [Authorize(Roles = "Capturista")]
        public ActionResult Create()
        {
            var producto = new Producto();
            ViewBag.Unidades = db.Unidades.OrderBy(p => p.Nombre).ToList();
            ViewBag.Categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();
            return View(producto);
        }

        //
        // POST: /ProductoManager/Create/
        [HttpPost]
        public ActionResult Create(Producto newProducto)
        {
            if (ModelState.IsValid)
            {
                //Guardamos el producto
                newProducto.Unidad = db.Unidades.Find(newProducto.IdUnidad);
                newProducto.Categoria = db.Categorias.Find(newProducto.IdCategoria);
                db.Productos.Add(newProducto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Unidades = db.Unidades.OrderBy(p => p.Nombre).ToList();
            ViewBag.Categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();
            return View(newProducto);
        }

        //GET: /ProductoManager/Edit/
        [Authorize(Roles = "Capturista")]
        public ActionResult Edit(int id)
        {
            var producto = db.Productos.Find(id);
            ViewBag.Unidades = db.Unidades.OrderBy(p => p.Nombre).ToList();
            ViewBag.Categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();
            return View(producto);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var producto = db.Productos.Find(id);
            if (TryUpdateModel(producto))
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Categorias = db.Categorias.OrderBy(g => g.Nombre).ToList();
                ViewBag.Unidades = db.Unidades.OrderBy(a => a.Nombre).ToList();
                return View(producto);
            }
        }

        public ActionResult Details(int id)
        {
            Producto producto = db.Productos.Find(id);
            
            /*Sacamos la lista de sucursales*/
            List<Sucursal> Sucursales = db.Sucursales.OrderBy(s => s.Nombre).ToList();
            /*Sacamos una lista de decimales correspondientes a las existencias por sucursal*/
            List<decimal> Existencias = new List<decimal>();

            foreach (Sucursal sucursal in Sucursales)
            {
                try
                {
                    var existencia = db.ProductosSucursal.Single(ps => ps.IdProducto == id && ps.IdSucursal == sucursal.ID);
                    Existencias.Add(existencia.cantidad);
                }
                catch
                {
                    Existencias.Add(0.0M);
                }
            }

            ViewBag.Sucursales = Sucursales;
            ViewBag.Existencias = Existencias;

            return View(producto);
        }

    }
}
