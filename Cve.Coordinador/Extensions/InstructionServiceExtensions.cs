using Cve.Coordinador.Models;
using Cve.Coordinador.Services.Interfaces;

namespace Cve.Coordinador.Extensions
{
    public static class InstructionServiceExtensions
    {
        public static async Task<IEnumerable<Instruction>> GetManyCreditor(
            this IInstructionService service,
            IEnumerable<PaymentMatrix>? paymentsmatrix,
            int idParticipant,
            CancellationToken token
        )
        {
            List<Instruction> result = new();
            List<Task<List<Instruction>>> tareas = new();
            tareas = paymentsmatrix!
                .Select(async b =>
                {
                    IEnumerable<Instruction>? instructions = await service.GetByCreditor(
                        b.Id,
                        idParticipant,
                        token
                    );
                    result.AddRange(instructions!);
                    return result;
                })
                .ToList();
            _ = await Task.WhenAll(tareas);
            return result;
        }

        public static async Task<IEnumerable<Instruction>> GetManyDebtor(
            this IInstructionService service,
            IEnumerable<PaymentMatrix>? paymentsmatrix,
            int idParticipant,
            CancellationToken token
        )
        {
            List<Instruction> result = new();
            List<Task<List<Instruction>>> tareas = new();
            tareas = paymentsmatrix!
                .Select(async b =>
                {
                    IEnumerable<Instruction>? instructions = await service.GetByDebtor(
                        b.Id,
                        idParticipant,
                        token
                    );
                    result.AddRange(instructions!);
                    return result;
                })
                .ToList();
            _ = await Task.WhenAll(tareas);
            return result;
        }
    }
}
