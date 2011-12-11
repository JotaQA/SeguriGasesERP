using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;
using System.Web.Security;

namespace SeguriGasesERP.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class DefaultDataController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /DefaultData/

        public ActionResult Index()
        {
            Cliente Cliente = new Cliente
            {
                CalleReceptor = "Cerrada Alvaro Obregon",
                CodigoPostalReceptor = "72760",
                ColoniaReceptor = "Fraccionamiento Horizontal El Campanario",
                Contacto = "Mane",
                Email = "jmanuelh@gmail.com",
                EstadoReceptor = "Puebla",
                MunicipioReceptor = "San Pedro Cholula",
                NoExteriorReceptor = "2",
                NombreCliente = "Jose Manuel Heredia Hidalgo",
                NombreReceptor = "Jose Manuel Heredia Hidalgo",
                PaisReceptor = "México",
                RfcReceptor = "HEHM880429QG7",
                Telefono = "2222474817"

            };

            db.Clientes.Add(Cliente);

            Sucursal Sucursal = new Sucursal
            {
                Cuenta ="SEG0809151GA",
                DomicilioFiscal = "Blvd. Forjadores de Puebla #8A Colonia Manantiales. San Pedro Cholula, Puebla. CP 72760",
                Nombre = "Forjadores",
                Password ="S3GUR1GAS3S",
                RFC = "SEG0809151GA",
                Telefono = "22224847474"
            };

            Sucursal Sucursal2 = new Sucursal
            {
                Cuenta = "SATELITE",
                DomicilioFiscal = "Av. Justo Sierra No. 1634 Colonia Satelite, Puebla, Pue. C.P. 72320",
                Nombre = "SATELITE",
                Password = "S3GUR1GAS3S",
                RFC = "SEG0809151GA",
                Telefono = "22224847474"
            };

            db.Sucursales.Add(Sucursal);
            db.Sucursales.Add(Sucursal2);

            TryUpdateModel(Sucursal);

            Categoria Cat1 = new Categoria
            {
                Nombre = "Gases"
            };
            Categoria Cat2 = new Categoria
            {
                Nombre = "Soldadura"
            };

            db.Categorias.Add(Cat1);
            db.Categorias.Add(Cat2);

            Unidad u1 = new Unidad
            {
                Nombre = "Kg"
            };

            Unidad u2 = new Unidad
            {
                Nombre = "M3"
            };

            Unidad u3 = new Unidad
            {
                Nombre = "PZA"
            };

            db.Unidades.Add(u1);
            db.Unidades.Add(u2);
            db.Unidades.Add(u3);

            TryUpdateModel(Cat1);         
            TryUpdateModel(Cat2);

            TryUpdateModel(u1);
            TryUpdateModel(u2);

            string[] roles = { "Administrador", "Gerente", "Almacenista", "Facturista", "Creditista", "Capturista" };
            Roles.CreateRole("Administrador");
            Roles.CreateRole("Gerente");
            Roles.CreateRole("Almacenista");
            Roles.CreateRole("Facturista");
            Roles.CreateRole("Creditista");
            Roles.CreateRole("Capturista");
            Roles.AddUserToRoles("jmanuelh", roles);
            

            db.SaveChanges();

            

            

            return View();
        }

        public ActionResult Productos()
        {
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(@"c:\producto.csv");
            while ((line = file.ReadLine()) != null)
            {
                if (line.Equals("") || line == null)
                    continue;
                //Parseamos la cadena
                string[] words = line.Split(';');
                string Descripcion = words[1] ?? "N/A";
                
                string Clave = words[0] ?? "N/A";
                string Nombre = words[1] ?? "N/A";
                decimal Precio = decimal.Parse(words[4]);
                decimal Costo = decimal.Parse(words[3]);

                var producto = new Producto
                {
                    Activo = true,
                    Descripcion = Descripcion,
                    Clave = Clave,
                    Nombre = Nombre,
                    PrecioLista = Precio,
                    Costo = Costo
                };

                db.Productos.Add(producto);
                db.SaveChanges();

            }

            file.Close();

            // Suspend the screen.
            return RedirectToAction("google.com");


        }

        public string limpia(string rfc)
        {
            char[] arr = rfc.ToUpper().ToCharArray();

            string RFC = "";
            int i = 0;
            foreach (char x in arr)
            {
                if ((x >= 'A' && x <= 'Z') || (x >= '0' && x <= '9'))
                {
                    RFC += x; 
                    i++;
                }
            }
          
            
            return RFC;
        }

        public string validar(string word)
        {
            if (word.Equals("") || word == null)
            {
                return "N/A";
            }

            return word;

        }

        public ActionResult InventarioForjadores()
        {
            string line;
            Sucursal suc = db.Sucursales.Single(s => s.Nombre.Equals("Forjadores"));

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(@"c:\INVENTARIO_F.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (line.Equals("") || line == null)
                    continue;
                //Parseamos la cadena
                string[] words = line.Split(',');
                string Clave = words[0];
                string Nombre = words[1];
                decimal existencia = decimal.Parse(words[2]);
                List<Producto> prods = db.Productos.Where(p => p.Clave.Trim().Equals(Clave.Trim()) || p.Nombre.Trim().Equals(Nombre.Trim())).ToList();
                if (prods.Count == 0 || prods == null)
                    continue;

                Producto prod = prods.First();

                var producto = new ProductoSucursal
                {
                    cantidad = existencia,
                    IdProducto = prod.ID,
                    IdSucursal = suc.ID,
                    Producto = prod,
                    Sucursal = suc
                };

                db.ProductosSucursal.Add(producto);
                db.SaveChanges();

            }

            file.Close();

            // Suspend the screen.
            return RedirectToAction("Google.com");

        }

        public ActionResult InventarioSatelite()
        {
            string line;
            Sucursal suc = db.Sucursales.Single(s => s.Nombre.ToLower().Equals("satelite"));

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(@"c:\INVENTARIO_SATELITE.CSV");
            while ((line = file.ReadLine()) != null)
            {
                if (line.Equals("") || line == null)
                    continue;
                //Parseamos la cadena
                string[] words = line.Split(',');
                string Clave = words[0];
                string Nombre = words[1];
                decimal existencia = decimal.Parse(words[2]);
                List<Producto> prods = db.Productos.Where(p => p.Clave.Trim().Equals(Clave.Trim()) || p.Nombre.Trim().Equals(Nombre.Trim())).ToList();
                if (prods.Count == 0 || prods == null)
                    continue;

                Producto prod = prods.First();

                var producto = new ProductoSucursal
                {
                    cantidad = existencia,
                    IdProducto = prod.ID,
                    IdSucursal = suc.ID,
                    Producto = prod,
                    Sucursal = suc
                };

                db.ProductosSucursal.Add(producto);
                db.SaveChanges();

            }

            file.Close();

            // Suspend the screen.
            return RedirectToAction("Google.com");

        }

        public ActionResult Clientes()
        {

            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(@"c:\cliente.csv");
            while ((line = file.ReadLine()) != null)
            {
                //Parseamos la cadena
                string[] words = line.Split(';');
                var cliente = new Cliente
                {
                    RfcReceptor = limpia(words[0]),
                    NombreReceptor = validar(words[1]),
                    NombreCliente = validar(words[1]) ,
                    Contacto = validar(words[15]) ,
                    Telefono = words[3] ,
                    CalleReceptor = validar(words[6]),
                    NoExteriorReceptor = validar(words[7]) ,
                    ColoniaReceptor = validar(words[8] ) ,
                    MunicipioReceptor = validar(words[9])  ,
                    EstadoReceptor = validar(words[10]),
                    PaisReceptor = "México",
                    CodigoPostalReceptor = validar(words[11]),
                    Email = words[12] ,
                    NoInteriorReceptor = words[14] ?? "N/A"

                };
                db.Clientes.Add(cliente);
                db.SaveChanges();

            }

            file.Close();

            // Suspend the screen.
            return RedirectToAction("http://www.google.com");
            
        }

    }
}
