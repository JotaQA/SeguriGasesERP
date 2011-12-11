using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;

namespace SeguriGasesERP.Controllers
{
    [Authorize]
    public class CategoriasManagerController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /CategoriasManager/

        public ActionResult Index()
        {
            var categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();
           
            return View(categorias);
        }

        //
        // GET: /CategoriasManager/Details/5
        
        public ActionResult Details(int id)
        {
            

            return View();
        }

        //
        // GET: /CategoriasManager/Create
        [Authorize(Roles = "Capturista")]
        public ActionResult Create()
        {
            var categoria = new Categoria();
            ViewBag.Categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();

            return View(categoria);
        } 

        //
        // POST: /CategoriasManager/Create

        [HttpPost]
        public ActionResult Create(Categoria newCategoria)
        {
            if (ModelState.IsValid)
            {
                db.Categorias.Add(newCategoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();
            return View(newCategoria);
        }
        
        //
        // GET: /CategoriasManager/Edit/5
        [Authorize(Roles = "Capturista")]
        public ActionResult Edit(int id)
        {
            var categoria = db.Categorias.Find(id);
            ViewBag.Categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();
            return View(categoria);
        }

        //
        // POST: /CategoriasManager/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var categoria = db.Categorias.Find(id);
            if (TryUpdateModel(categoria))
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();
            return View(categoria);
         
        }

        //
        // GET: /CategoriasManager/Delete/5
        [Authorize(Roles = "Capturista")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /CategoriasManager/Delete/5

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
