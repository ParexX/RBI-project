using System;

namespace BlazorSupervisionRBI.Data
{
    /*
    severity : Un niveau de danger ou d'incidence (critique, avertissement...) 
    countSeverity : Le nombre d'occurence des objects par niveau de danger.
    */
    public class Overview
    {
        public int severity{get; set;}

        public int countSeverity{get; set;} 
    }
}