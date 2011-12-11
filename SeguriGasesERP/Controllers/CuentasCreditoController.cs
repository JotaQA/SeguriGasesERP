using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;

namespace SeguriGasesERP.Controllers
{
    [Authorize(Roles = "Creditista")]
    public class CuentasCreditoController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /CuentasCredito/

       

        public ActionResult Index()
        {
            var cuentas = db.CuentasCredito.Include("Cliente").ToList();
            foreach (var item in cuentas)
            {
                item.Cliente = db.Clientes.Find(item.IdCliente);
            }
            return View(cuentas);
        }

        [HttpPost]
        public ActionResult Index(string word)
        {


            List<CuentaCredito> buscados = db.CuentasCredito.ToList();
            foreach (var item in buscados)
            {
                item.Cliente = db.Clientes.Find(item.IdCliente);
            }
            var b = buscados.AsQueryable();

            var bs = b.Where(c => c.Cliente.NombreCliente.ToLower().Contains(word.ToLower()));

            return View(bs);
        }

        //
        // GET: /CuentasCredito/Details/5

        public ActionResult Details(int id)
        {
            /*Vamos a sacar el estado de cuenta*/
            //Recuperamos la cuenta de credito
            var cuenta = db.CuentasCredito.Include("Ventas").Single( c => c.ID == id);
            /* Necesitamos la lista de todas las compras que se han ido a credito */
            List<Venta> ventas = cuenta.Ventas;

            /*De cada venta necesitamos los pagos*/
            List< List <Pago> > pagos = new List<List<Pago>>();

            foreach( var item in ventas)
            {
                List<Pago> pagoVenta = db.Pagos.Where(p => p.IdVenta == item.ID).ToList();
                pagos.Add(pagoVenta); 
                
            }

            ViewBag.Ventas = ventas;
            ViewBag.Pagos = pagos;
            ViewBag.Deuda = cuenta.getDeuda();

            return View(cuenta);
        }
        [Authorize(Roles = "Gerente")]
        public ActionResult Abonar(int idCuenta, int idVenta)
        {
            //Vamos a registrar un pago, para esto necesitamos sacar el resumen de la cuenta y de la venta correspondiente

            //Informacion Básica
            CuentaCredito Cuenta = db.CuentasCredito.Find(idCuenta);
            Venta venta = db.Ventas.Find(idVenta);
            List<Pago> Pagos = db.Pagos.Where(p => p.IdVenta == idVenta).ToList();
            Cliente cliente = db.Clientes.Find(Cuenta.IdCliente);

            
            ViewBag.Venta = venta;
            ViewBag.Pagos = Pagos;
            ViewBag.Cliente = cliente;
            ViewBag.Deuda = Cuenta.getDeuda();

            return View(Cuenta);

        }

        [HttpPost]
        public ActionResult Abonar(int idCuenta, int idVenta, decimal Monto, string Tipo)
        {

            //Informacion Básica
            CuentaCredito Cuenta = db.CuentasCredito.Find(idCuenta);
            Venta venta = db.Ventas.Find(idVenta);
            List<Pago> Pagos = db.Pagos.Where(p => p.IdVenta == idVenta).ToList();
            Cliente cliente = db.Clientes.Find(Cuenta.IdCliente);

            try
            {
                Pago newPago = new Pago
                {
                    Cliente = cliente,
                    FechaPago = DateTime.Now,
                    IdCliente = cliente.ID,
                    IdVenta = idVenta,
                    Monto = Monto,
                    TipoPago = Tipo,
                    Venta = venta
                };

                db.Pagos.Add(newPago);            

                db.SaveChanges();

                Pagos = db.Pagos.Where(p => p.IdVenta == idVenta).ToList();

                decimal abonos = 0;

                foreach (var pago in Pagos)
                {
                    abonos += pago.Monto;
                }

                if (abonos >= venta.Total)
                {
                    venta.Liquidado = true;
                    db.SaveChanges();
                }

                

                return RedirectToAction("Details/" + idCuenta);
            }
            catch
            {
                ViewBag.Venta = venta;
                ViewBag.Pagos = Pagos;
                ViewBag.Cliente = cliente;
                ViewBag.Deuda = Cuenta.getDeuda();

                return View(Cuenta);
            }
        }

        //
        // GET: /CuentasCredito/Create

        public ActionResult Create()
        {
            /*Recuperamos a los clientes que no tienen una cuenta de credito existente*/
            var CuentC = db.CuentasCredito.ToList();
            var Clientes = db.Clientes.OrderBy( c => c.NombreCliente).ToList();

            List<Cliente> ClientesCuentas = new List<Cliente>();

            foreach (var item in CuentC)
            {
                ClientesCuentas.Add(db.Clientes.Find(item.IdCliente));
            }

                        
            ViewBag.Clientes = Clientes.Except(ClientesCuentas);
           
                                      
            var CuentaCredito = new CuentaCredito();
            CuentaCredito.FechaCreado = DateTime.Now;
            return View(CuentaCredito);
        } 

        //
        // POST: /CuentasCredito/Create

        [HttpPost]
        public ActionResult Create(CuentaCredito newCuenta)
        {
            if (ModelState.IsValid)
            {
                //Guardamos la Cuenta de Credito
                newCuenta.FechaCreado = DateTime.Now;
                var existente = db.CuentasCredito.Where(c => c.IdCliente == newCuenta.IdCliente);
                if (existente.Count() > 0)
                    return RedirectToAction("Index");
                db.CuentasCredito.Add(newCuenta);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                var CL = from clNO in db.CuentasCredito.Include("Clientes")
                         select clNO.Cliente.ID;
                var Clientes = from clientes in db.Clientes
                               where !(CL.Any(cn => cn == clientes.ID))
                               select clientes;

                ViewBag.Clientes = Clientes; 

                return View(newCuenta);

            }
        }
        
        //
        // GET: /CuentasCredito/Edit/5
 
        public ActionResult Edit(int id)
        {
            /*Recuperamos a los clientes que no tienen una cuenta de credito existente*/
            var CuentC = db.CuentasCredito.ToList();

            var cuenta = db.CuentasCredito.Find(id);

            var Clientes = db.Clientes.Where(c => c.ID == cuenta.IdCliente).ToList();

            List<Cliente> ClientesCuentas = new List<Cliente>();

            foreach (var item in CuentC)
            {
                ClientesCuentas.Add(db.Clientes.Find(item.IdCliente));
            }


            ViewBag.Clientes = Clientes;

            
            return View(cuenta);
        }

        //
        // POST: /CuentasCredito/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var cuenta = db.CuentasCredito.Find(id);
            /*Solo mandaremos al cliente en si, ya que no se puede cambiar el cliente en la edicion*/
            
            var Clientes = db.Clientes.Where(c => c.ID == cuenta.IdCliente ).ToList();


            ViewBag.Clientes = Clientes;

            

            if (TryUpdateModel(cuenta))
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cuenta);
        }

        //
        // GET: /CuentasCredito/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /CuentasCredito/Delete/5
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
        
        /*Desplegamos la informacion de la venta que se pretende mandar a crédito*/
        [Authorize(Roles = "Gerente")]
        public ActionResult MandarACredito(int id)
        {
            var venta = db.Ventas.Find(id);
            var CuentaCredito = db.CuentasCredito.Where(c => c.IdCliente == venta.IdCliente);
            if(CuentaCredito.Count() != 1)
                return RedirectToAction("Error", new { Error = "La venta seleccionada, ya está dada de alta en crédito" });
            return View(venta);
        }

        [HttpPost]
        public ActionResult MandarACredito(int id, int basura = 1)
        {
            /*
             * Mandaremos a crédito la venta seleccionada
             */
           
           var venta = db.Ventas.Find(id);
            if(venta.Liquidado == false)
                return RedirectToAction("Error", new { Error = "La venta seleccionada, ya está dada de alta en crédito"});
            //Empezamos a pasarla pasando el campo de liquidado a falso
            var cCredito = db.CuentasCredito.Single(c => c.IdCliente == venta.IdCliente);

            venta.Liquidado = false;

            if (cCredito.Ventas.Count == 0)
            {
                cCredito.Ventas = new List<Venta>();
                cCredito.Ventas.Add(venta);
            }
            else
                cCredito.Ventas.Add(venta);
          
                db.SaveChanges();

                return RedirectToAction("Index");
            
        }
    }
}
