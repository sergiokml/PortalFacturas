using Cve.Coordinador.Models;

namespace Cve.Coordinador.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentMatrix?> GetById(int id, CancellationToken token);
        Task<IEnumerable<PaymentMatrix>?>? GetByDate(DateTime publishdate, CancellationToken token);
        Task<IEnumerable<PaymentMatrix>?>? GetByPeriod(DateTime after, CancellationToken token);
    }
}
