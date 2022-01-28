using System.Threading.Tasks;

namespace PortalFacturas.Interfaces
{
    public interface IXsltHelper
    {
        Task<byte[]> TransformAsync(byte[] inputXml);
        Task<IXsltHelper> AddParam(byte[] inputXml);
        Task<IXsltHelper> LoadXslAsync();
    }
}
