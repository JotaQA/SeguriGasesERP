using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;

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

            db.Sucursales.Add(Sucursal);

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

            db.Unidades.Add(u1);
            db.Unidades.Add(u2);

            TryUpdateModel(Cat1);         
            TryUpdateModel(Cat2);

            TryUpdateModel(u1);
            TryUpdateModel(u2);

            Producto p1 = new Producto
            {
                Activo = true,
                Clave = "SEGOX2011",
                Descripcion = "Oxigeno Industrial de 8.5",
                FotoUrl = "",
                IdCategoria = 1,                
                IdUnidad = 1,
                Nombre = "Oxigeno Industrial de 8.5",
                PrecioLista = 88.9M
            };

            Producto p2 = new Producto
            {
                Activo = true,
                Clave = "SEGSOL2011",
                Descripcion = "Soldadura Grinox",
                FotoUrl = "",
                IdCategoria = 2,
                IdUnidad = 2,
                Nombre = "Soldadura Grinox",
                PrecioLista = 55.00M,
            };

            db.Productos.Add(p1);
            db.Productos.Add(p2);

            db.SaveChanges();

            ProductoSucursal ps1 = new ProductoSucursal
            {
                cantidad = 100.00M,
                IdProducto = 1,
                IdSucursal = 1,
                Producto = db.Productos.Find(1),
                Sucursal = db.Sucursales.Find(1)
            };

            ProductoSucursal ps2 = new ProductoSucursal
            {
                cantidad = 100.00M,
                IdProducto = 2,
                IdSucursal = 1,
                Producto = db.Productos.Find(2),
                Sucursal = db.Sucursales.Find(1)
            };

            db.ProductosSucursal.Add(ps1);
            db.ProductosSucursal.Add(ps2);

            db.SaveChanges();

            

            TryUpdateModel(p1);
            TryUpdateModel(p2);

            

            

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
                //Parseamos la cadena
                string[] words = line.Split(';');
                string Descripcion = words[1];
                string Clave = words[0];
                string Nombre = words[1];
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
            return RedirectToAction("/");


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
                    RfcReceptor = words[0],
                    NombreReceptor = words[1],
                    NombreCliente = words[1],
                    Contacto = words[15],
                    Telefono = words[3],
                    CalleReceptor = words[6],
                    NoExteriorReceptor = words[7],
                    ColoniaReceptor = words[8],
                    MunicipioReceptor = words[9],
                    EstadoReceptor = words[10],
                    PaisReceptor = "México",
                    CodigoPostalReceptor = words[11],
                    Email = words[12],
                    NoInteriorReceptor = words[14]

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
