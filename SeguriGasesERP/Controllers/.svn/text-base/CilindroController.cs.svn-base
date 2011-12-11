using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;
namespace SeguriGasesERP.Controllers
{
    [Authorize]
    public class CilindroController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /Cilindro/

        public ActionResult Index()
        {
            var cilindros = db.Cilindros.OrderBy(c => c.TipoCilindro);
            return View(cilindros);
        }
        [Authorize(Roles = "Capturista")]
        public ActionResult Create()
        {
            var cilindro = new Cilindro();

            return View(cilindro);
        }

        [HttpPost]
        public ActionResult Create(Cilindro cilindro)
        {
            if (ModelState.IsValid)
            {
                //Guardamos el Cilindro
                db.Cilindros.Add(cilindro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cilindro);
        }
        [Authorize(Roles = "Capturista")]
        public ActionResult Editar(int id)
        {
            Cilindro cilindro = db.Cilindros.Find(id);

            return View(cilindro);
        }

        //
        //POST: /Cilindros/Editar/
        [HttpPost]
        public ActionResult Editar(int id, FormCollection collection)
        {
            var cilindro = db.Cilindros.Find(id);

            if (TryUpdateModel(cilindro))
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(cilindro);
            }
        }
    }
}
