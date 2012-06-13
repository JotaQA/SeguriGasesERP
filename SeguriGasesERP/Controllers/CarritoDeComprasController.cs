using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;
using SeguriGasesERP.ViewModels;
using System.Web.Security;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using SeguriGasesERP.WSRemota32;

namespace SeguriGasesERP.Controllers
{
    [Authorize(Roles = "Facturista")]
    public class CarritoDeComprasController : Controller
    {

        /******************************************************************************************************************************************
         * Nombre: EnviarWebRequest
         * Propósito: Crear una conexión remota y enviar un http request con todas las variables POST necesarias. Se genera una factura.
         * Output: Sin necesidad de especificación (bien puede manejarse como un sub).
         ******************************************************************************************************************************************/
        
        public string EnviarWebRequest(Cliente Cliente, Venta Venta, List<ProductoVenta> Productos, Sucursal Sucursal)
        {
            if (Cliente == null || Venta == null || Productos.Count < 1 || Sucursal == null)
                return "* Error en los argumentos de la funcion";

            //Instancia del Web Service para la conexion remota
            ConexionRemota32SoapClient conexionRemota = new ConexionRemota32SoapClient();

            /* Crear la información que llevará incluida la factura
             * Establecer TODOS los datos requeridos por Facturar en Línea, que deberán ser incluidos en el POST.
             * Nota: Checar archivo API de Excel que contiene la descripción y requerimiento de cada campo.
             * Crear la información que llevará el WS
             * Sección de Variables para la autenticación del usuario remoto.
             */

            #region Datos del Emisor

            //Creamos el arreglo del usuario remoto
            ArrayOfString datosUsuario = new ArrayOfString();
            //Establecer el nombre de usuario (RFC) (REQUERIDO). Posicion 0
            datosUsuario.Add(Sucursal.RFC);
            //Nombre de la cuenta (SUCURSAL). Posicion 1
            datosUsuario.Add(Sucursal.Cuenta);
            //Password. Posicion 2
            datosUsuario.Add(Sucursal.Password);

            #endregion

            #region Datos del cliente
            /*****************************************************************************************
            * Sección de Arreglo para identificar y actualizar los datos del Cliente o Receptor.
            *****************************************************************************************/

            //Creamos el arreglo que contendra los datos del cliente o receptor
            ArrayOfString datosReceptor = new ArrayOfString();

            //Nombre del cliente REQUERIDO. Posicion 0
            datosReceptor.Add(Cliente.NombreCliente);
            //Nombre del contacto principal OPCIONAL. Posicion 1
            datosReceptor.Add(Cliente.Contacto);
            //Telefono del contacto principal OPCIONAL. Posicion 2
            datosReceptor.Add(Cliente.Telefono);
            //Email del contacto OPCIONAL. Posicion 3
            datosReceptor.Add(Cliente.Email);
            //RFC del cliente REQUERIDO. Posicion 4
            datosReceptor.Add(Cliente.RfcReceptor);
            //Razon Social REQUERIDO. Posicion 5
            datosReceptor.Add(Cliente.NombreReceptor);
            // Calle de dirección Requerido. Posicion 6
            datosReceptor.Add(Cliente.CalleReceptor);
            // Numero Exterior REQUERIDO. Posicion 7
            datosReceptor.Add(Cliente.NoExteriorReceptor);
            //Numero Interior OPCIONAL. Posicion 8
            datosReceptor.Add(Cliente.NoInteriorReceptor);
            //Colonia REQUERIDO. Posicion 9.
            datosReceptor.Add(Cliente.ColoniaReceptor);
            //Localidad OPCIONAL. Posicion 10.
            datosReceptor.Add(Cliente.LocalidadReceptor);
            //Referencia OPCIONAL. Posicion 11.
            datosReceptor.Add(Cliente.ReferenciaReceptor);
            //Municipio REQUERIDO. Posicion 12
            datosReceptor.Add(Cliente.MunicipioReceptor);
            //Estado REQUERIDO. Posicion 13
            datosReceptor.Add(Cliente.EstadoReceptor);
            //Pais Requerido. Posicion 14
            datosReceptor.Add(Cliente.PaisReceptor);
            //Codigo Postal REQUERIDO. Posicion 15
            datosReceptor.Add(Cliente.CodigoPostalReceptor);

            #endregion

            #region Informacion del CFDI
            /*********************************************************
            ' Sección de Variables de información general del CFDI.
            ' *********************************************************/

            //Arreglo del comprobante general
            ArrayOfString datosCFDI = new ArrayOfString(); 

            //Establecer la clave de CFD a emitir, "FAC" para factura (REQUERIDO). Posicion 0
            datosCFDI.Add("FAC");
            //Establecer la forma de pago (REQUERIDO). Posicion 1
            datosCFDI.Add("Pago en una sola exhibición");
            //Pago en parcialidades. (OPCIONAL). Posicion 2
            datosCFDI.Add("");
            //Condiciones de pago. (OPCIONAL). Posicion 3
            datosCFDI.Add("");
            //Metodo de pago. (Opcional). Posicion 4
            datosCFDI.Add("");
            //Descuento Usado (Opcional). Posicion 5
            datosCFDI.Add("0.00");
            //Porcentaje de descuento (Opcional). Posicion 6
            datosCFDI.Add("0.00");
            //Motivo del descuento (Opcional). Posicion 7
            datosCFDI.Add("");
            //Establecer el tipo de moneda (Requerido). Posicion 8
            datosCFDI.Add("MXN");
            //Tipo de cambio(Opcional). Posicion 9
            datosCFDI.Add("");
            //Fecha de tipo de cambio (Opcional). Posicion 10
            datosCFDI.Add("");
            //Impuestos Retenidos (Requerido). Posicion 11
            datosCFDI.Add("0.000000");
            //Impuestos Trasladados (Requerido). Posicion 12
            datosCFDI.Add(Venta.TotalImpuestosTrasladados.ToString());
            //Establecer el subtotal (REQUERIDO). Posicion 13
            datosCFDI.Add(Venta.Subtotal.ToString());
            //Establecer el total (REQUERIDO). Posicion 14
            datosCFDI.Add(Venta.Total.ToString());
            //Establecer el importe con letra del total (REQUERIDO). Posicion 15
            datosCFDI.Add(Venta.ImporteLetra);

            //Nuevos Campos SAT 3.2
            // Lugar de expedicion (Requerido)
            datosCFDI.Add("Puebla");
            //(17) NumCuentaPago (OPCIONAL)
            datosCFDI.Add("");
            //(18) FolioFiscalOrig (OPCIONAL)
            datosCFDI.Add("");
            //(19) SerieFolioFiscalOrig (OPCIONAL)
            datosCFDI.Add("");
            //(20) FechaFolioFiscalOrig (OPCIONAL)
            datosCFDI.Add("");
            //(21) MontoFolioFiscalOrig (OPCIONAL)
            datosCFDI.Add("");

            #endregion

            #region Etiquetas
            /*************************************************************************************
            / Sección de Variables para el uso de información comercial de la empresa emisora.
            /*************************************************************************************/
            //Arreglo para enviar los datos de la empresa emisora (FEL)
            ArrayOfString datosEtiquetas = new ArrayOfString();
            //Establecer el nombre de la etiqueta personalizada (Opcional).
            //secuencia |nombre|valor|

            //Etiqueta para el portal web
            datosEtiquetas.Add("|Portal Web|www.segurigases.com|");
            //Etiqueta Correo electronico
            datosEtiquetas.Add("|Email|info@segurigases.com|");

            #endregion

            #region Conceptos
            /*****************************************************************************
            ' Sección de Variables para la información y descripción de los conceptos.
            ' *****************************************************************************/
            //arreglo para la referencia de los conceptos
            ArrayOfString datosConceptos = new ArrayOfString();          
                        
            int noProd = Productos.Count();
            //Por cada producto dentro de la venta creamos un concepto nuevo
            foreach(ProductoVenta item in Productos)
            {
                string cantidad = item.Count.ToString();
                string descripcion = item.Producto.Nombre;
                string valor = item.PrecioVenta.ToString();
                string importe = (item.PrecioVenta * item.Count).ToString();
                string unidad = item.Producto.Unidad.Nombre;
                string clave = item.Producto.Clave;

                //Secuencia: |cantidad|unidad|noIdentificacion|descripcion|valorUnitario|importe|
                datosConceptos.Add("|" + cantidad + "|" + unidad + "|" + clave + "|" + descripcion + "|" + valor + "|" + importe + "|");
            }
            #endregion

            #region Informacion Aduanera

            /*
             * Sección de variables para la información aduanera correspondiente a cada concepto usado en el CFDI.
             * IMPORTANTE: El tamaño del vector de aduanera debe coincidir respectivamente con el de conceptos, ya que es 1 a 1.
             * Secuencia: |numero|fecha|aduana|
             * Por el momento Segurigases no cuenta con informacion aduanera por lo tanto no se enviara un arreglo con las posiciones vacias
             */

            //Creacion del arreglo para la informacion aduanera
            ArrayOfString datosInfoAduanera = new ArrayOfString();

            foreach (var item in Productos)
                datosInfoAduanera.Add(null);

            #endregion

            #region Impuestos Retenidos

            /*
             * Sección de variables para la información de todos los impuestos retenidos utilizados en el CFDI. 
             * Secuencia: |NombreImpuesto|impuesto|importe|
             * SeguriGases no retiene impuestos por lo tanto no haremos nada aqui
             */

            //Arreglo para referenciar los impuestos retenidos
            ArrayOfString datosRetenidos = new ArrayOfString();

            #endregion

            #region Impuestos Retenidos Locales

            /*
             * Sección de variables para la información de todos los impuestos retenidos locales utilizados en el CFDI.
             * Secuencia: |NombreImpuesto|impuesto|tasa|importe|
             * SeguriGases no retiene impuestos, por lo tanto nos se hara nada
             */

            //Arreglo de Retenidos Locales
            ArrayOfString datosRetenidosLocales = new ArrayOfString();

            #endregion

            #region Impuestos trasladados

            /*
             * Sección de variables para la información de todos los impuestos trasladados utilizados en el CFDI.
             * Secuencia: |NombreImpuesto|impuesto|tasa|importe|
             */

            //Arreglo para referenciar los impuestos trasladados
            ArrayOfString datosTrasladados = new ArrayOfString();

            //Establecer el nombre arbitrario del impuesto trasladado (Opcional).
            string nombreTraslado1 = "IVA (IVA 16%)";
            //Establecer el tipo de impuesto trasladado (Opcional).
            string impuestoTraslado1 = "IVA";
            //Establecer la tasa de impuesto trasladado (Opcional).
            string tasa1 = "16.00";
            //Establecer el importe del impuesto trasladado (Opcional).
            string importeTraslado1 = Venta.TotalImpuestosTrasladados.ToString();

            string cadenaTraslados = "|" + nombreTraslado1 + "|" + impuestoTraslado1 + "|" + tasa1 + "|" + importeTraslado1 + "|";

            datosTrasladados.Add(cadenaTraslados);

            #endregion

            #region Impuestos Trasladados Locales

            /*
             * Sección de variables para la información de todos los impuestos trasladados locales utilizados en el CFDI.  
             * Secuencia:|NombreImpuesto|impuesto|tasa|importe|
             */

            //Arreglo de traslados locales
            ArrayOfString datosTrasladosLocales = new ArrayOfString();
            datosTrasladosLocales.Add(cadenaTraslados);
            
            #endregion

            #region Consumision del servicio

            //Respuesta del servicio
            ArrayOfString respuestaWS = new ArrayOfString();

            //Consumimos el servicio
            respuestaWS = conexionRemota.GenerarCFDIv32(datosUsuario,
                                                        datosReceptor,
                                                        datosCFDI,
                                                        datosEtiquetas,
                                                        datosConceptos,
                                                        datosInfoAduanera,
                                                        datosRetenidos,
                                                        datosTrasladados,
                                                        datosRetenidosLocales,
                                                        datosTrasladosLocales);

            string folio = "";
            //Verificar si la respuesta fue o no exitosa
            if (!respuestaWS[0].Equals("True"))
            {
                //No Fue exitosa, debemos cancelar la venta aqui, pero por el momento solo reportaremos el error en el log
                string ruta = @"C:\Facturas\respuesta" + DateTime.Now + ".txt";
                // Se usa una clase externa que simplemente guarda un contenido en un archivo de texto.

                //System.IO.File.WriteAllText(ruta, respuestaWS);
            }
            else
            {
                //Si fe exitosa, entonces guardaremos el Xml del CDF en la computadora
                XmlDocument cfdXML = new XmlDocument();                
                //El contenido XML se encuentra en la posicion 3 del arreglo
                cfdXML.LoadXml(respuestaWS[3]);
                folio = cfdXML.GetElementById("folio").ToString();
                //Leemos el folio
                //TODO: Parsear el XML para buscar el folio
                //Guardamos el XML
                //TODO: cfdXML.Save(@"C:\CFDXML\
            }
            #endregion             

            return folio;
        }


        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /CarritoDeCompras/

        public ActionResult Index()
        {
                      
            var carrito = CarritoDeCompras.GetCarrito(this.HttpContext);
            ViewBag.Clientes = db.Clientes.OrderBy(c => c.NombreCliente).ToList();
            var Sucursales = db.Sucursales.Where(s => s.Usuarios.Any(u => u.Username == HttpContext.User.Identity.Name));
            ViewBag.Sucursales = Sucursales;

                var viewModel = new CarritoDeComprasViewModel
                {
                    ElementosCarrito = carrito.GetElementos(),
                    TotalCarrito = 0
                };

                ViewBag.Elementos = viewModel.ElementosCarrito.Count();
          
            return View(viewModel);
        }

        //
        // POST: /CarritoDeCompras/
        // Se encarga de setear el cliente como variable de sesion para proseguir con la venta

              

        // GET: /CarritoDeCompras/Confirmar/
        //        
        public ActionResult Confirmar(int IdCliente, int IdSucursal)
        {           
            var carrito = CarritoDeCompras.GetCarrito(this.HttpContext);
            Cliente Cliente = db.Clientes.Find(IdCliente);

            ViewBag.Cliente = Cliente;
            ViewBag.IdSucursal = IdSucursal;

            var viewModel = new CarritoDeComprasViewModel
            {
                ElementosCarrito = carrito.GetElementos(),
                TotalCarrito = carrito.GetTotal(),
                cliente = db.Clientes.Find(IdCliente)
            };

            if (viewModel.ElementosCarrito.Count == 0)
                return RedirectToAction("Error", new { Error = "No hay elementos en el carrito" });

            /*
             * Aqui Verificamos el precio de venta que se maneja para cada producto 
             * al cliente al que se le realizará la venta, en caso de no haberle vendido
             * antes esta producto se pondrá por default el precio de lista, en caso contrario
             * se pondrá por default el último precio de venta
             */
            int i;
            for( i = 0; i < viewModel.ElementosCarrito.Count(); i++)
            {

                /*
                 * Lógica del precio:
                 *  Recuperar las ventas que se han hecho al cliente del mismo producto y sacar la más reciente
                 *  Setear el Precio de venta con el ultimo precio dado!
                 */
                
                //Sacamos todas las ventas que se han hecho
                var items = from prodVen in db.ProductosVenta.Include("Venta")
                            where prodVen.Venta.IdCliente == IdCliente
                            orderby prodVen.Venta.FechaVenta descending
                            select prodVen;

                decimal precioVenta = viewModel.ElementosCarrito[i].Producto.PrecioLista;               
                foreach (var item in items)
                {
                    if (item.Venta.IdCliente == IdCliente && item.ProductoId == viewModel.ElementosCarrito[i].ProductoId)
                    {
                        precioVenta = item.PrecioVenta;
                        break;
                    }

                }




                viewModel.ElementosCarrito[i].PrecioVenta = precioVenta;
                
                viewModel.ElementosCarrito[i].Existencia = viewModel.ElementosCarrito[i].GetCantidad(IdSucursal);
            }


            ViewBag.IdCliente = IdCliente;


            //return RedirectToAction("Resumen", new { IdClient = IdCliente });
            return View(viewModel);

        }

        //
        //POST: /CarritoDeCompras/Confirmar/
        /*
         * Aquí se procesa ya la información de la venta con los precios y cantidades finales
         * seteados por el usuario y se envía al usuario al resumen.
         */
        [HttpPost]
        public ActionResult Confirmar(int IdClient)
        {                
            int IdCliente = IdClient;

            int IdSucursal = int.Parse(Request.Form["IdSucursal"]);

            //int IdCliente = (int) ViewData["IdCliente"];
            var carrito = CarritoDeCompras.GetCarrito(this.HttpContext);
            ViewBag.Cliente = db.Clientes.Find(IdCliente);
            /*Recuperamos el Carrito de compras*/
            var viewModel = new CarritoDeComprasViewModel
            {
                ElementosCarrito = carrito.GetElementos(),
                TotalCarrito = carrito.GetTotal(),
                cliente = db.Clientes.Find(IdCliente)
            };

            if (viewModel.ElementosCarrito.Count == 0)
                return RedirectToAction("Error", new { Error = "No hay elementos en el carrito" });

            /*
             * Actualizamos cantidades y precios y Total del carrito
             */
            
            int i;
            string campoCantidad;
            string campoPrecio;
            viewModel.TotalCarrito = 0;
            for (i = 0; i < viewModel.ElementosCarrito.Count(); i++)
            {
                int IdP = viewModel.ElementosCarrito[i].ProductoId;
                var ElementoActual = db.Carritos.SingleOrDefault(
                                     c => c.CartId == carrito.IdCarritoDeCompras
                                         && c.ProductoId == IdP);
               campoCantidad = "Cantidad-" + viewModel.ElementosCarrito[i].ProductoId;
               campoPrecio = "Precio-" + viewModel.ElementosCarrito[i].ProductoId;
               if (Request.Form[campoCantidad].Equals(""))
                   return RedirectToAction("Error", new { Error = "No se ingreso información util para la cantidad, deben ser numeros decimales o enteros" });

               if (Request.Form[campoPrecio].Equals(""))
                   return RedirectToAction("Error", new { Error = "No se ingreso información util para los Precios, deben ser numeros decimales o enteros" });

               decimal Prec = decimal.Parse(Request.Form[campoPrecio]);
               viewModel.ElementosCarrito[i].PrecioVenta = (decimal)Prec;
               
               ElementoActual.PrecioVenta = (decimal)Prec;

               
                //No podemos vender más de lo que existe en el inventario, entonces regresamos con error
               if (decimal.Parse(Request.Form[campoCantidad]) > viewModel.ElementosCarrito[i].GetCantidad(IdSucursal))
               {
                   string Err = "No se puede vender: " + decimal.Parse(Request.Form[campoCantidad]) + "de " + viewModel.ElementosCarrito[i].Producto.Nombre + ", no hay suficientes en el inventario. Solo tenemos: " + viewModel.ElementosCarrito[i].Existencia;
                   return RedirectToAction("Error", new { Error = Err });
               }
               viewModel.ElementosCarrito[i].Count = decimal.Parse(Request.Form[campoCantidad]);
               ElementoActual.Count = decimal.Parse(Request.Form[campoCantidad]);
               viewModel.TotalCarrito += (viewModel.ElementosCarrito[i].PrecioVenta * viewModel.ElementosCarrito[i].Count);    
               
            }

            try
            {
                db.SaveChanges();
            }
            catch
            {            
                    return RedirectToAction("Error", new { Error = "No se pudo actualizar la información de los elementos" });
            }


            return RedirectToAction("Resumen", new { IdClient = IdCliente, IdSucursal = IdSucursal });
            
        }
        /*
         * Aqui se muestra el total de la venta al usuario y se dala opción para mandar a crtedito si es el cliente sugeto a credito
         * Al aceptar se procesa la venta y se guarda en la base de datos
         */
        public ActionResult Resumen(int IdClient, int IdSucursal)
        {            

            var carrito = CarritoDeCompras.GetCarrito(this.HttpContext);
            var viewModel = new CarritoDeComprasViewModel
            {
                ElementosCarrito = carrito.GetElementos(),
                TotalCarrito = carrito.GetTotal()
            };
                   
                        
            Cliente ClienteVenta = db.Clientes.Find(IdClient);
          
            if (ClienteVenta == null)
                return RedirectToAction("Error", new { Error = "No se Puede Identificar al Cliente" });


            ViewBag.Cliente = ClienteVenta;
            ViewBag.IdSucursal = IdSucursal;
            //Aqui checamos si el cliente es sujeto a credito


            //1.- Ver si tiene abierta una cuenta de credito
            if (ClienteVenta.SugetoCredito(viewModel.TotalCarrito*1.16M))
                ViewBag.SugetoCredito = 1;
            else
                ViewBag.SugetoCredito = 0;
           
                    
            return View(viewModel);


        }

        [HttpPost]
        public ActionResult Resumen(string credito)
        {
            int credit = 0;
            if (credito.Equals("true"))
                credit = 1;
            int IdCliente = int.Parse(Request.Form["IdCliente"]);



            int IdSucursal = int.Parse(Request.Form["IdSucursal"]);


            var carrito = CarritoDeCompras.GetCarrito(this.HttpContext);
           
            var viewModel = new CarritoDeComprasViewModel
            {
                ElementosCarrito = carrito.GetElementos(),
                TotalCarrito = carrito.GetTotal()
            };

            if (viewModel.ElementosCarrito.Count == 0)
                return RedirectToAction("Error", new { Error = "No hay elementos en el carrito" });

            Cliente ClienteVenta = db.Clientes.Find(IdCliente);
            if (ClienteVenta == null)
                return RedirectToAction("Error", new { Error = "No se Puede Identificar al Cliente" });

            ViewBag.Cliente = ClienteVenta;

            //Creamos La venta
            var venta = new Venta();

            TryUpdateModel(venta);

            
            
            try
            {
                venta.IdSucursal = IdSucursal;
                venta.IdCliente = IdCliente;
                venta.FechaVenta = DateTime.Now;
                venta.FechaTipoCambio = DateTime.Now;

                //Guardamos la venta
                db.Ventas.Add(venta);
                db.SaveChanges();

                //Procesamos la venta               
                CrearVenta(carrito, venta, IdCliente, credit, IdSucursal);
                //Generamos la factura Electrónica
                List<ProductoVenta> Productos = db.ProductosVenta.Where(pv => pv.VentaId == venta.ID).ToList();

                Sucursal Suc = db.Sucursales.Find(IdSucursal);
                
                
                string XML = EnviarWebRequest(ClienteVenta, venta, Productos, Suc);

                
           }
            catch
            {
                //Invalido   
                /*
                 * Vamos a proceder a deshacer la venta, regresando los productos al inventario.
                 * */
                 

                string Err;
                if (venta.CancelarVenta(IdSucursal) == 1)
                {
                     Err = "No se Ha realizado la venta, ocurrio un error. Intentar de nuevo.";
                }
                else
                     Err = "No se Ha realizado la venta, ocurrio un error, pero es posible que parte de la venta este registrada. Intentar de nuevo.";
                
              
                
                return RedirectToAction("Error", new { Error = Err });
            }


            return RedirectToAction("Ok", new { folio = venta.ClaveCFD, idCliente = IdCliente });
                
        }

        public ActionResult Ok(string folio, int idCliente)
        {
            /*
             * Sacaremos el balance de la cuenta de crédito del cliente:
             *  Necesitamos sacar la factura no liquidada mas antigua
             *  Necesitamos tambien la deuda que tiene vs su límite
             */
            int IdCliente = idCliente;
            
            var cliente = db.Clientes.Find(IdCliente);
            ViewBag.Cliente = cliente;
            var cuenta = db.CuentasCredito.Single( cc => cc.IdCliente == IdCliente);
            ViewBag.LimiteDinero = cuenta.MontoLimite;
            ViewBag.Deuda = cuenta.getDeuda();
            ViewBag.LimiteDias = cuenta.DiasLimite;
            ViewBag.Dias = cuenta.VentasMasVieja();
            ViewBag.Vencido = cliente.SugetoCredito();            
            ViewBag.Folio = folio;
            return View();

        }

        public ActionResult Error(string Error)
        {
            ViewBag.Error = Error;
            return View();
        }

        public ActionResult Confirmacion(int id)
        {
            return View(id);
        }


        // GET: /AgregarAlCarrito/

        public ActionResult AgregarAlCarrito(int id)
        {
            //Retrive producto de la BD

            var agregado = db.Productos.Single(producto => producto.ID == id);

            var cart = CarritoDeCompras.GetCarrito(this.HttpContext);

            cart.AgregarAlCarrito(agregado);

            return RedirectToAction("Index");
        }

        //AJAX
        //POST: /CarritoDeCompras/QuitarDelCarrito/

        
        public ActionResult QuitarDelCarrito(int id)
        {
            //Quitar elemento del carrito

            var cart = CarritoDeCompras.GetCarrito(this.HttpContext);

            //Quitarlo del carrito
            decimal itemcount = cart.QuitarDelCarrito(id);

            

            return RedirectToAction("Index");
        }

        // GET: /CarritoDeCompras/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = CarritoDeCompras.GetCarrito(this.HttpContext);

            ViewData["CartCount"] = cart.GetCantidad();

            return PartialView("CartSummary");
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

        public int CrearVenta(CarritoDeCompras carrito, Venta venta, int idCliente, int credito, int IdSucursal)
        {
            decimal IVA = 0.16M;
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

            var cartItems = carrito.GetElementos();

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

                //Aqui generamos un movimiento de Almacen
                 MovimientoAlmacen movimiento = new MovimientoAlmacen
                {
                    DescripcionMovimiento = "Venta de " + item.Count + " del producto " + item.Producto.Nombre,
                    Count = item.Count,
                    FechaMovimiento = System.DateTime.Now,
                    OrdenDeCompra = null,
                    IdOrednCompra = 0,
                    IdProducto = item.ProductoId,
                    Producto = db.Productos.Find(item.ProductoId),
                    IdSucursal = IdSucursal,
                    Sucursal = db.Sucursales.Find(IdSucursal),
                    TipoMovimiento = "Venta",
                    IdVenta = venta.ID,
                    username = HttpContext.User.Identity.Name
                };

                 db.MovimientosAlmacen.Add(movimiento);

                //Establecer el Subtotal de la venta - Modificar para el cambio de precios
                totalVenta += (item.Count * productoVenta.PrecioVenta);
            }

            venta.Moneda = "MXN Moneda Nacional (Peso Mexicano)";

            venta.usuario = HttpContext.User.Identity.Name;

            venta.Subtotal = totalVenta;

            venta.TotalImpuestosRetenidos = 0;

            venta.TotalImpuestosTrasladados = totalVenta * IVA;

            venta.IdCliente = idCliente;

            venta.IdSucursal = IdSucursal;

            decimal total =  venta.Subtotal + venta.TotalImpuestosTrasladados;

            venta.Total = total;

            decimal centavos = (total - (int)total) * 100;

            venta.ImporteLetra = toText(venta.Total) + ", " + (int)centavos + "/100 MXN";

            venta.Liquidado = true;

            

           

            

            venta.FormaDePago = "Pago en una sola exhibición";

            venta.IdSucursal = IdSucursal;

            decimal totalN = (totalVenta) + (totalVenta * IVA);

            decimal centavosN = 100*(totalN - (int)total);

            int cn = (int)centavosN;
            venta.ImporteLetra = toText(totalN) + " " + cn + "/100 M.N.";

            venta.Total = total;
            
            //Generamos un movimiento de Almacen
            /*
             * Averiguamos la cuenta de crédito a la que se va a cargar esta venta
             */
            if (credito == 1)
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

               

            }
           
            db.SaveChanges();

            carrito.VaciarCarrito();

            return venta.ID;

        }
    }
}
