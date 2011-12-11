using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeguriGasesERP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
/*
 * Incluirá Management
 */

namespace SeguriGasesERP.Models
{

    
    public class Cliente
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        /*
         * Campos de acuerdo a la especificacion de FEL para recibir los datos
         */
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        //Requerido, Nombre Corto del cliente
        [Required(ErrorMessage = "Es necesario indicar el Nombe del Cliente")]
        [DisplayName("Nombre Corto")] 
        public string NombreCliente { get; set; }
        //Opcional Nombre del contacto principal con el cliente
        [DisplayName("Nombre del contacto")] 
        public string Contacto { get; set; }
        //Opcional Telefono de contacto con la empresa
        public string Telefono { get; set; }
        //Opcional Email de contacto con la empresa
        public string Email { get; set; }
        //Requerido RFC del Cliente
        [DisplayName("RFC")] 
        [StringLength(13)]
        [Required(ErrorMessage = "Es obligatorio inicar el RFC del ciente")]
        public string RfcReceptor { get; set; }
        //Requerido Razon Social del cliente
        [DisplayName("Razon Social")] 
        [Required(ErrorMessage = "Es Obligatorio Indicar la Razon Social del cliente")]
        public string NombreReceptor { get; set; }
        //Requerido Calle del domicilio Fiscal del cliente
        [DisplayName("Calle")] 
        [Required(ErrorMessage = "Es Obligatorio incluir una calle en la Dirección del cliente")]
        public string CalleReceptor { get; set; }
        //Requerido Numero Exterior del Domicilio fiscal del cliente
        [DisplayName("Número Exterior")] 
        [Required(ErrorMessage = "Es Obligatorio incluir un Número Exterior")]
        public string NoExteriorReceptor { get; set; }
        //Opcional Numero Interior del Domicilio fiscal del cliente
        [DisplayName("Número Interior")] 
        public string NoInteriorReceptor { get; set; }
        //Requerido Colonia del Domicilio fiscal del cliente
        [Required(ErrorMessage = "Es Obligatorio incluir la Colonia")]
        [DisplayName("Colonia")] 
        public string ColoniaReceptor { get; set; }
        //Opcional Localidad del Cliente
        [DisplayName("Localidad")] 
        public string LocalidadReceptor { get; set; }
        //Opcional Descripción o referencia de la dirección fiscal del Cliente
        [DisplayName("Referencia")] 
        public string ReferenciaReceptor { get; set; }
        //Requerido El nombre del municipio de la dirección fiscal del Cliente
        [DisplayName("Municipio")] 
        [Required(ErrorMessage = "Es Obligatorio incluir el Municipio ")]
        public string MunicipioReceptor { get; set; }
        //Requerido El nombre del estado de la dirección fiscal del Receptor. Debe ser exáctamente igual a alguno de los valores permitidos.
        [DisplayName("Estado")] 
        [Required(ErrorMessage = "Es Obligatorio incluir el Estado")]
        public string EstadoReceptor { get; set; }

        /*  Valores permitidos para EstadoReceptor
            Aguascalientes
            Baja California
            Baja California Sur
            Campeche
            Chiapas
            Chihuahua
            Coahuila de Zaragoza
            Colima
            Distrito Federal
            Durango
            Estado de México
            Guanajuato
            Guerrero
            Hidalgo
            Jalisco
            Michoacán
            Morelos
            Nayarit
            Nuevo León
            Oaxaca
            Puebla
            Queretaro
            Quintana Roo
            San Luis Potosí
            Sinaloa
            Sonora
            Tabasco
            Tamaulipas
            Tlaxcala
            Veracruz
            Yucatán
            Zacatecas
            Estado Extranjero
         * 
         * */

        //Requerido El nombre del país de la dirección fiscal del Cliente
        [ScaffoldColumn(false)]
        public string PaisReceptor { get; set; }
        //Requerido El código postal de la dirección fiscal del Cliente
        [DisplayName("Código Postal")] 
        [Required(ErrorMessage = "Es Obligatorio incluir el Código Postal")]
        public string CodigoPostalReceptor { get; set; }

        

        public virtual List<Venta> Compras { get; set; }
        public virtual List<Pago> Pagos { get; set; }


        /*
         * Esta funcion verifica si un cliente es o no sugeto a credito, basandose en el límite de crédito que este tiene y el las facturas a credito que tiene
         * Si ya llego a su limite de credito o con el monto de la venta a realizar lo excede, no será sugeto a crédito. Tampoco será sugeto a crédito si tiene una o más facturas vencidas
         * No será sugeto a crédito si no existe una cuenta de crédito a su nombre.
         */
        public bool SugetoCredito()
        {
            //Primero buscamos si existe una cuenta de crédito a nombre del cliente
            var CuentaCredito = db.CuentasCredito.Single(cc => cc.IdCliente == ID);

            if (CuentaCredito == null)
                return false;

            //Verificamos si no se ha llegado alímite de crédito en días
            if (CuentaCredito.VentasVencidas())
                return false;

            //Verificamos si no excede su límite de credito en dinero
            if (CuentaCredito.getDeuda() > CuentaCredito.MontoLimite)
                return false;

            return true;

        }

        public bool SugetoCredito(decimal proximaVenta)
        {
            //Primero buscamos si existe una cuenta de crédito a nombre del cliente
            var CuentaCredito = db.CuentasCredito.Find(ID);

            if (CuentaCredito == null)
                return false;

            //Verificamos si no se ha llegado alímite de crédito en días
            
                if (CuentaCredito.VentasVencidas())
                    return false;
        

        

            //Verificamos si no excede su límite de credito en dinero
            if ((CuentaCredito.getDeuda() + proximaVenta) > CuentaCredito.MontoLimite)
                return false;

            return true;

        }
        
    }
}