using Microsoft.Azure.WebJobs;

using System;

namespace WebJobConvertToPdf
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    internal class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        private static void Main()
        {
            JobHostConfiguration config = new JobHostConfiguration
            {
                DashboardConnectionString = "DefaultEndpointsProtocol=https;AccountName=storageaccountporta82a1;AccountKey=Ro2tiSAq+8qAxSAjBxF+/VPGKj+Ad4QtTkelxZP0Dy0DFLZ32btINQvTLhz5taq707/QNw0gtXHtkRHP8UdGBw=="
            };

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            string URL = "https://storageaccountporta82a1.blob.core.windows.net/wkhtmltopdf/wkhtmltopdf.exe";
            string filename = "filename";
            try
            {
                using (System.Diagnostics.Process p = new System.Diagnostics.Process())
                {
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "wkhtmltopdf.exe",
                        Arguments = URL + " " + filename,
                        UseShellExecute = false
                    };
                    p.StartInfo = startInfo;
                    p.Start();
                    p.WaitForExit();
                    p.Close();
                }
            }
            catch (Exception)
            {
                //    WriteLine($"Something Happened: {ex.Message}");
            }



            JobHost host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }


    }
}
