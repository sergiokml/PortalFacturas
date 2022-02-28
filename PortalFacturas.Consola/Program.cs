using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Coordinador.Api;
using Coordinador.Api.Interfaces;
using Coordinador.Api.Models;

using Microsoft.Extensions.DependencyInjection;

namespace PortalFacturas.Consola
{
    internal class Program
    {
        private static IParticipantService ParticipantService { get; set; }
        private static IInstructionService InstructionService { get; set; }

        private static async Task Main(string[] args)
        {
            IServiceProvider serviceProvider = Manager.Configure();
            ParticipantService = serviceProvider.GetService<IParticipantService>();
            InstructionService = serviceProvider.GetService<IInstructionService>();

            IEnumerable<ParticipantResult> res = await ParticipantService.GetParticipantes();
        }
    }
}
