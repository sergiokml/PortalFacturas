using System.Threading.Tasks;

namespace PortalFacturas.Interfaces
{
    public interface IConvertToPdfService
    {
        Task<byte[]> ConvertToPdf(string content, string filename);
    }
}
