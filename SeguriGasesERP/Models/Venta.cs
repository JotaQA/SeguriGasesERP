using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeguriGasesERP.Models;


namespace SeguriGasesERP.Models
{
    [DecimalPrecision(18, 2)] 
    public class Venta
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        public int ID { get; set; }
        
        //Requerido: Tipo de CFD... Para FEL:MX  MAX. 3
        /*
         * Unicos valores Admitidos
         * FAC	Para el caso de emisión de Factura. Función fiscal: Ingreso.
         * HON	Para el caso de emisión de Recibo de Honorarios. Función fiscal: Ingreso.
         * ARR	Para el caso de emisión de Recibo de Arrendamiento. Función fiscal: Ingreso.
         * PAG	Para el caso de emisión de Nota de Pago. Función fiscal: Ingreso.
         * DON	Para el caso de emisión de Recibo de Donativo. Función fiscal: Ingreso.
         * CAR	Para el caso de emisión de Nota de Cargo. Función fiscal: Ingreso.
         * CRE	Para el caso de emisión de Nota de Crédito. Función fiscal: Egreso.
         * DEV	Para el caso de emisión de Nota de Devolución. Función fiscal: Egreso.
         * POR	Para el caso de emisión de Carta Porte. Función fiscal: Traslado.
         */
        public string ClaveCFD { get; set; }
        
        //Requerido: Forma de Pago 
        /*
         * Valores Permitidos:
         * Pago en una sola exhibición	Leyenda que deberá ir como forma de pago.
         * Parcialidades	Leyenda que deberá ir como forma de pago en caso de parcialidades.  En este caso deberá incluirse la variable "parcialidades" como indicador.
         */
        public string FormaDePago { get; set; }

        //Opcional: Parcialidades... Indicador del pago en parcialidades. Deberá separarse el pago del total por carácter: "/". Ejemplo: 2/4
        public string Parcialidades { get; set; }

        //Opcional: Condiciones de Pago Leyenda que indique las condiciones de pago.
        public string CondicionesDePago { get; set; }

        //Opcional Método de Pago. Leyenda que indica el método de pago utilizado.
        /*
         * Valores Permitidos:
         * Cheque
         * Efectivo
         * Transferencia bancaria
         * Tarjeta de crédito
         * tarjeta de débito
         */
        public string MetodoDePago { get; set; }

        //Opcional Descuento. Cifra decimal que representa el total (con seis decimales) del descuento. Ejemplo: 2750.750000
        public decimal Descuento { get; set; }

        //Opcional: Porcentaje Descuento
        //Cifra decimal (con seis decimales) que representa el porcentaje (de 0 a 100) del descuento. Ejemplo: 10.000000 (indicando un 10% de descuento).
        [DecimalPrecision(18, 2)] 
        public decimal PorcentajeDescuento { get; set; }

        //Opcional: Motivo Descuento
        //Leyenda que especifica el motivo del descuento.
        
        public string MotivoDescuento { get; set; }

        //Requerido: Moneda Indicador del tipo de moneda utilizada en el CFD.
        //MAXIMA LENGTH 3
        /*
         * Valores Permitidos:
         * MXN
         * USD
         * EUR
         */
        public string Moneda { get; set; }

        //Opcional: Tipo de Cambio. 
        //Cifra decimal (con seis decimales) que indica el tipo de cambio con respecto a la moneda nacional. Ejemplo 12.637000. Es REQUERIDO cuando se use moneda extranjera.
        [DecimalPrecision(18, 2)] 
        public decimal TipoCambio { get; set; }
        
        //Opcional: Fecha del Tipo de Cambio
        //Cadena abierta que especifica la fecha del tipo de cambio.
        public DateTime FechaTipoCambio { get; set; }

        //Requerido: Subtotal
        //Cifra decimal (con seis decimales) que representa el subtotal del CFD. Ejemplo: 10540.550000
        [DecimalPrecision(18, 2)] 
        public decimal Subtotal { get; set; }

        //Requerido: Total
        //Cifra decimal (con seis decimales) que representa el total del CFD. Ejemplo 12600.000000
        [DecimalPrecision(18, 2)] 
        public decimal Total { get; set; }

        //Requerido: Importe Con Letra
        //Leyenda que representa el importe con letra.+
        //MAX LENGTH: 200
        public string ImporteLetra { get; set; }

        //Requerido: Impuestos Retenidos
        //Cifra decimal (con seis decimales) que indica el total de impuestos retenidos del CFD. Ejemplo: 2700.000000
        [DecimalPrecision(18, 2)] 
        public decimal TotalImpuestosRetenidos { get; set; }

        //Requerido: Impuestos Trasladados
        //Cifra decimal (con seis decimales) que indica el total de impuestos trasladados del CFD. Ejemplo: 3200.000000
        [DecimalPrecision(18, 2)] 
        public decimal TotalImpuestosTrasladados { get; set; }

        //Fecha de la venta
        public DateTime FechaVenta { get; set; }

        public bool Liquidado { get; set; }

        /*
         * La información de los impuestos trasladados se incluye de manera estática en los controladores
         * a la hora de enviar la peticion post al servidor de facturación electrónica.
         */

        public int IdSucursal { get; set; }
        public int IdCliente { get; set; }
        //public int IdFactura { get; set; }
        public string FacturaXML { get; set; }
        public string usuario { get; set; }

        public virtual List<ProductoVenta> ProductosVenta { get; set; }
        //Cada venta pertenece a una Sucursal
        public virtual Sucursal Sucursal { get; set; }
        public virtual Cliente Cliente { get; set; }
        //public virtual Factura Factura { get; set; }
        //Relacion con la cuenta de crédito
        
        /*
         * Esta funcion cancela la venta, regresando los productos al inventario
         */
        public int CancelarVenta(int IdSucursal)
        {
            foreach(ProductoVenta item in ProductosVenta)
            {
                try{
                    /*
                     * Buscamos el elemento en ProductoVenta para eliminarlo
                     */
                    var PV = db.ProductosVenta.Single( p => p.VentaId == ID && p.ProductoId == item.ID);

                    //Lo eliminamos
                    db.ProductosVenta.Remove(PV);

                    /*
                     * Buscamos el ProductoSucursal de item para poder reestablecer su producto
                     */

                    var PS = db.ProductosSucursal.Single(ps => ps.IdProducto == item.ID && ps.IdSucursal == IdSucursal);

                    PS.cantidad += item.Count;

                    /*
                     * Aqui generamos un movimiento de regreso
                     */

                    MovimientoAlmacen movimiento = new MovimientoAlmacen
                    {
                        DescripcionMovimiento = "Regreso a inventario por cancelacion de la Venta " + ID + "Con folio y serie " + ClaveCFD,
                        Count = item.Count,
                        FechaMovimiento = System.DateTime.Now,
                        OrdenDeCompra = null,
                        IdOrednCompra = 0,
                        IdProducto = item.ProductoId,
                        Producto = db.Productos.Find(item.ProductoId),
                        IdSucursal = IdSucursal,
                        Sucursal = db.Sucursales.Find(IdSucursal),
                        TipoMovimiento = "Regreso a Inventario",                        
                        IdVenta = 0
                    };

                    db.MovimientosAlmacen.Add(movimiento);
                    db.SaveChanges();
                }
                catch
                {
                    return 0;
                }
                
            }

            //ahora eliminamos por completo nuesta venta
            try
            {
                var vent = db.Ventas.Find(ID);

                db.Ventas.Remove(vent);
                db.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
         

            



        }

        public string getFechaVencimiento(int IdCuenta)
        {
            CuentaCredito Cuenta = db.CuentasCredito.Find(IdCuenta);

            DateTime vence = FechaVenta.AddDays((double)Cuenta.DiasLimite);

            return vence.Month + "/" + vence.Day + "/" + vence.Year;

        }
        public int getDiasVencido(int IdCuenta)
        {
            if (Liquidado)
                return 0;

            CuentaCredito Cuenta = db.CuentasCredito.Find(IdCuenta);

            TimeSpan dias = DateTime.Now.Subtract(FechaVenta);

            int result = dias.Days - Cuenta.DiasLimite;;
            if (result < 0)
                return 0;
            return result;

        }

        public decimal getDeuda()
        {
            if (Liquidado)
                return 0;

            /*Buscamos todos los pagos relacionados con esta venta*/

            List<Pago> pagos = db.Pagos.Where(v => v.IdVenta == ID).ToList();
            decimal pagado = 0;
            foreach (var item in pagos)
            {
                pagado += item.Monto;
            }

            /*Restamos los pagos del total*/

            decimal deuda = Total - pagado;

            return deuda;
        }



    }
}