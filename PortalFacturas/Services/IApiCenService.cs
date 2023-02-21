using System.Collections.Generic;
using System.Threading.Tasks;

using PortalFacturas.Models;

namespace PortalFacturas.Services
{
    public interface IApiCenService
    {
        Task<List<ParticipantResult>> GetParticipantsAsync(string username = null);
        Task<string> GetAccessTokenAsync(string username, string password);
        Task<List<InstructionResult>> GetInstructionsAsync(string creditor, string debtor);
        Task GetDocumentos(List<InstructionResult> instructions);
    }
}
