using Cve.Coordinador.Models;

namespace Cve.Coordinador.Services.Interfaces
{
    public interface IPeriodService
    {
        Task<IEnumerable<Period>?> GetMany(object id, CancellationToken token);
    }
}
