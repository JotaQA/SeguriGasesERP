using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;

namespace SeguriGasesERP.Controllers
{
    [Authorize]
    public class DotacionesController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /Dotaciones/
        //Lista de las dotaciones existentes
        public ActionResult Index()
        {
            var Dotaciones = db.DotacionesCilindro.OrderBy(d => d.IdCliente);
            List<DotacionCilindros> dotaciones = Dotaciones.ToList();
            return View(dotaciones);
        }
        [Authorize(Roles = "Almacenista")]
        public ActionResult Create(int NoCilindros)
        {            
            int noCilindros = NoCilindros;
            if (noCilindros == 0)
            {
                return RedirectToAction("Error", new { Error = "Debes indicar el numero de cilindros a incluir en la Dotacion nueva" });
            }


            ViewBag.Clientes = db.Clientes.OrderBy(c => c.NombreCliente);
            ViewBag.Cilindros = db.Cilindros.OrderBy(c => c.TipoCilindro);
            ViewBag.noCilindros = noCilindros;
            var dotacion = new DotacionCilindros();
            return View(dotacion);
        }

        [HttpPost]
        public ActionResult Create(DotacionCilindros dotacion)
        {
            int IdCliente = int.Parse(Request.Form["IdCliente"]);
            int noCilindros = int.Parse(Request.Form["noCilindros"]);

            if (IdCliente == 0)
            {
                return RedirectToAction("Error", new { Error = "Debes indicar la el cliente a quien se asignará la dotacion" });
            }

            if(db.DotacionesCilindro.Where( d => d.IdCliente == IdCliente).Count() != 0)
            {
                return RedirectToAction("Error", new { Error = "Ya esxiste una dotacion para este cliente" });
            }

            
            dotacion.IdCliente = IdCliente;
            dotacion.Cliente = db.Clientes.Find(IdCliente);

            if (ModelState.IsValid)
            {
                //Guardamos la dotacion
                db.DotacionesCilindro.Add(dotacion);
                db.SaveChanges();
                int IdDotacion = dotacion.ID;

                dotacion = db.DotacionesCilindro.Find(IdDotacion);
                dotacion.Cilindros = new List<CilindroDotacion>();
                int i = 0;

                /*Guardamos cada uno de los cilindros*/

                for (i = 1; i <= noCilindros; i++)
                {
                    string labelId = "IdCilindro" + i;
                    string labelDeposito = "depositoCilindro" + i;

                    int idClilindro = int.Parse(Request.Form[labelId]);
                    decimal deposito = decimal.Parse(Request.Form[labelDeposito]);

                    Cilindro cilindro = db.Cilindros.Find(idClilindro);

                    var CD = new CilindroDotacion
                    {
                        IdCilindro = idClilindro,
                        Cilindro = cilindro,
                        IdDotacion = IdDotacion,
                        Dotacion = dotacion,
                        Deposito = deposito
                    };

                    dotacion.Cilindros.Add(CD);
                    db.CilindrosDotacion.Add(CD);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.Clientes = db.Clientes.OrderBy(c => c.NombreCliente);
                ViewBag.Cilindros = db.Cilindros.OrderBy(c => c.TipoCilindro);
                ViewBag.noCilindros = noCilindros;
                return View(dotacion);
            }

        }
        [Authorize(Roles = "Almacenista")]
        public ActionResult Ampliar(int id)
        {
            var dotacion = db.DotacionesCilindro.Find(id);
            ViewBag.Cilindros = db.Cilindros.OrderBy(c => c.TipoCilindro).ToList();
            return View(dotacion);
        }
        [HttpPost]
        public ActionResult Ampliar(DotacionCilindros dotacion)
        {
            /*Se asignará un nuevo cilindro a la dotcion*/
            int IdCilindro = int.Parse(Request.Form["IdCilindro"]);
            decimal Deposito = decimal.Parse(Request.Form["Deposito"]);
            Cilindro cilindro = db.Cilindros.Find(IdCilindro);
            var Dotacion = db.DotacionesCilindro.Include("Cilindros").Single(d => d.ID == dotacion.ID);

            /*Creamos el nuevo cilindro para la dotacion*/
            var CD = new CilindroDotacion
            {
                IdCilindro = IdCilindro,
                Cilindro = cilindro,
                IdDotacion = Dotacion.ID,
                Dotacion = Dotacion,
                Deposito = Deposito                
            };

            Dotacion.Cilindros.Add(CD);
            


            db.CilindrosDotacion.Add(CD);

            db.SaveChanges();

            return RedirectToAction("Index");

        }
        [Authorize(Roles = "Almacenista")]
        public ActionResult Details(int id)
        {
            var Dotacion = db.DotacionesCilindro.Include("Cilindros").Include("Cliente").Single(d => d.ID == id);

            return View(Dotacion);
        }
        [Authorize(Roles = "Almacenista")]
        public ActionResult Quitar(int id, int dotacion)
        {
            var Dotacion = db.DotacionesCilindro.Include("Cilindros").Single(d => d.ID == dotacion);
            var Cilindro = db.CilindrosDotacion.Single(c => c.ID == id);

            Dotacion.Cilindros.Remove(Cilindro);
            db.CilindrosDotacion.Remove(Cilindro);

            db.SaveChanges();


            return RedirectToAction("Details/" + dotacion);
        }

    }
}
