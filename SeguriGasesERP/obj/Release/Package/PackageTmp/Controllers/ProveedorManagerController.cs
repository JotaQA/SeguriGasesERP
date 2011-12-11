using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;
namespace SeguriGasesERP.Controllers
{
    [Authorize]
    public class ProveedorManagerController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();

        //
        // GET: /ProveedorManager/

        public ActionResult Index()
        {
            var proveedores = db.Proveedores.OrderBy(o => o.RazonSocial); 
            return View(proveedores);
        }

        //
        // GET: /ProveedorManager/Details/5

        public ActionResult Details(int id)
        {
            var proveedor = db.Proveedores.Find(id);
            return View(proveedor);
        }

        //
        // GET: /ProveedorManager/Create
        [Authorize(Roles = "Capturista")]
        public ActionResult Create()
        {
            var proveedor = new Proveedor();
            return View(proveedor);
        } 

        //
        // POST: /ProveedorManager/Create

        [HttpPost]
        public ActionResult Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                //Guardamos Proveedor
                db.Proveedores.Add(proveedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }
        
        //
        // GET: /ProveedorManager/Edit/5
        [Authorize(Roles = "Capturista")]
        public ActionResult Edit(int id)
        {
            var proveedor = db.Proveedores.Find(id);
            return View(proveedor);
        }

        //
        // POST: /ProveedorManager/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var proveedor = db.Proveedores.Find(id);

            if (TryUpdateModel(proveedor))
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        //
        // GET: /ProveedorManager/Delete/5
        [Authorize(Roles = "Capturista")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ProveedorManager/Delete/5

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
