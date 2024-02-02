using Cve.Coordinador.Models;
using Refit;

namespace Cve.Coordinador.Infraestructure
{
    public interface IGitHubApi
    {
        [Get("/api/v1/resources/agents/?email={email}")]
        Task<BaseModel<Agent>> GetUser(string email);


        [Post("/api/token-auth/")]
        Task<object> GetToken([Body] object user);


        [Put("/api/v1/resources/auxiliary-files/")]
        Task<object> PutFile([Body] StreamContent content, [Header("Authorization")] string token);
    }

}
