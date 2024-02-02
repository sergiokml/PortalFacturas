using Cve.Coordinador.Models;

namespace Cve.Coordinador.Services.Interfaces
{
    public interface IBillingWindowService
    {
        Task<IEnumerable<BillingWindow>?> GetByCreditor(
            int billing_type,
            DateTime period,
            int creditorId,
            CancellationToken ct
        );
    }
}
