﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceEstadoUpload
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://DefaultNamespace", ConfigurationName="ServiceEstadoUpload.QueryEstUp")]
    public interface QueryEstUp
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getEstUpReturn")]
        string getEstUp(string RutCompania, string DvCompania, string TrackId, string Token);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getEstUpReturn")]
        System.Threading.Tasks.Task<string> getEstUpAsync(string RutCompania, string DvCompania, string TrackId, string Token);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getVersionMayorReturn")]
        string getVersionMayor();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getVersionMayorReturn")]
        System.Threading.Tasks.Task<string> getVersionMayorAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getVersionMenorReturn")]
        string getVersionMenor();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getVersionMenorReturn")]
        System.Threading.Tasks.Task<string> getVersionMenorAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getVersionPatchReturn")]
        string getVersionPatch();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getVersionPatchReturn")]
        System.Threading.Tasks.Task<string> getVersionPatchAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getVersionReturn")]
        string getVersion();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getVersionReturn")]
        System.Threading.Tasks.Task<string> getVersionAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getStateReturn")]
        string getState();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getStateReturn")]
        System.Threading.Tasks.Task<string> getStateAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public interface QueryEstUpChannel : ServiceEstadoUpload.QueryEstUp, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public partial class QueryEstUpClient : System.ServiceModel.ClientBase<ServiceEstadoUpload.QueryEstUp>, ServiceEstadoUpload.QueryEstUp
    {
        
        /// <summary>
        /// Implemente este método parcial para configurar el punto de conexión de servicio.
        /// </summary>
        /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
        /// <param name="clientCredentials">Credenciales de cliente</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public QueryEstUpClient() : 
                base(QueryEstUpClient.GetDefaultBinding(), QueryEstUpClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.QueryEstUp.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public QueryEstUpClient(EndpointConfiguration endpointConfiguration) : 
                base(QueryEstUpClient.GetBindingForEndpoint(endpointConfiguration), QueryEstUpClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public QueryEstUpClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(QueryEstUpClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public QueryEstUpClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(QueryEstUpClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public QueryEstUpClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public string getEstUp(string RutCompania, string DvCompania, string TrackId, string Token)
        {
            return base.Channel.getEstUp(RutCompania, DvCompania, TrackId, Token);
        }
        
        public System.Threading.Tasks.Task<string> getEstUpAsync(string RutCompania, string DvCompania, string TrackId, string Token)
        {
            return base.Channel.getEstUpAsync(RutCompania, DvCompania, TrackId, Token);
        }
        
        public string getVersionMayor()
        {
            return base.Channel.getVersionMayor();
        }
        
        public System.Threading.Tasks.Task<string> getVersionMayorAsync()
        {
            return base.Channel.getVersionMayorAsync();
        }
        
        public string getVersionMenor()
        {
            return base.Channel.getVersionMenor();
        }
        
        public System.Threading.Tasks.Task<string> getVersionMenorAsync()
        {
            return base.Channel.getVersionMenorAsync();
        }
        
        public string getVersionPatch()
        {
            return base.Channel.getVersionPatch();
        }
        
        public System.Threading.Tasks.Task<string> getVersionPatchAsync()
        {
            return base.Channel.getVersionPatchAsync();
        }
        
        public string getVersion()
        {
            return base.Channel.getVersion();
        }
        
        public System.Threading.Tasks.Task<string> getVersionAsync()
        {
            return base.Channel.getVersionAsync();
        }
        
        public string getState()
        {
            return base.Channel.getState();
        }
        
        public System.Threading.Tasks.Task<string> getStateAsync()
        {
            return base.Channel.getStateAsync();
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.QueryEstUp))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.QueryEstUp))
            {
                return new System.ServiceModel.EndpointAddress("https://palena.sii.cl/DTEWS/QueryEstUp.jws");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return QueryEstUpClient.GetBindingForEndpoint(EndpointConfiguration.QueryEstUp);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return QueryEstUpClient.GetEndpointAddress(EndpointConfiguration.QueryEstUp);
        }
        
        public enum EndpointConfiguration
        {
            
            QueryEstUp,
        }
    }
}