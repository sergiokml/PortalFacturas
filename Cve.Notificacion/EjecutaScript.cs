using System.Diagnostics;
using System.Reflection;

using Microsoft.Extensions.Configuration;

namespace Cve.Notificacion
{
    public class EjecutaScript
    {
        private readonly IConfiguration config;

        public EjecutaScript(IConfiguration config)
        {
            this.config = config;
        }

        public async Task InsertInstrucciones(string args)
        {
            using Process compiler = new();
            compiler.StartInfo = new ProcessStartInfo(
                Path.Combine(
                    config.GetSection("Scripts:InsertInstrucciones").Value!,
                    "InsertInstrucciones.exe"
                ),
                args
            )
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };
            _ = compiler.Start();
            string rr = await compiler.StandardOutput.ReadToEndAsync();
            await compiler.WaitForExitAsync();
        }
    }
}
