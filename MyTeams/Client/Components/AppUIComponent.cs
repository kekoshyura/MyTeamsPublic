using System.Collections.Immutable;
using Microsoft.AspNetCore.Components;
using MyTeamsCore.Common;
using MyTeamsCore;

namespace MyTeams.Client.Components;


public class 
AppUIComponent: ComponentBase {
    
    [Parameter]
    public string Class {get;set;}
    
    [Parameter]
    public string Style {get;set;}

}
public static class 
Сss {
    public static string
    T(this bool value, string @class) => value ? @class : string.Empty;

    public static string
    N(this bool value, string @class) => value ? string.Empty : @class;
}