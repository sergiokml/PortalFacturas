using Cve.Coordinador.Models;

namespace Cve.Coordinador.Services.Interfaces
{
    public interface IBillingTypeService
    {
        Task<BillingType>? GetById(string id, CancellationToken ct);
        Task<IEnumerable<BillingType>?> GetByCreditor(
            DateTime period,
            int creditorId,
            CancellationToken ct
        );
    }
}
