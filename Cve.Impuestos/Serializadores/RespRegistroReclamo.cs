using System.Xml.Serialization;

namespace Cve.Impuestos.Serializadores
{
    public class RespConsultarFechaRecepcionSii
    {
        // NOTA: El código generado puede requerir, como mínimo, .NET Framework 4.5 o .NET Core/Standard 2.0.
        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [XmlRoot(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {
            private EnvelopeBody? bodyField;

            /// <remarks/>
            public EnvelopeBody? Body
            {
                get => bodyField;
                set => bodyField = value;
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {
            private consultarFechaRecepcionSiiResponse? consultarFechaRecepcionSiiResponseField;

            /// <remarks/>
            [XmlElement(Namespace = "http://ws.registroreclamodte.diii.sdi.sii.cl")]
            public consultarFechaRecepcionSiiResponse? consultarFechaRecepcionSiiResponse
            {
                get => consultarFechaRecepcionSiiResponseField;
                set => consultarFechaRecepcionSiiResponseField = value;
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "http://ws.registroreclamodte.diii.sdi.sii.cl")]
        [XmlRoot(Namespace = "http://ws.registroreclamodte.diii.sdi.sii.cl", IsNullable = false)]
        public partial class consultarFechaRecepcionSiiResponse
        {
            private string? returnField;

            /// <remarks/>
            [XmlElement(Namespace = "")]
            public string? @return
            {
                get => returnField;
                set => returnField = value;
            }
        }
    }

    public class RespListarEventosHistDoc
    {
        // NOTA: El código generado puede requerir, como mínimo, .NET Framework 4.5 o .NET Core/Standard 2.0.
        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [XmlRoot(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {
            private EnvelopeBody? bodyField;

            /// <remarks/>
            public EnvelopeBody? Body
            {
                get => bodyField;
                set => bodyField = value;
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {
            private listarEventosHistDocResponse? listarEventosHistDocResponseField;

            /// <remarks/>
            [XmlElement(Namespace = "http://ws.registroreclamodte.diii.sdi.sii.cl")]
            public listarEventosHistDocResponse? listarEventosHistDocResponse
            {
                get => listarEventosHistDocResponseField;
                set => listarEventosHistDocResponseField = value;
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "http://ws.registroreclamodte.diii.sdi.sii.cl")]
        [XmlRoot(Namespace = "http://ws.registroreclamodte.diii.sdi.sii.cl", IsNullable = false)]
        public partial class listarEventosHistDocResponse
        {
            private @return? returnField;

            /// <remarks/>
            [XmlElement(Namespace = "")]
            public @return? @return
            {
                get => returnField;
                set => returnField = value;
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [XmlType(AnonymousType = true)]
        [XmlRoot(Namespace = "", IsNullable = false)]
        public partial class @return
        {
            private byte codRespField;

            private string? descRespField;

            private returnListaEventosDoc? listaEventosDocField;

            /// <remarks/>
            public byte codResp
            {
                get => codRespField;
                set => codRespField = value;
            }

            /// <remarks/>
            public string? descResp
            {
                get => descRespField;
                set => descRespField = value;
            }

            /// <remarks/>
            public returnListaEventosDoc? listaEventosDoc
            {
                get => listaEventosDocField;
                set => listaEventosDocField = value;
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [XmlType(AnonymousType = true)]
        public partial class returnListaEventosDoc
        {
            private string? codEventoField;

            private string? descEventoField;

            private uint rutResponsableField;

            private string? dvResponsableField;

            private string? fechaEventoField;

            /// <remarks/>
            public string? codEvento
            {
                get => codEventoField;
                set => codEventoField = value;
            }

            /// <remarks/>
            public string? descEvento
            {
                get => descEventoField;
                set => descEventoField = value;
            }

            /// <remarks/>
            public uint rutResponsable
            {
                get => rutResponsableField;
                set => rutResponsableField = value;
            }

            /// <remarks/>
            public string? dvResponsable
            {
                get => dvResponsableField;
                set => dvResponsableField = value;
            }

            /// <remarks/>
            public string? fechaEvento
            {
                get => fechaEventoField;
                set => fechaEventoField = value;
            }
        }
    }

    public class RespIngresarAceptacionReclamoDoc
    {
        // NOTA: El código generado puede requerir, como mínimo, .NET Framework 4.5 o .NET Core/Standard 2.0.
        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [XmlRoot(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {
            private EnvelopeBody? bodyField;

            /// <remarks/>
            public EnvelopeBody Body
            {
                get => bodyField!;
                set => bodyField = value;
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {
            private ingresarAceptacionReclamoDocResponse? ingresarAceptacionReclamoDocResponseField;

            /// <remarks/>
            [XmlElement(Namespace = "http://ws.registroreclamodte.diii.sdi.sii.cl")]
            public ingresarAceptacionReclamoDocResponse? ingresarAceptacionReclamoDocResponse
            {
                get => ingresarAceptacionReclamoDocResponseField;
                set => ingresarAceptacionReclamoDocResponseField = value;
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "http://ws.registroreclamodte.diii.sdi.sii.cl")]
        [XmlRoot(Namespace = "http://ws.registroreclamodte.diii.sdi.sii.cl", IsNullable = false)]
        public partial class ingresarAceptacionReclamoDocResponse
        {
            private @return? returnField;

            /// <remarks/>
            [XmlElement(Namespace = "")]
            public @return @return
            {
                get => returnField!;
                set => returnField = value;
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [XmlType(AnonymousType = true)]
        [XmlRoot(Namespace = "", IsNullable = false)]
        public partial class @return
        {
            private byte codRespField;

            private string? descRespField;

            /// <remarks/>
            public byte codResp
            {
                get => codRespField;
                set => codRespField = value;
            }

            /// <remarks/>
            public string? descResp
            {
                get => descRespField;
                set => descRespField = value;
            }
        }
    }
}
