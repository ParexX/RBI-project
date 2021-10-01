using System;

namespace BlazorSupervisionRBI.Data {
    /*
        nodeName : Nom du serveur
        clientName : Nom du client
        csCode : Code CS
        nodeStatus : Etat du serveur
        applicationName : Nom de l'application
        applicationID : Id de l'application
        detailsUrl : Redirecion vers le logiciel Solarwind
    */
    public class DetailsAPM {
        public string nodeName{get;set;}
        public string clientName{get;set;}
        public string csCode{get;set;}
        public int nodeStatus{get;set;}
        public string applicationName{get;set;}
        public int applicationID{get;set;}
        public string detailsUrl{get;set;}
    }
}