using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PortalFacturas.Models
{
    public class Mapper
    {
        [JsonPropertyName("directory")]
        public string Directory { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class ExtensionObject
    {
        [JsonPropertyName("directory")]
        public string Directory { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("namespace")]
        public string Namespace { get; set; }

        [JsonPropertyName("assemblyName")]
        public string AssemblyName { get; set; }

        [JsonPropertyName("className")]
        public string ClassName { get; set; }
    }

    public class UploadModel
    {
        [JsonPropertyName("inputXml")]
        public string InputXml { get; set; }

        [JsonPropertyName("mapper")]
        public Mapper Mapper { get; set; } = new Mapper();

        [JsonPropertyName("extensionObjects")]
        public List<ExtensionObject> ExtensionObjects { get; set; } = new List<ExtensionObject>();

    }
}
