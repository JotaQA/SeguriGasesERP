using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeguriGasesERP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SeguriGasesERP.Models
{
    [DecimalPrecision(18, 2)] 
    public class CuentaCredito
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        /*
         * Cuenta de Crédito para el cliente
         */
        public int ID { get; set; }
        public DateTime FechaCreado { get; set; }
        [Required(ErrorMessage = "Es Obligatorio los días de limite en  el crédito")]
        [Range(1, 120, ErrorMessage = "Los días de crédito deben ser entre 1 y 120 días")]
        public int DiasLimite { get; set; }

        
        [Required(ErrorMessage = "Es Obligatorio incluir un Límite de Crédito")]
        [DecimalPrecision(18, 2)]
        public decimal MontoLimite { get; set; }
        [Required(ErrorMessage = "Es Obligatorio relacionar la Cuenta con un Usuario")]
        public int IdCliente { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual List<Venta> Ventas { get; set; }
        /*
         * Esta funcion determina si se tiene una venta vencida, es decir aun se debe una factura y ya se excedió el tiempo establecido de pago
         */
        public bool VentasVencidas()
        {
            /*
             * 1.- Recuperamos la ventas que no han sido liquidadas
             * 2.- Restamos al día de hoy la fecha de la factura más vieja, si el resultado es mayor a DiasLimite, regresamos true, sino false
             */

            var VentasDeuda = from ventas in db.Ventas
                              where ventas.Liquidado == false && ventas.IdCliente == IdCliente
                              orderby ventas.FechaVenta ascending
                              select ventas;
            //Si no hay ninguna deuda, no hay ventas vencidas
            if (VentasDeuda.Count() == 0)
                return false;
            TimeSpan dias = DateTime.Now.Subtract(VentasDeuda.First().FechaVenta);
            if (dias.Days > DiasLimite)
            {
                return true;
            }

            return false;
        }

        public int VentasMasVieja()
        {
            /*
             * 1.- Recuperamos la ventas que no han sido liquidadas
             * 2.- Restamos al día de hoy la fecha de la factura más vieja, si el resultado es mayor a DiasLimite, regresamos true, sino false
             */

            var VentasDeuda = from ventas in db.Ventas
                              where ventas.Liquidado == false && ventas.IdCliente == IdCliente
                              orderby ventas.FechaVenta ascending
                              select ventas;
            //Si no hay ninguna deuda, no hay ventas vencidas
            if (VentasDeuda.Count() == 0)
                return -1;
            TimeSpan dias = DateTime.Now.Subtract(VentasDeuda.First().FechaVenta);
            return dias.Days;
            
        }

        /*
         * Esta funcion nos indicará el monto total de la deuda para la cuenta de crédito
         */
        public decimal getDeuda()
        {
            /*
             * Para saber la deuda, necesitamos saber las ventas que se le han hecho a dicho cliente que han sido enviadas a crédito y que no han sido liquidadas.
             * Una vez teniendo estas ventas, por cada venta qe obtenemos, buscamos los pagos que se han relaizado a la misma y se sumara la diferencia entre lo adeudado y los pagos realizados al total.
             */

            decimal deuda = 0;

            /*
             * 1.- Seleccionar las ventas con adeudo que tiene el cliente             
             */
            var vent = from venta in db.Ventas
                         where venta.Liquidado == false && venta.IdCliente == IdCliente
                         orderby venta.FechaVenta ascending
                         select venta;

            List<Venta> ventas = vent.ToList();
          
            /*
             * 2.- Verificamos que la venta adeudada más vieja sea menos antigüa que su plazo de pago
             */

            //Si no tienen adeudos
            if (ventas.Count() == 0)
                return 0;

            /*Buscamos los pagos para cada venta*/
            foreach(var item in ventas)
            {
                decimal deudaVenta = item.Total;

                var pagos = from p in db.Pagos
                            where p.IdVenta == item.ID
                            select p;

                //Si no se han realizado pagos, se suma a deuda el total de la venta con iva.
                if(pagos == null)
                {
                    deuda += deudaVenta;
                    continue;
                }

                foreach(var abono in pagos)
                {
                    deudaVenta -= abono.Monto;
                }

                deuda += deudaVenta;
            }

            return deuda;
        }
         
    }
}