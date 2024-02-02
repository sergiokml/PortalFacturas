using Cve.Coordinador.Models;

namespace Cve.Coordinador.Services.Interfaces
{
    public interface IAgentService
    {
        Task<IEnumerable<Agent>> GetByEmail(string email, CancellationToken ct);
    }
}
