using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;

namespace SeguriGasesERP.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ComprasController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /Compras/

        public ActionResult Index()
        {
            var PS = db.ProductosSucursal.OrderBy(s => s.IdSucursal);
            return View(PS);
        }

        public ActionResult Create()
        {
            var compraNueva = new ProductoSucursal();
            ViewBag.Productos = db.Productos.OrderBy(p => p.Nombre);
            ViewBag.Sucursales = db.Sucursales.OrderBy(s => s.Nombre);
            return View(compraNueva);
        }

        [HttpPost]
        public ActionResult Create(ProductoSucursal newPS)
        {
            ViewBag.Productos = db.Productos.OrderBy(p => p.Nombre);
            ViewBag.Sucursales = db.Sucursales.OrderBy(s => s.Nombre);

            if (ModelState.IsValid)
            {
                db.ProductosSucursal.Add(newPS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Http://www.google.com");


        }

        //
        // GET: /UnidadManager/Edit/5

        public ActionResult Edit(int id)
        {
            var ProductoSucursal = db.ProductosSucursal.Find(id);

            return View(ProductoSucursal);
        }

        //
        // POST: /UnidadManager/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var ProductoSucursal = db.ProductosSucursal.Find(id);

            if (TryUpdateModel(ProductoSucursal))
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ProductoSucursal);
        }

    }
}
