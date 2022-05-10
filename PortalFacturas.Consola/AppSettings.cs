namespace PortalFacturas.Consola
{
    public class AppSettings
    {
        public const string Variables = "VariablesApp";
        public string UserName { get; set; }
        public string Password { get; set; }
        public string GrantType { get; set; }
        public string Scope { get; set; }
        public string Resource { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SiteId { get; set; }
        public string UrlApiCen { get; set; }
        public string UrlApiSharePoint { get; set; }
        public string UrlGraph { get; set; }
        public string SqlServer { get; set; }
        public string SftpServer { get; set; }
    }
}
