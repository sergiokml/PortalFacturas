namespace Cve.Impuestos.Serializadores
{
    // NOTA: El código generado puede requerir, como mínimo, .NET Framework 4.5 o .NET Core/Standard 2.0.
    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(
        AnonymousType = true,
        Namespace = "http://www.sii.cl/XMLSchema"
    )]
    [System.Xml.Serialization.XmlRoot(
        Namespace = "http://www.sii.cl/XMLSchema",
        IsNullable = false
    )]
    public class RESPUESTA
    {
        private RESPUESTARESP_BODY? rESP_BODYField;

        private RESPUESTARESP_HDR? rESP_HDRField;

        /// <remarks/>
        public RESPUESTARESP_BODY RESP_BODY
        {
            get => rESP_BODYField!;
            set => rESP_BODYField = value;
        }

        /// <remarks/>
        public RESPUESTARESP_HDR RESP_HDR
        {
            get => rESP_HDRField!;
            set => rESP_HDRField = value;
        }
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(
        AnonymousType = true,
        Namespace = "http://www.sii.cl/XMLSchema"
    )]
    public partial class RESPUESTARESP_BODY
    {
        private ulong sEMILLAField;
        private string? rECIBIDOField;

        private string? eSTADOField;

        private string? gLOSAField;

        private ulong tRACKIDField;

        private string? nUMATENCIONField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "")]
        public string RECIBIDO
        {
            get => rECIBIDOField!;
            set => rECIBIDOField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "")]
        public string ESTADO
        {
            get => eSTADOField!;
            set => eSTADOField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "")]
        public string GLOSA
        {
            get => gLOSAField!;
            set => gLOSAField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "")]
        public ulong TRACKID
        {
            get => tRACKIDField;
            set => tRACKIDField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "")]
        public string NUMATENCION
        {
            get => nUMATENCIONField!;
            set => nUMATENCIONField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "")]
        public ulong SEMILLA
        {
            get => sEMILLAField;
            set => sEMILLAField = value;
        }
        private string? tOKENField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "")]
        public string TOKEN
        {
            get => tOKENField!;
            set => tOKENField = value;
        }
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(
        AnonymousType = true,
        Namespace = "http://www.sii.cl/XMLSchema"
    )]
    public partial class RESPUESTARESP_HDR
    {
        private string? eSTADOField;

        private string? gLOSA_ESTADOField;

        private byte eRR_CODEField;

        private string? gLOSA_ERRField;

        private object? nUM_ATENCIONField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "")]
        public string? ESTADO
        {
            get => eSTADOField;
            set => eSTADOField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "")]
        public string? GLOSA_ESTADO
        {
            get => gLOSA_ESTADOField;
            set => gLOSA_ESTADOField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "")]
        public byte ERR_CODE
        {
            get => eRR_CODEField;
            set => eRR_CODEField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "")]
        public string? GLOSA_ERR
        {
            get => gLOSA_ERRField;
            set => gLOSA_ERRField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "")]
        public object? NUM_ATENCION
        {
            get => nUM_ATENCIONField;
            set => nUM_ATENCIONField = value;
        }
    }
}
