using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;

namespace SeguriGasesERP.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /Clientes/
        //Listado de Clientes

        public ActionResult Index()
        {
            var clientes = db.Clientes.ToList();
            return View(clientes);
        }

        //
        // GET: /Clientes/Editar/5
        //Edicion de Clientes
        [Authorize(Roles = "Capturista")]
        public ActionResult Editar(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
           
            return View(cliente);
        }

        //
        //POST: /Clientes/Editar/
        [HttpPost]
        public ActionResult Editar(int id, FormCollection collection)
        {
            var cliente = db.Clientes.Find(id);

            if (TryUpdateModel(cliente))
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(cliente);
            }
        }


        

        //
        // GET: /Clientes/Crear/
        // Controlador para crear un cliente
        [Authorize(Roles = "Capturista")]
        public ActionResult Crear()
        {
            
            var cliente = new Cliente();
            cliente.PaisReceptor = "México";
            return View(cliente);
        }

        //
        //POST: /Clientes/Crear/
        //Controlador que recibe el nuevo cliente a crear, lo valida y lo crea
        [HttpPost]
        public ActionResult Crear(Cliente newCliente)
        {
            
            if (ModelState.IsValid)
            {
                //Guardar el cliente
                
                db.Clientes.Add(newCliente);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(newCliente);
        }

        public ActionResult Details(int id)
        {
            Cliente cliente = db.Clientes.Find(id);

            return View(cliente);

        }
    }
}
