using System.Text.Json.Serialization;

namespace Cve.Impuestos.Models
{
    // D: Data
    // MD : MetaData
    internal class DetalleReq
    {
        [JsonPropertyName("metaData")]
        public MetaData? MetaData { get; set; }

        [JsonPropertyName("data")]
        public Data? Data { get; set; }

        public DetalleReq(MetaData? metaData, Data? data)
        {
            MetaData = metaData;
            Data = data;
        }
    }

    public class DDetalleReq
    {
        [JsonPropertyName("tipoDoc")]
        public string? TipoDoc { get; set; }

        [JsonPropertyName("rut")]
        public string? Rut { get; set; }

        [JsonPropertyName("dv")]
        public string? Dv { get; set; }

        [JsonPropertyName("periodo")]
        public string? Periodo { get; set; }

        [JsonPropertyName("operacion")]
        public int Operacion { get; set; }

        [JsonPropertyName("derrCodigo")]
        public string? DerrCodigo { get; set; }

        [JsonPropertyName("refNCD")]
        public string? RefNCD { get; set; }
    }

    public class MDDetalleReq
    {
        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }

        [JsonPropertyName("conversationId")]
        public string? ConversationId { get; set; }

        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; set; }

        [JsonPropertyName("page")]
        public object? Page { get; set; }
    }

    // REQ DETALLE
    public class ReqDetalle
    {
        public ReqDetalle(MetaData? metaData, Data? data)
        {
            MetaData = metaData;
            Data = data;
        }

        [JsonPropertyName("metaData")]
        public MetaData? MetaData { get; set; }

        [JsonPropertyName("data")]
        public Data? Data { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("rutEmisor")]
        public string? RutEmisor { get; set; }

        [JsonPropertyName("dvEmisor")]
        public string? DvEmisor { get; set; }

        [JsonPropertyName("ptributario")]
        public string? Ptributario { get; set; }

        [JsonPropertyName("codTipoDoc")]
        public string? CodTipoDoc { get; set; }

        [JsonPropertyName("operacion")]
        public string? Operacion { get; set; }

        [JsonPropertyName("estadoContab")]
        public string? EstadoContab { get; set; }
    }

    public class MetaData
    {
        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }

        [JsonPropertyName("conversationId")]
        public string? ConversationId { get; set; }

        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; set; }

        [JsonPropertyName("page")]
        public object? Page { get; set; }
    }

    // RES DETALLE
    public class ResDetalle
    {
        [JsonPropertyName("data")]
        public List<Datum>? Data { get; set; }

        [JsonPropertyName("esDocPapel")]
        public bool EsDocPapel { get; set; }

        [JsonPropertyName("dataCabecera")]
        public object? DataCabecera { get; set; }

        [JsonPropertyName("metaData")]
        public MetaDatum? MetaData { get; set; }

        [JsonPropertyName("respEstado")]
        public RespEstado? RespEstado { get; set; }
    }

    public class RespEstado
    {
        [JsonPropertyName("codRespuesta")]
        public int CodRespuesta { get; set; }

        [JsonPropertyName("msgeRespuesta")]
        public string? MsgeRespuesta { get; set; }

        [JsonPropertyName("codError")]
        public string? CodError { get; set; }
    }

    public class MetaDatum
    {
        [JsonPropertyName("conversationId")]
        public string? ConversationId { get; set; }

        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; set; }

        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }

        [JsonPropertyName("info")]
        public object? Info { get; set; }

        [JsonPropertyName("errors")]
        public List<Error>? Errors { get; set; }

        [JsonPropertyName("page")]
        public object? Page { get; set; }
    }

    public class Error
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }
    }

    public class Datum
    {
        [JsonPropertyName("dhdrCodigo")]
        public long? DhdrCodigo { get; set; }

        [JsonPropertyName("dcvCodigo")]
        public int? DcvCodigo { get; set; }

        [JsonPropertyName("dcvEstadoContab")]
        public object? DcvEstadoContab { get; set; }

        [JsonPropertyName("detCodigo")]
        public long? DetCodigo { get; set; }

        [JsonPropertyName("detTipoDoc")]
        public int? DetTipoDoc { get; set; }

        [JsonPropertyName("detRutDoc")]
        public int? DetRutDoc { get; set; }

        [JsonPropertyName("detDvDoc")]
        public string? DetDvDoc { get; set; }

        [JsonPropertyName("detRznSoc")]
        public string? DetRznSoc { get; set; }

        [JsonPropertyName("detNroDoc")]
        public long? DetNroDoc { get; set; }

        [JsonPropertyName("detFchDoc")]
        public string? DetFchDoc { get; set; }

        [JsonPropertyName("detFecAcuse")]
        public string? DetFecAcuse { get; set; }

        [JsonPropertyName("detFecReclamado")]
        public string? DetFecReclamado { get; set; }

        [JsonPropertyName("detFecRecepcion")]
        public string? DetFecRecepcion { get; set; }

        [JsonPropertyName("detMntExe")]
        public long? DetMntExe { get; set; }

        [JsonPropertyName("detMntNeto")]
        public long? DetMntNeto { get; set; }

        [JsonPropertyName("detMntActFijo")]
        public long? DetMntActFijo { get; set; }

        [JsonPropertyName("detMntIVAActFijo")]
        public long? DetMntIVAActFijo { get; set; }

        [JsonPropertyName("detMntIVANoRec")]
        public long? DetMntIVANoRec { get; set; }

        [JsonPropertyName("detMntCodNoRec")]
        public long? DetMntCodNoRec { get; set; }

        [JsonPropertyName("detMntSinCredito")]
        public long? DetMntSinCredito { get; set; }

        [JsonPropertyName("detMntIVA")]
        public long? DetMntIVA { get; set; }

        [JsonPropertyName("detMntTotal")]
        public long? DetMntTotal { get; set; }

        [JsonPropertyName("detTasaImp")]
        public string? DetTasaImp { get; set; }

        [JsonPropertyName("detAnulado")]
        public object? DetAnulado { get; set; }

        [JsonPropertyName("detIVARetTotal")]
        public int? DetIVARetTotal { get; set; }

        [JsonPropertyName("detIVARetParcial")]
        public int? DetIVARetParcial { get; set; }

        [JsonPropertyName("detIVANoRetenido")]
        public int? DetIVANoRetenido { get; set; }

        [JsonPropertyName("detIVAPropio")]
        public int? DetIVAPropio { get; set; }

        [JsonPropertyName("detIVATerceros")]
        public int? DetIVATerceros { get; set; }

        [JsonPropertyName("detIVAUsoComun")]
        public int? DetIVAUsoComun { get; set; }

        [JsonPropertyName("detLiqRutEmisor")]
        public int? DetLiqRutEmisor { get; set; }

        [JsonPropertyName("detLiqDvEmisor")]
        public object? DetLiqDvEmisor { get; set; }

        [JsonPropertyName("detLiqValComNeto")]
        public int? DetLiqValComNeto { get; set; }

        [JsonPropertyName("detLiqValComExe")]
        public int? DetLiqValComExe { get; set; }

        [JsonPropertyName("detLiqValComIVA")]
        public int? DetLiqValComIVA { get; set; }

        [JsonPropertyName("detIVAFueraPlazo")]
        public int? DetIVAFueraPlazo { get; set; }

        [JsonPropertyName("detTipoDocRef")]
        public int? DetTipoDocRef { get; set; }

        [JsonPropertyName("detFolioDocRef")]
        public string? DetFolioDocRef { get; set; }

        [JsonPropertyName("detExpNumId")]
        public object? DetExpNumId { get; set; }

        [JsonPropertyName("detExpNacionalidad")]
        public object? DetExpNacionalidad { get; set; }

        [JsonPropertyName("detCredEc")]
        public int? DetCredEc { get; set; }

        [JsonPropertyName("detLey18211")]
        public int? DetLey18211 { get; set; }

        [JsonPropertyName("detDepEnvase")]
        public int? DetDepEnvase { get; set; }

        [JsonPropertyName("detIndSinCosto")]
        public int? DetIndSinCosto { get; set; }

        [JsonPropertyName("detIndServicio")]
        public int? DetIndServicio { get; set; }

        [JsonPropertyName("detMntNoFact")]
        public int? DetMntNoFact { get; set; }

        [JsonPropertyName("detMntPeriodo")]
        public int? DetMntPeriodo { get; set; }

        [JsonPropertyName("detPsjNac")]
        public int? DetPsjNac { get; set; }

        [JsonPropertyName("detPsjInt")]
        public int? DetPsjInt { get; set; }

        [JsonPropertyName("detNumInt")]
        public object? DetNumInt { get; set; }

        [JsonPropertyName("detCdgSIISucur")]
        public long? DetCdgSIISucur { get; set; }

        [JsonPropertyName("detEmisorNota")]
        public int? DetEmisorNota { get; set; }

        [JsonPropertyName("detTabPuros")]
        public int? DetTabPuros { get; set; }

        [JsonPropertyName("detTabCigarrillos")]
        public int? DetTabCigarrillos { get; set; }

        [JsonPropertyName("detTabElaborado")]
        public int? DetTabElaborado { get; set; }

        [JsonPropertyName("detImpVehiculo")]
        public int? DetImpVehiculo { get; set; }

        [JsonPropertyName("detTpoImp")]
        public int? DetTpoImp { get; set; }

        [JsonPropertyName("detTipoTransaccion")]
        public int? DetTipoTransaccion { get; set; }

        [JsonPropertyName("detEventoReceptor")]
        public string? DetEventoReceptor { get; set; }

        [JsonPropertyName("detEventoReceptorLeyenda")]
        public string? DetEventoReceptorLeyenda { get; set; }

        [JsonPropertyName("cambiarTipoTran")]
        public bool CambiarTipoTran { get; set; }

        [JsonPropertyName("detPcarga")]
        public int? DetPcarga { get; set; }

        [JsonPropertyName("descTipoTransaccion")]
        public string? DescTipoTransaccion { get; set; }

        [JsonPropertyName("totalDtoiMontoImp")]
        public int? TotalDtoiMontoImp { get; set; }

        [JsonPropertyName("totalDinrMontoIVANoR")]
        public object? TotalDinrMontoIVANoR { get; set; }

        [JsonPropertyName("emisorAgresivo")]
        public bool EmisorAgresivo { get; set; }

        [JsonPropertyName("fechaActivacionAnotacion")]
        public object? FechaActivacionAnotacion { get; set; }
    }

    // RES RESUMEN
    public class ResResumen
    {
        [JsonPropertyName("data")]
        public List<DatumResumen>? Data { get; set; }

        [JsonPropertyName("totDocRes")]
        public int? TotDocRes { get; set; }

        [JsonPropertyName("tieneEmisorAgresivo")]
        public bool TieneEmisorAgresivo { get; set; }

        [JsonPropertyName("verF29")]
        public bool VerF29 { get; set; }

        [JsonPropertyName("dataCabecera")]
        public DataCabecera? DataCabecera { get; set; }

        [JsonPropertyName("metaData")]
        public MetaData? MetaData { get; set; }

        [JsonPropertyName("respEstado")]
        public RespEstado? RespEstado { get; set; }
    }

    public class DataCabecera
    {
        [JsonPropertyName("dcvPcarga")]
        public int? DcvPcarga { get; set; }

        [JsonPropertyName("dcvRutEmisor")]
        public int? DcvRutEmisor { get; set; }

        [JsonPropertyName("dcvDvEmisor")]
        public string? DcvDvEmisor { get; set; }

        [JsonPropertyName("dcvPtributario")]
        public int? DcvPtributario { get; set; }

        [JsonPropertyName("dcvOperacion")]
        public string? DcvOperacion { get; set; }

        [JsonPropertyName("dcvCodigo")]
        public int? DcvCodigo { get; set; }

        [JsonPropertyName("dcvFctProp")]
        public string? DcvFctProp { get; set; }

        [JsonPropertyName("dcvVigente")]
        public object? DcvVigente { get; set; }

        [JsonPropertyName("dcvFecCreacion")]
        public string? DcvFecCreacion { get; set; }

        [JsonPropertyName("dcvFecModificacion")]
        public string? DcvFecModificacion { get; set; }

        [JsonPropertyName("dcvNroCruceHadoop")]
        public object? DcvNroCruceHadoop { get; set; }

        [JsonPropertyName("dcvFchUltimoCruceHadoop")]
        public object? DcvFchUltimoCruceHadoop { get; set; }
    }

    public class DatumResumen
    {
        [JsonPropertyName("dcvCodigo")]
        public int? DcvCodigo { get; set; }

        [JsonPropertyName("rsmnCodigo")]
        public int? RsmnCodigo { get; set; }

        [JsonPropertyName("rsmnTipoDocInteger")]
        public int? RsmnTipoDocInteger { get; set; }

        [JsonPropertyName("dcvNombreTipoDoc")]
        public string? DcvNombreTipoDoc { get; set; }

        [JsonPropertyName("dcvTipoIngresoDoc")]
        public string? DcvTipoIngresoDoc { get; set; }

        [JsonPropertyName("rsmnLink")]
        public bool RsmnLink { get; set; }

        [JsonPropertyName("rsmnMntExe")]
        public long? RsmnMntExe { get; set; }

        [JsonPropertyName("rsmnMntNeto")]
        public long? RsmnMntNeto { get; set; }

        [JsonPropertyName("rsmnMntIVA")]
        public long? RsmnMntIVA { get; set; }

        [JsonPropertyName("rsmnMntIVANoRec")]
        public int? RsmnMntIVANoRec { get; set; }

        [JsonPropertyName("rsmnIVAUsoComun")]
        public int? RsmnIVAUsoComun { get; set; }

        [JsonPropertyName("dcvOperacion")]
        public object? DcvOperacion { get; set; }

        [JsonPropertyName("rsmnMntTotal")]
        public long? RsmnMntTotal { get; set; }

        [JsonPropertyName("rsmnEstadoContab")]
        public object? RsmnEstadoContab { get; set; }

        [JsonPropertyName("rsmnTotDoc")]
        public int? RsmnTotDoc { get; set; }

        [JsonPropertyName("rsmnTotalRutEmisor")]
        public object? RsmnTotalRutEmisor { get; set; }
    }
}
