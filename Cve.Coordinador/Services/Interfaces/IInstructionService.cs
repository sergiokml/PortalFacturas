using Cve.Coordinador.Models;

namespace Cve.Coordinador.Services.Interfaces
{
    public interface IInstructionService
    {
        Task<IEnumerable<Instruction>?> GetByCreditor(
            int pmId,
            int creditorId,
            CancellationToken ct
        );
        Task<IEnumerable<Instruction>?> GetByDebtor(int pmId, int debtorId, CancellationToken ct);
        Task<IEnumerable<Instruction>?> GetByDebtor(
            string[] creditors,
            string debtorId,
            CancellationToken ct
        );
        Task<IEnumerable<Instruction>?> GetByCreditor(
            string[] debtors,
            string creditorId,
            CancellationToken ct
        );
        Task<Instruction?> GetById(int id, CancellationToken ct);
        Task<IEnumerable<Instruction>?> GetById(
            string creditorId,
            string debtorId,
            CancellationToken ct
        );
        Task<int> GetByCreditor(int creditorId, CancellationToken ct);
        Task<IEnumerable<Instruction>?> GetByCreditor(
            DateTime period,
            string creditorId,
            CancellationToken ct
        );
        Task<IEnumerable<Instruction>?> GetByDebtor(
            DateTime period,
            string debtorId,
            CancellationToken ct
        );
    }
}
