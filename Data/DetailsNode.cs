using System;

namespace BlazorSupervisionRBI.Data {
    /*
    nodeName : Nom du serveur
    codeClient : Code client
    codeCS : Code CS 
    nodeStatus : Etat du serveur
    detailsUrl : Redirecion vers le logiciel Solarwinds
    */
    public class DetailsNode {
        public string nodeName{get;set;}
        public string codeClient{get;set;}
        public string codeCS{get;set;}
        public int nodeStatus{get;set;}
        public string detailsUrl{get;set;}
    }
}