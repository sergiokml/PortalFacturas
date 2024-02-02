using Cve.Coordinador.Models;
using Cve.Coordinador.Services.Interfaces;

namespace Cve.Coordinador.Extensions
{
    public static class BillingServiceExtensions
    {
        public static async Task<IEnumerable<BillingWindow>> GetBillingWindowsAsync(
            this IBillingWindowService service,
            IEnumerable<BillingType> billingTypes,
            DateTime period,
            int id,
            CancellationToken token
        )
        {
            List<BillingWindow> result = new();
            List<Task<List<BillingWindow>>> tareas = new();
            tareas = billingTypes
                .Select(async b =>
                {
                    IEnumerable<BillingWindow>? r = await service.GetByCreditor(
                        b.Id,
                        period,
                        id,
                        token
                    );
                    result.AddRange(r!);
                    return result;
                })
                .ToList();
            _ = await Task.WhenAll(tareas);
            return result;
        }
    }
}
