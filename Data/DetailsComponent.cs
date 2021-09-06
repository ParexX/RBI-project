using System;

namespace BlazorSupervisionRBI
{
    public class DetailsComponent
    {
        public int applicationId { get; set; }
        public string componentName { get; set; }
        public int componentStatus { get; set; }
        public string detailsUrl{get;set;}
    }
}