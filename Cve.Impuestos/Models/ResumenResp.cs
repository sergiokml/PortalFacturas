using System.Text.Json.Serialization;

namespace Cve.Impuestos.Models
{
    // D: Data
    // MD : MetaData
    public class ResumenResp
    {
        [JsonPropertyName("data")]
        public DResumenResp? Data { get; set; }

        [JsonPropertyName("metaData")]
        public MDResumenResp? MetaData { get; set; }

        [JsonPropertyName("respEstado")]
        public RespEstado? RespEstado { get; set; }
    }

    public class ErrorResumenResp
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }
    }

    public class MDResumenResp
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
        public List<ErrorResumenResp>? Errors { get; set; }

        [JsonPropertyName("page")]
        public object? Page { get; set; }
    }

    public class DResumenResp
    {
        [JsonPropertyName("resumenDte")]
        public List<ResumenDte>? ResumenDte { get; set; }

        [JsonPropertyName("datosAsync")]
        public object? DatosAsync { get; set; }
    }

    public class ResumenDte
    {
        [JsonPropertyName("tipoDoc")]
        public int TipoDoc { get; set; }

        [JsonPropertyName("tipoDocDesc")]
        public string? TipoDocDesc { get; set; }

        [JsonPropertyName("totalDoc")]
        public int TotalDoc { get; set; }

        [JsonPropertyName("mntExento")]
        public int MntExento { get; set; }

        [JsonPropertyName("mntNeto")]
        public int MntNeto { get; set; }

        [JsonPropertyName("mntIVA")]
        public int MntIVA { get; set; }

        [JsonPropertyName("mntTotal")]
        public int MntTotal { get; set; }

        [JsonPropertyName("seccion")]
        public string? Seccion { get; set; }

        [JsonPropertyName("periodo")]
        public string? Periodo { get; set; }

        [JsonPropertyName("rut")]
        public int Rut { get; set; }

        [JsonPropertyName("dv")]
        public string? Dv { get; set; }

        [JsonPropertyName("refNCD")]
        public int RefNCD { get; set; }

        [JsonPropertyName("totalDocNCD")]
        public int TotalDocNCD { get; set; }
    }

    //public class RespEstado
    //{
    //    [JsonPropertyName("codRespuesta")]
    //    public int CodRespuesta { get; set; }

    //    [JsonPropertyName("msgeRespuesta")]
    //    public string? MsgeRespuesta { get; set; }

    //    [JsonPropertyName("codError")]
    //    public object? CodError { get; set; }
    //}
}
