using System;
using System.Collections.Generic;

namespace BlazorSupervisionRBI
{
    public static class CommonClass {
        public static Dictionary<int, List<string>> statusLayout = new Dictionary<int, List<string>>(){
    {0,new List<string>{"unknown","lighblue"}},
    {1, new List<string>{"up","limegreen"}},
    {2,new List<string>{"down","orange"}},
    {3,new List<string>{"warning","gold"}},
    {12, new List<string>{"unreachable","mediumblue"}},
    {14, new List<string>{"critical","red"}},
    {27, new List<string>{"disabled","grey"}}
    };
        public static string solarWindsLink = "http://supervision.cloudrbi.com";
    }
}