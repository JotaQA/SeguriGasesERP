using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;

namespace SeguriGasesERP.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class SucursalManagerController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /SucursalManager/

        public ActionResult Index()
        {
            var Sucursales = db.Sucursales.OrderBy(s => s.Nombre);
            return View(Sucursales);
        }

        //
        // GET: /SucursalManager/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SucursalManager/Create

        public ActionResult Create()
        {
            var Sucursal = new Sucursal();
            return View(Sucursal);
        } 

        //
        // POST: /SucursalManager/Create

        [HttpPost]
        public ActionResult Create(Sucursal newSucursal)
        {
            if (ModelState.IsValid)
            {
                db.Sucursales.Add(newSucursal);
                db.SaveChanges();

                return View("Index");
            }

            return View(newSucursal);
        }
        
        //
        // GET: /SucursalManager/Edit/5
 
        public ActionResult Edit(int id)
        {
            var sucursal = db.Sucursales.Find(id);
            
            return View(sucursal);
        }

        //
        // POST: /SucursalManager/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var sucursal = db.Sucursales.Find(id);
            var categoria = sucursal;
            if (TryUpdateModel(categoria))
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        //
        // GET: /SucursalManager/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /SucursalManager/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
