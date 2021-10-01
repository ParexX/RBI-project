using System;

namespace BlazorSupervisionRBI.Data
{
    /*
    hardwareInfoID : Id de l'alerte materiel
    categoryName : Le nom du materiel defectueux
    nodeName : Le nom du serveur
    cdiValdo : Le code client
    csCode : Le code CS
    alertMessage : Le message d'alerte 
    detailsUrl : Le redirection vers le logiciel Solarwind
    */
    public class DysfunctionalHardware
    {
        public int hardwareInfoID {get;set;}
        public string categoryName{get;set;}
        public string nodeName{get;set;}
        public string cdiValdo{get;set;}
        public string csCode{get;set;}
        public string alertMessage{get;set;}
        public string detailsUrl{get;set;}

    }
}