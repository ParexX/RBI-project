// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace BlazorSupervisionRBI.Pages
{
    #line hidden
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\ADOMEON\BlazorSupervisionRBI\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\ADOMEON\BlazorSupervisionRBI\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\ADOMEON\BlazorSupervisionRBI\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\ADOMEON\BlazorSupervisionRBI\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\ADOMEON\BlazorSupervisionRBI\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\ADOMEON\BlazorSupervisionRBI\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\ADOMEON\BlazorSupervisionRBI\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\ADOMEON\BlazorSupervisionRBI\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\ADOMEON\BlazorSupervisionRBI\_Imports.razor"
using BlazorSupervisionRBI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\ADOMEON\BlazorSupervisionRBI\_Imports.razor"
using BlazorSupervisionRBI.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "C:\Users\ADOMEON\BlazorSupervisionRBI\_Imports.razor"
using System;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\Users\ADOMEON\BlazorSupervisionRBI\_Imports.razor"
using System.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "C:\Users\ADOMEON\BlazorSupervisionRBI\_Imports.razor"
using System.Data.SqlClient;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\ADOMEON\BlazorSupervisionRBI\Pages\Fetch_App.razor"
using BlazorSupervisionRBI.Data;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/")]
    public partial class Fetch_App : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 195 "C:\Users\ADOMEON\BlazorSupervisionRBI\Pages\Fetch_App.razor"
       
    private string solarWindsLink = "http://supervision.cloudrbi.com";
    private List<Overview> overviewSymantec;
    private List<Overview> overviewVeeam;
    private List<Overview> overviewNode;
    private Dictionary<int, List<string>> statusLayout = new Dictionary<int, List<string>>(){
{0,new List<string>{"unknown","lighblue"}},
{2,new List<string>{"down","tomato"}},
{3,new List<string>{"warning","gold"}},
{12, new List<string>{"unreachable","mediumblue"}},
{14, new List<string>{"critical","red"}},
{27, new List<string>{"disabled","grey"}}
};
    private Dictionary<int, List<DetailsNode>> detailsNodeBySeverity = new Dictionary<int, List<DetailsNode>>();
    private Dictionary<int, List<DysfunctionalHardware>> categoryByHardwareInfo = new Dictionary<int, List<DysfunctionalHardware>>();
    List<DysfunctionalHardware> hardwareInfoByNode = new List<DysfunctionalHardware>();
    Dictionary<int, List<DetailsAPM>> detailsSymantecAppBySeverity = new Dictionary<int, List<DetailsAPM>>();
    Dictionary<int, List<DetailsAPM>> detailsVeeamAppBySeverity = new Dictionary<int, List<DetailsAPM>>();
    Dictionary<int, List<DetailsComponent>> detailsComponentByApp = new Dictionary<int, List<DetailsComponent>>();

    protected override async Task OnInitializedAsync()
    {
        overviewSymantec = await OverviewService.GetAppBySeverityAsync("Symantec");
        overviewVeeam = await OverviewService.GetAppBySeverityAsync("Veeam");
        overviewNode = await OverviewService.GetNodeBySeverityAsync();

        foreach (var item in overviewNode)
        {
            List<DetailsNode> singleDetailList = await DetailsNodeService.GetDetailsNodeAsync(item.severity);
            detailsNodeBySeverity.Add(item.severity, singleDetailList);
        }

        foreach (var item in await DysfunctionalHardwareService.GetDysfunctionalHardwareAsync())
        {
            if (categoryByHardwareInfo.ContainsKey(item.hardwareInfoID))
            {
                categoryByHardwareInfo[item.hardwareInfoID].Add(new DysfunctionalHardware { categoryName = item.categoryName });
            }
            else
            {
                categoryByHardwareInfo.Add(item.hardwareInfoID, new List<DysfunctionalHardware>() {new DysfunctionalHardware {categoryName = item.categoryName }});
                hardwareInfoByNode.Add(new DysfunctionalHardware{
                    hardwareInfoID = item.hardwareInfoID, nodeName = item.nodeName, detailsUrl = item.detailsUrl, alertMessage = item.alertMessage
                    });
            }
        }

        foreach (var item in overviewSymantec)
        {
            List<DetailsAPM> singleDetailList = await DetailsAPMService.GetDetailsAPMAsync(item.severity, "Symantec");
            detailsSymantecAppBySeverity.Add(item.severity, singleDetailList);
        }

        foreach (var list in detailsSymantecAppBySeverity.Values)
        {
            foreach (var item in list)
            {
                List<DetailsComponent> singleComponentList = await DetailsAPMService.GetDetailsComponentAsync(item.applicationID);
                detailsComponentByApp.Add(item.applicationID, singleComponentList);
            }
        }

        foreach (var item in overviewVeeam)
        {
            List<DetailsAPM> singleDetailList = await DetailsAPMService.GetDetailsAPMAsync(item.severity, "Veeam");
            detailsVeeamAppBySeverity.Add(item.severity, singleDetailList);
        }

        foreach (var list in detailsVeeamAppBySeverity.Values)
        {
            foreach (var item in list)
            {
                List<DetailsComponent> singleComponentList = await DetailsAPMService.GetDetailsComponentAsync(item.applicationID);
                detailsComponentByApp.Add(item.applicationID, singleComponentList);
            }
        }
        StateHasChanged();
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private DetailsAPMService DetailsAPMService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private DysfunctionalHardwareService DysfunctionalHardwareService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private DetailsNodeService DetailsNodeService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private OverviewService OverviewService { get; set; }
    }
}
#pragma warning restore 1591
