using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;


namespace BlazorSupervisionRBI
{
    public class HostedRunspace
    {
        // Rs : Envronnement d'execution des scripts powershell.
        private Runspace Rs {get;set;}

        /*
        Description : Instancie l'environnement d'execution. On ne declare q'une seule instance car le processeur execute un seul script à la fois.
        */
        public void InitializeRunspaces()
        {
            try{
                Rs = RunspaceFactory.CreateRunspace();
                Rs.Open();
            }
            catch (InvalidRunspaceStateException ex)//Retourne une exception à l'appel de la methode Open
            {
                Console.WriteLine("L'environnemnt d'execution est deja ouvert");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        /*
        Description : Execute les scripts un par un
        Entree :
                stringContents : Le code source des scripts Powershell
                scriptParameters : Les paramètres de connexion aux base de données Orion et SQL.
        */
        public async Task RunScripts(string[] stringContents, Dictionary<string, object> scriptParameters) 
        {
            if (Rs == null)//Verifie si l'environnement d'execution est bien instancié.
            {
                throw new ApplicationException("Runspace must be initialized before calling RunScript().");
            }
            var pipelineObjects = new PSDataCollection<PSObject>();
            var BeginTime = new TimeSpan();
            Console.WriteLine(DateTime.Now.TimeOfDay);
            foreach(string script in stringContents){
                /*
                Le block using appelle la methode Dispose de l'objet PowerShell instancié entre les parenthèses.
                La methode Dispose gere l'allocation de ressource au objet.
                Dans ce contexte, l'objet Powershell ajoute le script et ses paramètres et supprime ces ressources après l'execution du script,
                de sorte à ne pas surcharger inutilement la liste de scripts que l'objet PowerShell doit éxecuter.
                */
                using (PowerShell ps = PowerShell.Create()){
                        ps.Runspace = Rs;           
                        BeginTime = DateTime.Now.TimeOfDay;
                        ps.AddScript(script);// ajoute le script à executer.
                        ps.AddParameters(scriptParameters); // ajoute les paramètres de connexion
                        try {
                            pipelineObjects = await ps.InvokeAsync().ConfigureAwait(false);
                        }// execute le script et attend le resultat.
                        catch(Exception e){Console.WriteLine($"Exception : {e}");}
                        foreach (var item in pipelineObjects){
                            Console.WriteLine(item.BaseObject.ToString());// affiche ce que retourne le script dans le terminal.
                        }
                        //Decompte le temps d'execution du script.
                        Console.WriteLine($"Temps d'execution : {DateTime.Now.TimeOfDay - BeginTime}");
                }
            }   
        }
    }
}
