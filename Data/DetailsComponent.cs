using System;

namespace BlazorSupervisionRBI.Data
{
    /*
        applicationID : Id de l'application
        componentName : Nom du composant
        componentStatus : Etat du composant
        detailsUrl : Redirecion vers le logiciel Solarwinds 
    */
    public class DetailsComponent
    {
        public int applicationId { get; set; }
        public string componentName { get; set; }
        public int componentStatus { get; set; }
        public string detailsUrl{get;set;}
    }
}