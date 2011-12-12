using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;
using System.Web.Security;


namespace SeguriGasesERP.Controllers
{
    
    [Authorize]
    public class BusquedaController : Controller
    {

        #region Globals

        private const int resultsPerPage = 10;

        #endregion

        SeguriGasesEntities db = new SeguriGasesEntities();

        //
        // GET: /Busqueda/

        public ActionResult Index(int? page)
        {          
            //Seteamos el valor de page de acuerdo al parametro, si recibimos nulo lo seteamos a cero
            page = page == null ? 0 : (int) page;

            //Numero de resultados a ignorar
            int elementsSkip = (int) page * resultsPerPage;

            //Sacamos el total de productos para realizar la paginacion
            int numProductos = db.Productos.Count();

            //Tomamos 10 productos a partir del numero de pagina, esto es page * 10
            List<Producto> productos = db.Productos.Take(resultsPerPage).Skip(elementsSkip).ToList();
            foreach (Producto producto in productos)
            {
                
                if(producto.IdUnidad != 0)
                   producto.Unidad = db.Unidades.Find(producto.IdUnidad);
                if (producto.IdCategoria != 0)
                    producto.Categoria = db.Categorias.Find(producto.IdCategoria);

            }

            return View(productos);
        }

        //POST: /Busqueda/
        [HttpPost]
        public ActionResult Index(string word)
        {
        
            
            List<Producto> buscados = (from productos in db.Productos
                           where ((productos.Nombre.Contains(word) || productos.Clave.Contains(word)) && productos.Activo == true)
                           select productos).ToList();

            foreach (Producto producto in buscados)
            {

                if (producto.IdUnidad != 0)
                    producto.Unidad = db.Unidades.Find(producto.IdUnidad);
                if (producto.IdCategoria != 0)
                    producto.Categoria = db.Categorias.Find(producto.IdCategoria);

            }

            return View(buscados);
        }

        public ActionResult Busqueda(String word)
        {
            
            var buscados = from productos in db.Productos
                           where productos.Nombre.Contains(word)
                           select productos;
            
            return View(buscados);
        }

        public ActionResult Detalles(int id)
        {
            var producto = db.Productos.Single(p => p.ID == id);

            return View(producto);

        }

        public ActionResult Existencias()
        {

            List<ProductoSucursal> buscados = db.ProductosSucursal.Include("Producto").Include("Sucursal").OrderBy(ps => ps.Producto.Nombre).ToList();

            foreach(var item in buscados)
            {
                item.Sucursal = db.Sucursales.Find(item.IdSucursal);
                item.Producto = db.Productos.Find(item.IdProducto);

            }

            return View(buscados);

        }

    }
}
