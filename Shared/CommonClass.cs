using System;
using System.Collections.Generic;

namespace BlazorSupervisionRBI.Shared
{
    public static class CommonClass
    {
        public static Dictionary<int, List<string>> statusLayout = new Dictionary<int, List<string>>(){
    {0,new List<string>{"inconnue","black"}},
    {1, new List<string>{"operationnel","limegreen"}},
    {2,new List<string>{"non operationnel","darkorange"}},
    {3,new List<string>{"en avertissement","gold"}},
    {12, new List<string>{"injoignable","mediumblue"}},
    {14, new List<string>{"critique","red"}},
    {27, new List<string>{"desactivé","grey"}}
    };
        public static string solarWindsLink = "http://supervision.cloudrbi.com";
        public static Dictionary<string, string> credentials = new Dictionary<string, string>(){
            {"server","SRVJIRA\\SQLJIRA"},
            {"database","OrionSQL"},
            {"user","Orion"},
            {"pwd","orionrbi092021"},
        };
    }
}