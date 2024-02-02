using Cve.Coordinador.Models;

namespace Cve.Coordinador.Services.Interfaces
{
    public interface IParticipantService
    {
        Task<IEnumerable<Participant>?> GetById(int[] ids, CancellationToken ct);
        Task<Participant?> GetById(int id, CancellationToken ct);
        Task<Participant?> GetByRut(string rut, CancellationToken ct);
        Task<IEnumerable<Participant>?> GetAll(CancellationToken ct);
    }
}
