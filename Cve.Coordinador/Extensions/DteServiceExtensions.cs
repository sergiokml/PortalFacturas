using Cve.Coordinador.Models;
using Cve.Coordinador.Services.Interfaces;

namespace Cve.Coordinador.Extensions;

public static class DteServiceExtensions
{
    public static async Task<IEnumerable<Dte>> GetManyDebtorAsync(
        this IDteService service,
        IEnumerable<PaymentMatrix>? paymentsmatrix,
        int idParticipant,
        CancellationToken token
    )
    {
        List<Dte> result = new();
        List<Task<List<Dte>>> tareas = new();
        tareas = paymentsmatrix!
            .Select(async b =>
            {
                IEnumerable<Dte>? instructions = await service.GetManyDebtorAsync(
                    b.Id,
                    idParticipant,
                    token
                );
                result.AddRange(instructions);
                return result;
            })
            .ToList();
        _ = await Task.WhenAll(tareas);
        return result;
    }

    public static async Task GetDocumentos(
        this IDteService service,
        IEnumerable<Instruction> instructions,
        CancellationToken token
    )
    {
        List<Task> tareas = new();
        tareas = instructions
            .Select(async m =>
            {
                var dte = await service.GetManyInstructionId(m.Id, token);

                if (dte != null && dte.Any())
                {
                    foreach (Dte item in dte)
                    {
                        if (item.Type == 2) // 61 NC
                        {
                            item.NetAmount *= -1;
                        }
                    }
                    m.DteAsociados = dte.ToList();
                }
            })
            .ToList();
        await Task.WhenAll(tareas);
    }
}
