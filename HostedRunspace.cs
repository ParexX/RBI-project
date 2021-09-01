using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;


namespace BlazorSupervisionRBI
{
    /// <summary>
    /// Contains functionality for executing PowerShell scripts.
    /// </summary>
    public class HostedRunspace
    {
        private Runspace Rs {get;set;}
        /// <summary>
        /// Runs a PowerShell script with parameters and prints the resulting pipeline objects to the console output. 
        /// </summary>
        /// <param name="scriptContents">The script file contents.</param>
        /// <param name="scriptParameters">A dictionary of parameter names and parameter values.</param>

        public void InitializeRunspaces(string[] modulesToLoad)
        {
            // create the default session state.
            // session state can be used to set things like execution policy, language constraints, etc.
            // optionally load any modules (by name) that were supplied.

            var defaultSessionState = InitialSessionState.CreateDefault();
            defaultSessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;
            
            foreach (var moduleName in modulesToLoad)
            {
                defaultSessionState.ImportPSModule(moduleName);
            }
            Rs = RunspaceFactory.CreateRunspace(defaultSessionState);
            Rs.Open();
        }
    
        public async Task MultiRunScript(string[] stringContents, Dictionary<string, object> scriptParameters) 
        {
            // create a new hosted PowerShell instance using the default runspace.
            // wrap in a using statement to ensure resources are cleaned up.
            if (Rs == null)
            {
                throw new ApplicationException("Runspace must be initialized before calling RunScript().");
            }
            var pipelineObjects = new PSDataCollection<PSObject>();
            var BeginTime = new TimeSpan();
            Console.WriteLine(DateTime.Now.TimeOfDay);
            foreach(string script in stringContents){
                using (PowerShell ps = PowerShell.Create()){
                        ps.Runspace = Rs;           
                        BeginTime = DateTime.Now.TimeOfDay;
                        ps.AddScript(script);// specify the script code to run.
                        ps.AddParameters(scriptParameters); // specify the parameters to pass into the script
                        try {
                            pipelineObjects = await ps.InvokeAsync().ConfigureAwait(false);
                        }// execute the script and await the result
                        catch(Exception e){Console.WriteLine($"Exception : {e}");}
                        foreach (var item in pipelineObjects){
                            Console.WriteLine(item.BaseObject.ToString());// print the resulting pipeline objects to the console.
                        }
                        Console.WriteLine($"Temps d'execution : {DateTime.Now.TimeOfDay - BeginTime}");
                }
            }   
        }
    }
}
