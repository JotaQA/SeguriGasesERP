﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SeguriGasesERP.WSRemota32 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ArrayOfString", Namespace="https://www.fel.mx/ConexionRemotaCFDI", ItemName="string")]
    [System.SerializableAttribute()]
    public class ArrayOfString : System.Collections.Generic.List<string> {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://www.fel.mx/ConexionRemotaCFDI", ConfigurationName="WSRemota32.ConexionRemota32Soap")]
    public interface ConexionRemota32Soap {
        
        // CODEGEN: Generating message contract since element name datosUsuario from namespace https://www.fel.mx/ConexionRemotaCFDI is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="https://www.fel.mx/ConexionRemotaCFDI/GenerarCFDIv32", ReplyAction="*")]
        SeguriGasesERP.WSRemota32.GenerarCFDIv32Response GenerarCFDIv32(SeguriGasesERP.WSRemota32.GenerarCFDIv32Request request);
        
        // CODEGEN: Generating message contract since element name datosUsuario from namespace https://www.fel.mx/ConexionRemotaCFDI is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="https://www.fel.mx/ConexionRemotaCFDI/GenerarCodigoBidimensional", ReplyAction="*")]
        SeguriGasesERP.WSRemota32.GenerarCodigoBidimensionalResponse GenerarCodigoBidimensional(SeguriGasesERP.WSRemota32.GenerarCodigoBidimensionalRequest request);
        
        // CODEGEN: Generating message contract since element name datosUsuario from namespace https://www.fel.mx/ConexionRemotaCFDI is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="https://www.fel.mx/ConexionRemotaCFDI/CancelarCFDI", ReplyAction="*")]
        SeguriGasesERP.WSRemota32.CancelarCFDIResponse CancelarCFDI(SeguriGasesERP.WSRemota32.CancelarCFDIRequest request);
        
        // CODEGEN: Generating message contract since element name datosUsuario from namespace https://www.fel.mx/ConexionRemotaCFDI is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="https://www.fel.mx/ConexionRemotaCFDI/EnviarCFDI", ReplyAction="*")]
        SeguriGasesERP.WSRemota32.EnviarCFDIResponse EnviarCFDI(SeguriGasesERP.WSRemota32.EnviarCFDIRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GenerarCFDIv32Request {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GenerarCFDIv32", Namespace="https://www.fel.mx/ConexionRemotaCFDI", Order=0)]
        public SeguriGasesERP.WSRemota32.GenerarCFDIv32RequestBody Body;
        
        public GenerarCFDIv32Request() {
        }
        
        public GenerarCFDIv32Request(SeguriGasesERP.WSRemota32.GenerarCFDIv32RequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.fel.mx/ConexionRemotaCFDI")]
    public partial class GenerarCFDIv32RequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public SeguriGasesERP.WSRemota32.ArrayOfString datosUsuario;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public SeguriGasesERP.WSRemota32.ArrayOfString datosReceptor;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public SeguriGasesERP.WSRemota32.ArrayOfString datosCFDI;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public SeguriGasesERP.WSRemota32.ArrayOfString etiquetas;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public SeguriGasesERP.WSRemota32.ArrayOfString conceptos;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public SeguriGasesERP.WSRemota32.ArrayOfString informacionAduanera;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public SeguriGasesERP.WSRemota32.ArrayOfString retenciones;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public SeguriGasesERP.WSRemota32.ArrayOfString traslados;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
        public SeguriGasesERP.WSRemota32.ArrayOfString retencionesLocales;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=9)]
        public SeguriGasesERP.WSRemota32.ArrayOfString trasladosLocales;
        
        public GenerarCFDIv32RequestBody() {
        }
        
        public GenerarCFDIv32RequestBody(SeguriGasesERP.WSRemota32.ArrayOfString datosUsuario, SeguriGasesERP.WSRemota32.ArrayOfString datosReceptor, SeguriGasesERP.WSRemota32.ArrayOfString datosCFDI, SeguriGasesERP.WSRemota32.ArrayOfString etiquetas, SeguriGasesERP.WSRemota32.ArrayOfString conceptos, SeguriGasesERP.WSRemota32.ArrayOfString informacionAduanera, SeguriGasesERP.WSRemota32.ArrayOfString retenciones, SeguriGasesERP.WSRemota32.ArrayOfString traslados, SeguriGasesERP.WSRemota32.ArrayOfString retencionesLocales, SeguriGasesERP.WSRemota32.ArrayOfString trasladosLocales) {
            this.datosUsuario = datosUsuario;
            this.datosReceptor = datosReceptor;
            this.datosCFDI = datosCFDI;
            this.etiquetas = etiquetas;
            this.conceptos = conceptos;
            this.informacionAduanera = informacionAduanera;
            this.retenciones = retenciones;
            this.traslados = traslados;
            this.retencionesLocales = retencionesLocales;
            this.trasladosLocales = trasladosLocales;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GenerarCFDIv32Response {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GenerarCFDIv32Response", Namespace="https://www.fel.mx/ConexionRemotaCFDI", Order=0)]
        public SeguriGasesERP.WSRemota32.GenerarCFDIv32ResponseBody Body;
        
        public GenerarCFDIv32Response() {
        }
        
        public GenerarCFDIv32Response(SeguriGasesERP.WSRemota32.GenerarCFDIv32ResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.fel.mx/ConexionRemotaCFDI")]
    public partial class GenerarCFDIv32ResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public SeguriGasesERP.WSRemota32.ArrayOfString GenerarCFDIv32Result;
        
        public GenerarCFDIv32ResponseBody() {
        }
        
        public GenerarCFDIv32ResponseBody(SeguriGasesERP.WSRemota32.ArrayOfString GenerarCFDIv32Result) {
            this.GenerarCFDIv32Result = GenerarCFDIv32Result;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GenerarCodigoBidimensionalRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GenerarCodigoBidimensional", Namespace="https://www.fel.mx/ConexionRemotaCFDI", Order=0)]
        public SeguriGasesERP.WSRemota32.GenerarCodigoBidimensionalRequestBody Body;
        
        public GenerarCodigoBidimensionalRequest() {
        }
        
        public GenerarCodigoBidimensionalRequest(SeguriGasesERP.WSRemota32.GenerarCodigoBidimensionalRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.fel.mx/ConexionRemotaCFDI")]
    public partial class GenerarCodigoBidimensionalRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public SeguriGasesERP.WSRemota32.ArrayOfString datosUsuario;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string UUID;
        
        public GenerarCodigoBidimensionalRequestBody() {
        }
        
        public GenerarCodigoBidimensionalRequestBody(SeguriGasesERP.WSRemota32.ArrayOfString datosUsuario, string UUID) {
            this.datosUsuario = datosUsuario;
            this.UUID = UUID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GenerarCodigoBidimensionalResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GenerarCodigoBidimensionalResponse", Namespace="https://www.fel.mx/ConexionRemotaCFDI", Order=0)]
        public SeguriGasesERP.WSRemota32.GenerarCodigoBidimensionalResponseBody Body;
        
        public GenerarCodigoBidimensionalResponse() {
        }
        
        public GenerarCodigoBidimensionalResponse(SeguriGasesERP.WSRemota32.GenerarCodigoBidimensionalResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.fel.mx/ConexionRemotaCFDI")]
    public partial class GenerarCodigoBidimensionalResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public SeguriGasesERP.WSRemota32.ArrayOfString GenerarCodigoBidimensionalResult;
        
        public GenerarCodigoBidimensionalResponseBody() {
        }
        
        public GenerarCodigoBidimensionalResponseBody(SeguriGasesERP.WSRemota32.ArrayOfString GenerarCodigoBidimensionalResult) {
            this.GenerarCodigoBidimensionalResult = GenerarCodigoBidimensionalResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CancelarCFDIRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CancelarCFDI", Namespace="https://www.fel.mx/ConexionRemotaCFDI", Order=0)]
        public SeguriGasesERP.WSRemota32.CancelarCFDIRequestBody Body;
        
        public CancelarCFDIRequest() {
        }
        
        public CancelarCFDIRequest(SeguriGasesERP.WSRemota32.CancelarCFDIRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.fel.mx/ConexionRemotaCFDI")]
    public partial class CancelarCFDIRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public SeguriGasesERP.WSRemota32.ArrayOfString datosUsuario;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public SeguriGasesERP.WSRemota32.ArrayOfString ListaACancelar;
        
        public CancelarCFDIRequestBody() {
        }
        
        public CancelarCFDIRequestBody(SeguriGasesERP.WSRemota32.ArrayOfString datosUsuario, SeguriGasesERP.WSRemota32.ArrayOfString ListaACancelar) {
            this.datosUsuario = datosUsuario;
            this.ListaACancelar = ListaACancelar;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CancelarCFDIResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CancelarCFDIResponse", Namespace="https://www.fel.mx/ConexionRemotaCFDI", Order=0)]
        public SeguriGasesERP.WSRemota32.CancelarCFDIResponseBody Body;
        
        public CancelarCFDIResponse() {
        }
        
        public CancelarCFDIResponse(SeguriGasesERP.WSRemota32.CancelarCFDIResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.fel.mx/ConexionRemotaCFDI")]
    public partial class CancelarCFDIResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public SeguriGasesERP.WSRemota32.ArrayOfString CancelarCFDIResult;
        
        public CancelarCFDIResponseBody() {
        }
        
        public CancelarCFDIResponseBody(SeguriGasesERP.WSRemota32.ArrayOfString CancelarCFDIResult) {
            this.CancelarCFDIResult = CancelarCFDIResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EnviarCFDIRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="EnviarCFDI", Namespace="https://www.fel.mx/ConexionRemotaCFDI", Order=0)]
        public SeguriGasesERP.WSRemota32.EnviarCFDIRequestBody Body;
        
        public EnviarCFDIRequest() {
        }
        
        public EnviarCFDIRequest(SeguriGasesERP.WSRemota32.EnviarCFDIRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.fel.mx/ConexionRemotaCFDI")]
    public partial class EnviarCFDIRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public SeguriGasesERP.WSRemota32.ArrayOfString datosUsuario;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string UUID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string email;
        
        public EnviarCFDIRequestBody() {
        }
        
        public EnviarCFDIRequestBody(SeguriGasesERP.WSRemota32.ArrayOfString datosUsuario, string UUID, string email) {
            this.datosUsuario = datosUsuario;
            this.UUID = UUID;
            this.email = email;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EnviarCFDIResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="EnviarCFDIResponse", Namespace="https://www.fel.mx/ConexionRemotaCFDI", Order=0)]
        public SeguriGasesERP.WSRemota32.EnviarCFDIResponseBody Body;
        
        public EnviarCFDIResponse() {
        }
        
        public EnviarCFDIResponse(SeguriGasesERP.WSRemota32.EnviarCFDIResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.fel.mx/ConexionRemotaCFDI")]
    public partial class EnviarCFDIResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public SeguriGasesERP.WSRemota32.ArrayOfString EnviarCFDIResult;
        
        public EnviarCFDIResponseBody() {
        }
        
        public EnviarCFDIResponseBody(SeguriGasesERP.WSRemota32.ArrayOfString EnviarCFDIResult) {
            this.EnviarCFDIResult = EnviarCFDIResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ConexionRemota32SoapChannel : SeguriGasesERP.WSRemota32.ConexionRemota32Soap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ConexionRemota32SoapClient : System.ServiceModel.ClientBase<SeguriGasesERP.WSRemota32.ConexionRemota32Soap>, SeguriGasesERP.WSRemota32.ConexionRemota32Soap {
        
        public ConexionRemota32SoapClient() {
        }
        
        public ConexionRemota32SoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ConexionRemota32SoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ConexionRemota32SoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ConexionRemota32SoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SeguriGasesERP.WSRemota32.GenerarCFDIv32Response SeguriGasesERP.WSRemota32.ConexionRemota32Soap.GenerarCFDIv32(SeguriGasesERP.WSRemota32.GenerarCFDIv32Request request) {
            return base.Channel.GenerarCFDIv32(request);
        }
        
        public SeguriGasesERP.WSRemota32.ArrayOfString GenerarCFDIv32(SeguriGasesERP.WSRemota32.ArrayOfString datosUsuario, SeguriGasesERP.WSRemota32.ArrayOfString datosReceptor, SeguriGasesERP.WSRemota32.ArrayOfString datosCFDI, SeguriGasesERP.WSRemota32.ArrayOfString etiquetas, SeguriGasesERP.WSRemota32.ArrayOfString conceptos, SeguriGasesERP.WSRemota32.ArrayOfString informacionAduanera, SeguriGasesERP.WSRemota32.ArrayOfString retenciones, SeguriGasesERP.WSRemota32.ArrayOfString traslados, SeguriGasesERP.WSRemota32.ArrayOfString retencionesLocales, SeguriGasesERP.WSRemota32.ArrayOfString trasladosLocales) {
            SeguriGasesERP.WSRemota32.GenerarCFDIv32Request inValue = new SeguriGasesERP.WSRemota32.GenerarCFDIv32Request();
            inValue.Body = new SeguriGasesERP.WSRemota32.GenerarCFDIv32RequestBody();
            inValue.Body.datosUsuario = datosUsuario;
            inValue.Body.datosReceptor = datosReceptor;
            inValue.Body.datosCFDI = datosCFDI;
            inValue.Body.etiquetas = etiquetas;
            inValue.Body.conceptos = conceptos;
            inValue.Body.informacionAduanera = informacionAduanera;
            inValue.Body.retenciones = retenciones;
            inValue.Body.traslados = traslados;
            inValue.Body.retencionesLocales = retencionesLocales;
            inValue.Body.trasladosLocales = trasladosLocales;
            SeguriGasesERP.WSRemota32.GenerarCFDIv32Response retVal = ((SeguriGasesERP.WSRemota32.ConexionRemota32Soap)(this)).GenerarCFDIv32(inValue);
            return retVal.Body.GenerarCFDIv32Result;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SeguriGasesERP.WSRemota32.GenerarCodigoBidimensionalResponse SeguriGasesERP.WSRemota32.ConexionRemota32Soap.GenerarCodigoBidimensional(SeguriGasesERP.WSRemota32.GenerarCodigoBidimensionalRequest request) {
            return base.Channel.GenerarCodigoBidimensional(request);
        }
        
        public SeguriGasesERP.WSRemota32.ArrayOfString GenerarCodigoBidimensional(SeguriGasesERP.WSRemota32.ArrayOfString datosUsuario, string UUID) {
            SeguriGasesERP.WSRemota32.GenerarCodigoBidimensionalRequest inValue = new SeguriGasesERP.WSRemota32.GenerarCodigoBidimensionalRequest();
            inValue.Body = new SeguriGasesERP.WSRemota32.GenerarCodigoBidimensionalRequestBody();
            inValue.Body.datosUsuario = datosUsuario;
            inValue.Body.UUID = UUID;
            SeguriGasesERP.WSRemota32.GenerarCodigoBidimensionalResponse retVal = ((SeguriGasesERP.WSRemota32.ConexionRemota32Soap)(this)).GenerarCodigoBidimensional(inValue);
            return retVal.Body.GenerarCodigoBidimensionalResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SeguriGasesERP.WSRemota32.CancelarCFDIResponse SeguriGasesERP.WSRemota32.ConexionRemota32Soap.CancelarCFDI(SeguriGasesERP.WSRemota32.CancelarCFDIRequest request) {
            return base.Channel.CancelarCFDI(request);
        }
        
        public SeguriGasesERP.WSRemota32.ArrayOfString CancelarCFDI(SeguriGasesERP.WSRemota32.ArrayOfString datosUsuario, SeguriGasesERP.WSRemota32.ArrayOfString ListaACancelar) {
            SeguriGasesERP.WSRemota32.CancelarCFDIRequest inValue = new SeguriGasesERP.WSRemota32.CancelarCFDIRequest();
            inValue.Body = new SeguriGasesERP.WSRemota32.CancelarCFDIRequestBody();
            inValue.Body.datosUsuario = datosUsuario;
            inValue.Body.ListaACancelar = ListaACancelar;
            SeguriGasesERP.WSRemota32.CancelarCFDIResponse retVal = ((SeguriGasesERP.WSRemota32.ConexionRemota32Soap)(this)).CancelarCFDI(inValue);
            return retVal.Body.CancelarCFDIResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SeguriGasesERP.WSRemota32.EnviarCFDIResponse SeguriGasesERP.WSRemota32.ConexionRemota32Soap.EnviarCFDI(SeguriGasesERP.WSRemota32.EnviarCFDIRequest request) {
            return base.Channel.EnviarCFDI(request);
        }
        
        public SeguriGasesERP.WSRemota32.ArrayOfString EnviarCFDI(SeguriGasesERP.WSRemota32.ArrayOfString datosUsuario, string UUID, string email) {
            SeguriGasesERP.WSRemota32.EnviarCFDIRequest inValue = new SeguriGasesERP.WSRemota32.EnviarCFDIRequest();
            inValue.Body = new SeguriGasesERP.WSRemota32.EnviarCFDIRequestBody();
            inValue.Body.datosUsuario = datosUsuario;
            inValue.Body.UUID = UUID;
            inValue.Body.email = email;
            SeguriGasesERP.WSRemota32.EnviarCFDIResponse retVal = ((SeguriGasesERP.WSRemota32.ConexionRemota32Soap)(this)).EnviarCFDI(inValue);
            return retVal.Body.EnviarCFDIResult;
        }
    }
}