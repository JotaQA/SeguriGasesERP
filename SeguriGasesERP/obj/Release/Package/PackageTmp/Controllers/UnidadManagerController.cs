using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;

namespace SeguriGasesERP.Controllers
{
    [Authorize]
    public class UnidadManagerController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /UnidadManager/

        public ActionResult Index()
        {
            var unidades = db.Unidades.OrderBy(c => c.Nombre).ToList();
            return View(unidades);
        }



        //
        // GET: /UnidadManager/Create
        [Authorize(Roles = "Capturista")]
        public ActionResult Create()
        {
            var unidad = new Unidad();            
            return View(unidad);
        } 

        //
        // POST: /UnidadManager/Create

        [HttpPost]
        public ActionResult Create(Unidad newUnidad)
        {
            if (ModelState.IsValid)
            {
                db.Unidades.Add(newUnidad);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(newUnidad);
        }
        
        //
        // GET: /UnidadManager/Edit/5
        [Authorize(Roles = "Capturista")]
        public ActionResult Edit(int id)
        {
            var unidad = db.Unidades.Find(id);

            return View(unidad);
        }

        //
        // POST: /UnidadManager/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var unidad = db.Unidades.Find(id);

            if (TryUpdateModel(unidad))
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unidad);
        }

        //
        // GET: /UnidadManager/Delete/5
        [Authorize(Roles = "Capturista")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /UnidadManager/Delete/5

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
