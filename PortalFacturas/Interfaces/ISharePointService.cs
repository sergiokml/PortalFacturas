using System.Threading.Tasks;

namespace PortalFacturas.Interfaces
{
    public interface ISharePointService
    {
        Task<byte[]> DownloadConvertedFileAsync(string path, string fileId, string targetFormat);
    }
}
