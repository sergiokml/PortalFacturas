using Cve.Coordinador.Models;

namespace Cve.Coordinador.Services.Interfaces
{
    public interface IDteService
    {
        Task<IEnumerable<Dte>> GetManyDebtorAsync(int idPm, int id, CancellationToken token);
        Task<IEnumerable<Dte>> GetManyCreditorAsync(
            int creditor,
            int debtor,
            CancellationToken token
        );
        Task<IEnumerable<Dte>> GetManyInstructionId(int id, CancellationToken token);
    }
}
