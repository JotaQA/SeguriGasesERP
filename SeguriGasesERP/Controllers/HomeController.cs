using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeguriGasesERP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HttpContext.Session["Valido"] = "HOLA ME LLAMO Mane";

            ViewBag.Message = "Bienvenido al ERP de SeguriGases!!!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Menu()
        {
            return View();
        }
    }
}
