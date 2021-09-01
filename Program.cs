using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazorSupervisionRBI
{
    public class Program
    {
        public static async Task RunScript()
        {
            try
            {
                var pathScripts = "wwwroot/ressources/scripts";
                var Scripts = new string[]{
                File.ReadAllText($"{pathScripts}\\ClearData.ps1",Encoding.UTF8),
                File.ReadAllText($"{pathScripts}\\MigrateApplicationTemplate.ps1",Encoding.UTF8),
                File.ReadAllText($"{pathScripts}\\MigrateNodeTable.ps1",Encoding.UTF8),
                File.ReadAllText($"{pathScripts}\\MigrateTagTable.ps1",Encoding.UTF8),
                File.ReadAllText($"{pathScripts}\\MigrateApplication.ps1",Encoding.UTF8),
                File.ReadAllText($"{pathScripts}\\MigrateComponentTable.ps1",Encoding.UTF8),
                File.ReadAllText($"{pathScripts}\\MigrateHardwareInfo.ps1",Encoding.UTF8),
                File.ReadAllText($"{pathScripts}\\MigrateCategoryTable.ps1",Encoding.UTF8),
                File.ReadAllText($"{pathScripts}\\MigrateHardwareTable.ps1",Encoding.UTF8)
                };


                var ConnexionParameters = new Dictionary<string, object>()
                {
                    {"hostname","Orion.cloudrbi.com"},
                    {"username","adomeon"},
                    {"mdp","Rbi@nto#357"},
                    {"db", "OrionSQL" },
                    {"id","Orion"},
                    {"pwdd","orionrbi"}
                };
                var Runspace = new HostedRunspace();
                Runspace.InitializeRunspaces(new string[1] { "SwisPowerShell" });
                
                await Runspace.MultiRunScript(Scripts, ConnexionParameters);
                while (true)
                {
                    if (DateTime.Now.Second == 0)
                        await Runspace.MultiRunScript(Scripts, ConnexionParameters);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void Main(string[] args)
        {
            //RunScript();
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
