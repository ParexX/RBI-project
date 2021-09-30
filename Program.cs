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
        /* 
        Description : la methode fournit les arguments des methodes InitializeRunSpace et MultiRunScript de la classe HostedRunSpace,
        a savoir les scripts Powershell à executer, les informations de connexion pour l'authentification et les modules Powershell a importer.  
        */
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
                    {"pwdd","orionrbi092021"}
                };
                var Runspace = new HostedRunspace();
                Runspace.InitializeRunspaces(new string[1] { "SwisPowerShell" });
                
                await Runspace.MultiRunScript(Scripts, ConnexionParameters);
                while (true)
                {
                    /// On reexecute touts les scripts passés en argument toutes les 4 minutes
                    if (DateTime.Now.Second == 0 && DateTime.Now.Minute % 4 == 0)
                        await Runspace.MultiRunScript(Scripts, ConnexionParameters);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /*
        Description : La methode main execute les scripts et construit l'application.
        Entree : Les arguments fournis dans l'entrée standart.
        */
        public static void Main(string[] args)
        {
            RunScript();
            CreateHostBuilder(args).Build().Run();

        }

        // La construction de l'application
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
