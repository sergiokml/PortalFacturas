using System.Text.Json.Serialization;

namespace Cve.Impuestos.Models
{
    public class RegValidaDteResp
    {
        [JsonPropertyName("data")]
        public DataRegValidaDteRespModel? Data { get; set; }

        [JsonPropertyName("metaData")]
        public MetaDataRegValidaDteRespModel? MetaData { get; set; }

        public RegValidaDteResp(
            DataRegValidaDteRespModel? data,
            MetaDataRegValidaDteRespModel? metaData
        )
        {
            Data = data;
            MetaData = metaData;
        }
    }

    public class DataRegValidaDteRespModel
    {
        [JsonPropertyName("dhdrCodigo")]
        public long DhdrCodigo { get; set; }

        [JsonPropertyName("dhdrRutEmisor")]
        public int DhdrRutEmisor { get; set; }

        [JsonPropertyName("dhdrDvEmisor")]
        public string? DhdrDvEmisor { get; set; }

        [JsonPropertyName("rzEmisor")]
        public string? RzEmisor { get; set; }

        [JsonPropertyName("dtdcCodigo")]
        public int DtdcCodigo { get; set; }

        [JsonPropertyName("descDoc")]
        public string? DescDoc { get; set; }

        [JsonPropertyName("dhdrFolio")]
        public int DhdrFolio { get; set; }

        [JsonPropertyName("dhdrRutRecep")]
        public int DhdrRutRecep { get; set; }

        [JsonPropertyName("dhdrDvRecep")]
        public string? DhdrDvRecep { get; set; }

        [JsonPropertyName("rzReceptor")]
        public string? RzReceptor { get; set; }

        [JsonPropertyName("dhdrFchEmis")]
        public string? DhdrFchEmis { get; set; }

        [JsonPropertyName("dhdrMntTotal")]
        public int DhdrMntTotal { get; set; }

        [JsonPropertyName("dhdrIva")]
        public int DhdrIva { get; set; }

        [JsonPropertyName("dtecTmstRecep")]
        public string? DtecTmstRecep { get; set; }

        [JsonPropertyName("diferenciaFecha")]
        public int DiferenciaFecha { get; set; }

        [JsonPropertyName("tieneAccesoReceptor")]
        public bool TieneAccesoReceptor { get; set; }

        [JsonPropertyName("tieneAccesoEmisor")]
        public bool TieneAccesoEmisor { get; set; }

        [JsonPropertyName("tieneAccesoTenedorVig")]
        public bool TieneAccesoTenedorVig { get; set; }

        [JsonPropertyName("pagadoAlContado")]
        public bool PagadoAlContado { get; set; }

        [JsonPropertyName("tieneReclamos")]
        public bool TieneReclamos { get; set; }

        [JsonPropertyName("mayorOchoDias")]
        public bool MayorOchoDias { get; set; }

        [JsonPropertyName("tieneAcuses")]
        public bool TieneAcuses { get; set; }

        [JsonPropertyName("tieneReferenciaGuia")]
        public object? TieneReferenciaGuia { get; set; }

        [JsonPropertyName("msgDteCedible")]
        public object? MsgDteCedible { get; set; }

        [JsonPropertyName("listEvenHistDoc")]
        public List<ListEvenHistDoc>? ListEvenHistDoc { get; set; }
    }

    public class ListEvenHistDoc
    {
        [JsonPropertyName("codigoDoc")]
        public object? CodigoDoc { get; set; }

        [JsonPropertyName("descEvento")]
        public string? DescEvento { get; set; }

        [JsonPropertyName("codEvento")]
        public string? CodEvento { get; set; }

        [JsonPropertyName("fechaEvento")]
        public string? FechaEvento { get; set; }

        [JsonPropertyName("rutResponsable")]
        public int RutResponsable { get; set; }

        [JsonPropertyName("dvResponsable")]
        public string? DvResponsable { get; set; }

        [JsonPropertyName("direccionIp")]
        public object? DireccionIp { get; set; }

        [JsonPropertyName("rutEmisor")]
        public object? RutEmisor { get; set; }

        [JsonPropertyName("dvEmisor")]
        public object? DvEmisor { get; set; }

        [JsonPropertyName("rutReceptor")]
        public object? RutReceptor { get; set; }

        [JsonPropertyName("dvReceptor")]
        public object? DvReceptor { get; set; }
    }

    public class MetaDataRegValidaDteRespModel
    {
        [JsonPropertyName("conversationId")]
        public object? ConversationId { get; set; }

        [JsonPropertyName("transactionId")]
        public object? TransactionId { get; set; }

        [JsonPropertyName("namespace")]
        public object? Namespace { get; set; }

        [JsonPropertyName("info")]
        public object? Info { get; set; }

        [JsonPropertyName("errors")]
        public object? Errors { get; set; }

        [JsonPropertyName("page")]
        public object? Page { get; set; }
    }
}
