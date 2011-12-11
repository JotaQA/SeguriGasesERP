using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using SeguriGasesERP.Models;
using SeguriGasesERP.ViewModels;
using System.Web.Security;


namespace SeguriGasesERP.Models
{
    [DecimalPrecision(18, 2)] 
    public partial class CarritoDeCompras
    {
        [DecimalPrecision(18, 2)] 
        public const decimal IVA = (decimal)0.16;
        SeguriGasesEntities db = new SeguriGasesEntities();

        public string IdCarritoDeCompras { get; set; }

        public const string CartSessionKey = "CartId";

        public static CarritoDeCompras GetCarrito(HttpContextBase context)
        {
            var carrito = new CarritoDeCompras();
            carrito.IdCarritoDeCompras = context.User.Identity.Name;
            return carrito;
        }

        //Helper Simplifica las llamadas 
        public static CarritoDeCompras GetCarrito(Controller controller)
        {
            return GetCarrito(controller.HttpContext);
        }

        public void SumarAlCarrito(Producto producto, decimal Cantidad)
        {
            // Averiguamos si tenemos abierto un carrito para agregarle los productos
            // Si no hay carrito abirto creamos uno

            var elementoCarrito = db.Carritos.SingleOrDefault(
                                    c => c.CartId == IdCarritoDeCompras
                                        && c.ProductoId == producto.ID);
            if (elementoCarrito == null)
            {
                //Creamos un carrito nuevo
                elementoCarrito = new Carrito
                {
                    ProductoId = producto.ID,
                    CartId = IdCarritoDeCompras,
                    Count = Cantidad,
                    DateCreated = DateTime.Now
                };

                db.Carritos.Add(elementoCarrito);
            }
            else
            {
                //Si el elemento está en el carrito, le agregamos uno a la cantidad
                elementoCarrito.Count += Cantidad;
            }

            //Guardamos los cambios 
            db.SaveChanges();
        }

        public void AgregarAlCarrito(Producto producto)
        {
            // Averiguamos si tenemos abierto un carrito para agregarle los productos
            // Si no hay carrito abirto creamos uno

            var elementoCarrito = db.Carritos.SingleOrDefault(
                                    c => c.CartId == IdCarritoDeCompras
                                        && c.ProductoId == producto.ID);
            if (elementoCarrito == null)
            {
                //Creamos un carrito nuevo
                elementoCarrito = new Carrito
                {
                    ProductoId = producto.ID,
                    CartId = IdCarritoDeCompras,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                db.Carritos.Add(elementoCarrito);
            }
            else
            {
                //Si el elemento está en el carrito, le agregamos uno a la cantidad
                elementoCarrito.Count++;
            }

            //Guardamos los cambios 
            db.SaveChanges();
        }

        public decimal QuitarDelCarrito(int id)
        {
            //Recuperamos el carrito
            var elementoCarrito = db.Carritos.Single(
                                    carrito => carrito.CartId == IdCarritoDeCompras
                                        && carrito.RecordId == id);
            decimal itemCount = 0;

            if (elementoCarrito != null)
            {
                db.Carritos.Remove(elementoCarrito);
                
                db.SaveChanges();
                
            }

            return itemCount;
        }

        public void VaciarCarrito()
        {
            var elementosCarrito = db.Carritos.Where(carrito => carrito.CartId == IdCarritoDeCompras);

            foreach (var cartItem in elementosCarrito)
            {
                db.Carritos.Remove(cartItem);
            }

            db.SaveChanges();
        }

        public List<Carrito> GetElementos()
        {
            return db.Carritos.Where(cart => cart.CartId == IdCarritoDeCompras).ToList();
        }

        public int GetCantidad()
        {
            //Sacamos la cuenta de cada elemento y despues la sumamos 
            int? count = (from cartItems in db.Carritos
                          where cartItems.CartId == IdCarritoDeCompras
                          select (int?)cartItems.Count).Sum();

            //Regresamos 0 si es Null
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            //Multiplicamos la cantidad de cada producto por su precio
            decimal? total = (from cartItems in db.Carritos
                              where cartItems.CartId == IdCarritoDeCompras
                              select cartItems.Count * cartItems.PrecioVenta).Sum();

            return total ?? decimal.Zero;
        }

        public int CrearVenta(HttpContextBase context, Venta venta, int idCliente, int credito, int IdSucursal)
        {
            /*
             * Proceso de la venta:
             * 1.- Setear los precios y cantidades finales de cada producto - Se hace en el controlador
             * 2.- Calcular el total de la venta, así como los impuestos y el Subtotal
             * 3.- Crear las relaciones de la venta con los productos
             * 4.- Mandar a crédito si asi se requiere
             * 5.- Mandar Datos a la FEL para la factura electrónica
             * 6.- Recibir la factura electrónica y guardarla
             */
            decimal totalVenta = 0;
            
            var cartItems = GetElementos();

            foreach (var item in cartItems)
            {
                var productoVenta = new ProductoVenta
                {
                    ProductoId = item.ProductoId,
                    VentaId = venta.ID,                    
                    PrecioVenta = item.PrecioVenta,
                    Count = item.Count,
                    Producto = db.Productos.Find(item.ProductoId),
                    Venta = db.Ventas.Find(venta.ID)
                    
                };

                

                db.ProductosVenta.Add(productoVenta);
                db.SaveChanges();

                

                /*Aqui quitaremos los productos del inventario y generaremos un moviemiento en el*/

                //Buscamos el elemento que representa en ProductoSucursal
              
                
                var PS = db.ProductosSucursal.Single(p => p.IdProducto == item.ProductoId && p.IdSucursal == IdSucursal);

                //Quitamos del inventario la cantidad
                PS.cantidad -= item.Count;
                

                //Establecer el Subtotal de la venta - Modificar para el cambio de precios
                totalVenta += (item.Count * productoVenta.PrecioVenta);
            }

            venta.Subtotal = totalVenta;

            venta.TotalImpuestosRetenidos = 0;

            venta.TotalImpuestosTrasladados = totalVenta * IVA;

            venta.IdCliente = idCliente;

            venta.IdSucursal = IdSucursal;

            venta.Total = venta.Subtotal + venta.TotalImpuestosTrasladados;

            venta.ImporteLetra = toText(venta.Total);

            venta.Liquidado = true;


           

            venta.FormaDePago = "Pago en una sola exhibición";

            venta.IdSucursal = IdSucursal;

            decimal total = (totalVenta) + (totalVenta*IVA);

            venta.ImporteLetra = toText(total);

            venta.Total = total; 
             /*
             * Averiguamos la cuenta de crédito a la que se va a cargar esta venta
             */
            if(credito == 1)
            {
                var cCredito = db.CuentasCredito.Single(c => c.IdCliente == idCliente);

                venta.Liquidado = false;

                if (cCredito.Ventas.Count == 0)
                {
                    cCredito.Ventas = new List<Venta>();
                    cCredito.Ventas.Add(venta);
                }
                else
                cCredito.Ventas.Add(venta);

                db.SaveChanges();

            }
       
            //Aqui va la peticion con FEL

            db.SaveChanges();

            VaciarCarrito();

            return venta.ID;

        }

        public string toText(decimal value)
        {

            string Num2Text = "";

            value = Math.Truncate(value);

            if (value == 0) Num2Text = "CERO";

            else if (value == 1) Num2Text = "UNO";

            else if (value == 2) Num2Text = "DOS";

            else if (value == 3) Num2Text = "TRES";

            else if (value == 4) Num2Text = "CUATRO";

            else if (value == 5) Num2Text = "CINCO";

            else if (value == 6) Num2Text = "SEIS";

            else if (value == 7) Num2Text = "SIETE";

            else if (value == 8) Num2Text = "OCHO";

            else if (value == 9) Num2Text = "NUEVE";

            else if (value == 10) Num2Text = "DIEZ";

            else if (value == 11) Num2Text = "ONCE";

            else if (value == 12) Num2Text = "DOCE";

            else if (value == 13) Num2Text = "TRECE";

            else if (value == 14) Num2Text = "CATORCE";

            else if (value == 15) Num2Text = "QUINCE";

            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);

            else if (value == 20) Num2Text = "VEINTE";

            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);

            else if (value == 30) Num2Text = "TREINTA";

            else if (value == 40) Num2Text = "CUARENTA";

            else if (value == 50) Num2Text = "CINCUENTA";

            else if (value == 60) Num2Text = "SESENTA";

            else if (value == 70) Num2Text = "SETENTA";

            else if (value == 80) Num2Text = "OCHENTA";

            else if (value == 90) Num2Text = "NOVENTA";

            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);

            else if (value == 100) Num2Text = "CIEN";

            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);

            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";

            else if (value == 500) Num2Text = "QUINIENTOS";

            else if (value == 700) Num2Text = "SETECIENTOS";

            else if (value == 900) Num2Text = "NOVECIENTOS";

            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);

            else if (value == 1000) Num2Text = "MIL";

            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);

            else if (value < 1000000)
            {

                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";

                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);

            }

            else if (value == 1000000) Num2Text = "UN MILLON";

            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);

            else if (value < 1000000000000)
            {

                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";

                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);

            }

            else if (value == 1000000000000) Num2Text = "UN BILLON";

            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {

                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";

                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            }

            return Num2Text;

        }

    }
}