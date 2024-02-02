namespace Cve.Impuestos.Serializadores
{
    public class TimbrajeCaf
    {
        // NOTA: El código generado puede requerir, como mínimo, .NET Framework 4.5 o .NET Core/Standard 2.0.
        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true)]
        [System.Xml.Serialization.XmlRoot(Namespace = "", IsNullable = false)]
        public partial class AUTORIZACION
        {
            private AUTORIZACIONCAF? cAFField;

            private string? rSASKField;

            private string? rSAPUBKField;

            /// <remarks/>
            public AUTORIZACIONCAF CAF
            {
                get => cAFField!;
                set => cAFField = value;
            }

            /// <remarks/>
            public string RSASK
            {
                get => rSASKField!;
                set => rSASKField = value;
            }

            /// <remarks/>
            public string RSAPUBK
            {
                get => rSAPUBKField!;
                set => rSAPUBKField = value;
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true)]
        public partial class AUTORIZACIONCAF
        {
            private AUTORIZACIONCAFDA? daField;

            private AUTORIZACIONCAFFRMA? fRMAField;

            private decimal versionField;

            /// <remarks/>
            public AUTORIZACIONCAFDA DA
            {
                get => daField!;
                set => daField = value;
            }

            /// <remarks/>
            public AUTORIZACIONCAFFRMA FRMA
            {
                get => fRMAField!;
                set => fRMAField = value;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttribute()]
            public decimal version
            {
                get => versionField;
                set => versionField = value;
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true)]
        public partial class AUTORIZACIONCAFDA
        {
            private string? reField;

            private string? rsField;

            private byte tdField;

            private AUTORIZACIONCAFDARNG? rNGField;

            private string? faField;

            private AUTORIZACIONCAFDARSAPK? rSAPKField;

            private ushort iDKField;

            /// <remarks/>
            public string RE
            {
                get => reField!;
                set => reField = value;
            }

            /// <remarks/>
            public string RS
            {
                get => rsField!;
                set => rsField = value;
            }

            /// <remarks/>
            public byte TD
            {
                get => tdField;
                set => tdField = value;
            }

            /// <remarks/>
            public AUTORIZACIONCAFDARNG RNG
            {
                get => rNGField!;
                set => rNGField = value;
            }

            /// <remarks/>
            //[System.Xml.Serialization.XmlElement(DataType = "date")]
            public string? FA
            {
                get => faField;
                set => faField = value;
            }

            /// <remarks/>
            public AUTORIZACIONCAFDARSAPK RSAPK
            {
                get => rSAPKField!;
                set => rSAPKField = value;
            }

            /// <remarks/>
            public ushort IDK
            {
                get => iDKField;
                set => iDKField = value;
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true)]
        public partial class AUTORIZACIONCAFDARNG
        {
            private ushort dField;

            private ushort hField;

            /// <remarks/>
            public ushort D
            {
                get => dField;
                set => dField = value;
            }

            /// <remarks/>
            public ushort H
            {
                get => hField;
                set => hField = value;
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true)]
        public partial class AUTORIZACIONCAFDARSAPK
        {
            private string? mField;

            private string? eField;

            /// <remarks/>
            public string M
            {
                get => mField!;
                set => mField = value;
            }

            /// <remarks/>
            public string E
            {
                get => eField!;
                set => eField = value;
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true)]
        public partial class AUTORIZACIONCAFFRMA
        {
            private string? algoritmoField;

            private string? valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttribute()]
            public string algoritmo
            {
                get => algoritmoField!;
                set => algoritmoField = value;
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlText()]
            public string Value
            {
                get => valueField!;
                set => valueField = value;
            }
        }
    }
}
