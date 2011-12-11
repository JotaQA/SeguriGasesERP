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


namespace SeguriGasesERP.Controllers
{
    [Authorize(Roles = "Facturista")]
    public class CarritoDeComprasController : Controller
    {

        /******************************************************************************************************************************************
        ' Nombre: EnviarWebRequest
        ' Propósito: Crear una conexión remota y enviar un http request con todas las variables POST necesarias. Se genera una factura.
        ' Output: Sin necesidad de especificación (bien puede manejarse como un sub).

        '******************************************************************************************************************************************/
        
        public string EnviarWebRequest(Cliente Cliente, Venta Venta, List<ProductoVenta> Productos, Sucursal Sucursal)
        {
            if (Cliente == null || Venta == null || Productos.Count < 1 || Sucursal == null)
                return "* Error en los argumentos de la funcion";
            //URL del servidor web al cual se enviará la solicitud.
            string url = "https://www.facturarenlinea.com/CFD/ConexionRemota/ConexionRemota.aspx";
            //Crear un objeto de conexión remota al servidor.
            WebRequest request = WebRequest.Create(url);
            //Establecer el método de conexión vía: POST.
            request.Method = "POST";

            /* Crear la información que llevará incluida el POST del request.
             * Establecer TODOS los datos requeridos por Facturar en Línea, que deberán ser incluidos en el POST.
             * Nota: Checar archivo API de Excel que contiene la descripción y requerimiento de cada campo.
             */
            /* Crear la información que llevará incluida el POST del request.
            ' Sección de Variables POST para la autenticación del usuario remoto.
             */
            
            //Establecer el nombre de usuario (RFC) (REQUERIDO).
            string RFC = Sucursal.RFC;
            //Nombre de la cuenta (SUCURSAL)
            string Cuenta = Sucursal.Cuenta;
            //Password
            string Password = Sucursal.Password ;//+ "FASLOE";


            
      
            /*****************************************************************************************
        ' Sección de Variables POST para identificar y actualizar los datos del Cliente o Receptor.
        ' *****************************************************************************************/

            //Nombre del cliente REQUERIDO
            string NombreCliente = Cliente.NombreCliente;
            //Nombre del contacto principal OPCIONAL
            string Contacto = Cliente.Contacto;
            //Telefono del contacto principal OPCIONAL
            string Telefono = Cliente.Telefono;
            //Email del contacto OPCIONAL
            string Email = Cliente.Email;
            //RFC del cliente REQUERIDO
            string rfcReceptor = Cliente.RfcReceptor;
            //Razon Social REQUERIDO
            string nombreReceptor = Cliente.NombreReceptor;
            // Calle de dirección Requerido
            string calleReceptor = Cliente.CalleReceptor;
            // Numero Exterior REQUERIDO
            string noExteriorReceptor = Cliente.NoExteriorReceptor;
            //Numero Interior OPCIONAL
            string noInteriorReceptor = Cliente.NoInteriorReceptor;
            //Colonia REQUERIDO
            string coloniaReceptor = Cliente.ColoniaReceptor;
            //Localidad OPCIONAL
            string localidadReceptor = Cliente.LocalidadReceptor;
            //Referencia OPCIONAL
            string referenciaReceptor = Cliente.ReferenciaReceptor;
            //Municipio REQUERIDO
            string municipioReceptor = Cliente.MunicipioReceptor;
            //Estado REQUERIDO
            string estadoReceptor = Cliente.EstadoReceptor;
            //Pais Requerido
            string paisReceptor = Cliente.PaisReceptor;
            //Codigo Postal REQUERIDO
            string codigoPostalReceptor = Cliente.CodigoPostalReceptor;


            /*********************************************************
            ' Sección de Variables POST de información general del CFD.
            ' *********************************************************/

            //Establecer la clave de CFD a emitir, "FAC" para factura (REQUERIDO).
            string ClaveCFD  = "FAC";
            //Establecer la forma de pago (REQUERIDO).
            string formaDePago = "Pago en una sola exhibición";
            //Establecer el tipo de moneda (Requerido).
            string moneda = "MXN";
            //Establecer el subtotal (REQUERIDO).
            string subTotal= Venta.Subtotal.ToString();
            //Establecer el total (REQUERIDO).
            string total = Venta.Total.ToString();
            //Establecer el importe con letra del total (REQUERIDO).
            string importeConLetra = Venta.ImporteLetra;

            /*****************************************************************************
            ' Sección de Variables POST para la información y descripción de los conceptos.
            ' *****************************************************************************/
            //Creamos los arreglos para cada producto
            int noProd = Productos.Count();
            String[] Cantidades = new String[noProd];
            String[] Descripciones = new String[noProd];
            String[] Valores = new String[noProd];
            String[] Importes = new String[noProd];

            int i = 0;
            foreach(ProductoVenta item in Productos)
            {
                Cantidades[i] = item.Count.ToString();
                Descripciones[i] = item.Producto.Nombre;
                Valores[i] = item.PrecioVenta.ToString();
                Importes[i] = (item.PrecioVenta * item.Count).ToString();
                i++;
            }



            /*
             * Sección de Variables POST para indicar el total de impuestos utilizados en el CFD.
             */

            /*************************************************************************************
        ' Sección de Variables POST para el uso de información comercial de la empresa emisora.
        ' *************************************************************************************/
        //Establecer el nombre de la etiqueta personalizada (Opcional).
        string nombreEtiqueta1  = "Portal web";
        //Establecer el valor de la etiqueta personalizada (Opcional).
        string valorEtiqueta1 = "www.segurigases.com";
        //Establecer el nombre de la etiqueta personalizada (Opcional).
        string nombreEtiqueta2 = "Email";
        //Establecer el valor de la etiqueta personalizada (Opcional).
        string valorEtiqueta2 = "info@segurigases.com";


            //Impuestos Retenidos
            string totalImpuestosRetenidos = "0.000000";
            //Impuestos Trasladados
            string totalImpuestosTrasladados = Venta.TotalImpuestosTrasladados.ToString();

            /*
             * Sección de Variables POST para la información de todos los impuestos trasladados utilizados en el CFD.
             */

            //Establecer el nombre arbitrario del impuesto trasladado (Opcional).
            string nombreTraslado1  = "IVA (IVA 16%)";
            //Establecer el tipo de impuesto trasladado (Opcional).
            string impuestoTraslado1 = "IVA";
            //Establecer la tasa de impuesto trasladado (Opcional).
            string tasa1 = "16.000000";
            //Establecer el importe del impuesto trasladado (Opcional).
            string importeTraslado1 = Venta.TotalImpuestosTrasladados.ToString();

            /*
             * Unir las variables en un string tipo POST.
             * Nota: las variables se van uniendo de la siguiente manera: var1=valor1&var2=valor2&var3=valor4 ... etc
             */

            string postData = "RFC=" + RFC + "&Cuenta=" + Cuenta + "&Password=" + Password +
        "&NombreCliente=" + NombreCliente + "&Contacto=" + Contacto + "&Telefono=" + Telefono + "&Email=" + Email + "&rfcReceptor=" + rfcReceptor + "&nombreReceptor=" + nombreReceptor +
        "&calleReceptor=" + calleReceptor + "&noExteriorReceptor=" + noExteriorReceptor + "&noInteriorReceptor=" + noInteriorReceptor +
        "&coloniaReceptor=" + coloniaReceptor + "&localidadReceptor=" + localidadReceptor + "&referenciaReceptor=" + referenciaReceptor +
        "&municipioReceptor=" + municipioReceptor + "&estadoReceptor=" + estadoReceptor + "&paisReceptor=" + paisReceptor +
        "&codigoPostalReceptor=" + codigoPostalReceptor +
        "&ClaveCFD=" + ClaveCFD + "&formaDePago=" + formaDePago + "&moneda=" + moneda +
        "&subTotal=" + subTotal + "&total=" + total + "&importeConLetra=" + importeConLetra +
        "&nombreEtiqueta1=" + nombreEtiqueta1 + "&valorEtiqueta1=" + valorEtiqueta1 + "&nombreEtiqueta2=" + nombreEtiqueta2 + "&valorEtiqueta2=" + valorEtiqueta2;
            i = 0;
            foreach (ProductoVenta item in Productos)
            {
                postData += "&cantidad" + (i+1) + "="+ Cantidades[i] + "&descripcion" + (i+1) + "=" + Descripciones[i] + "&valorUnitario" + (i+1) + "=" + Valores[i] + "&importe" + (i+1) + "=" + Importes[i];
                i++;
            }
        
        postData += "&totalImpuestosRetenidos=" + totalImpuestosRetenidos + "&totalImpuestosTrasladados=" + totalImpuestosTrasladados + 
        "&nombreTraslado1=" + nombreTraslado1 + "&impuestoTraslado1=" + impuestoTraslado1 + "&tasa1=" + tasa1 + "&importeTraslado1=" + importeTraslado1;



        //' Establecer el string del POST en un arreglo de bytes para su envío.
        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            //Establecer la propiedad ContentType del WebRequest.
        request.ContentType = "application/x-www-form-urlencoded";
        //Establecer la propiedad ContentLength del WebRequest.
        request.ContentLength = byteArray.Length;
        //Obtener el stream del request.
        Stream dataStream  = request.GetRequestStream();
        //Escribir la información al stream del request.
        dataStream.Write(byteArray, 0, byteArray.Length);
        //Cerrar el objeto Stream.
        dataStream.Close();

        // Obtener la respuesta.
        WebResponse respuesta = request.GetResponse();
        //Obtener el stream que contiene la respuesta del servidor.
        dataStream = respuesta.GetResponseStream();
        //Abrir el stream usando un StreamReader para acceso sencillo.
        StreamReader reader = new  StreamReader(dataStream);
        // Leer el contenido.
        string responseFromServer = reader.ReadToEnd();
        // Mostrar el contenido.
        //Console.WriteLine(responseFromServer)

        // Paso opcional: guardar el contenido de response en un archivo de texto."
        // Cualquier directorio donde se desee almacenar el contenido del response. Este paso no es necesario si se leerán las variables de retorno como headers.
            string ruta = @"C:\Facturas\respuesta.txt";
        // Se usa una clase externa que simplemente guarda un contenido en un archivo de texto.
            System.IO.File.WriteAllText(ruta, responseFromServer);

        /************************************************************************
        ' Sección para obtener las variables de respuesta (HEADERS del RESPONSE).
        '************************************************************************/
        //Establecer un objeto del tipo UTF8Encoding.
        System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();
        //Declarar variable para almacenar los bytes.
        byte[] encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["Usuario"]);
            // Una vez decodificados y obtenidos, pueden simplemente almacenarse en un string para futuro uso.

        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["Usuario"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["Cuenta"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["EjecucionCorrecta"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["MensajeEjecucion"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["Serie"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        string SerieFolio = utf8.GetString(encodedBytes);
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["Folio"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        SerieFolio += utf8.GetString(encodedBytes);
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["Fecha"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["NumeroCertificado"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["NumeroAprobacion"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["AnoAprobacion"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["SelloDigital"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["SelloDigitalPACFD"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["CadenaOriginal"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["InfoPACFD"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["LeyendaDocumentoImpreso"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        Response.Write(utf8.GetString(encodedBytes) + "<br>");
        // Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["CreditosRestantes"]);
        // Escribir en pantalla el header transformando de UTF-8 al encoding por default.
        Response.Write(utf8.GetString(encodedBytes) + "<br>");

        /*******************************************************************************************
        ' Sección para obtener el CFD oficial, es decir, el archivo XML enviado en un string limpio.
        '*******************************************************************************************/

        //Obtener bytes usando el encoding default.
        encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(respuesta.Headers["XMLGenerado"]);
        //Obtener el string limpio, del código XML del CFD.
        string xmlGenerado = utf8.GetString(encodedBytes);

        //Crear un objeto XMLDocument.
        XmlDocument xmlDoc = new XmlDocument();
        //Cargar el contenido XML al objeto por medio del string (XML) recibido.
        xmlDoc.LoadXml(xmlGenerado);
        //Guardar el CFD en su formato XML usando el objeto XmlDocument.
        xmlDoc.Save(ruta + "FACTURA_" + Venta.ID + ".xml");

            //Hacer try.
        Venta.ClaveCFD = SerieFolio;

        var venta = db.Ventas.Find(Venta.ID);
        if (venta == null)
            return "* Error al recuperar la venta que se está procesando";
        
        
        
        
        try
        {
            
            // Crear un objeto tipo UTF8Encoding.
            System.Text.UTF8Encoding utf8Encoder = new System.Text.UTF8Encoding();
            // Crear un objeto Byte e inicializarlo.
            byte[] bufferXML = utf8Encoder.GetBytes(xmlGenerado);
            // Obtener en flujo de bytes el string XML.
            // Crear el archivo XML y tener internamente copia de mi CFD oficial.
            //GUARDAMOS EL XML EN LA BD
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            venta.ClaveCFD = SerieFolio;
            venta.FacturaXML = enc.GetString(bufferXML);
            db.SaveChanges();

            
            
            System.IO.File.WriteAllBytes(ruta + "FACTURA_" + Venta.ID + "_from_Stream.xml", bufferXML);
        }
        catch
        {
            
            // Mostrar mensaje de error.
            Response.Write("Imposible crear archivo XML.");
        }


            /*
             * Cerramos las conexiones!
             */
        // Cerrar el objeto StreamReader.
        reader.Close();
        // Cerrar stream de datos.
        dataStream.Close();
        // Cerrar el objeto WebResponse.
        respuesta.Close();





            return SerieFolio;
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
