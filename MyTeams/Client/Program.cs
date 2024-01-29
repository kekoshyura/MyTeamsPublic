using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyTeams.Client;
using MyTeams.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<AppView>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddSingleton(typeof(App));

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

AppStore.Instance = new AppStore(initialState: App.Initial(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }));
AppDispatcher.Instance = new AppDispatcher(AppStore.Instance);

AppStore.Instance.StateChanged += async newApp => {
    await AppHelper.ViewState.SetParametersAsync(
        ParameterView.FromDictionary(
            (IDictionary<string, object?>)new Dictionary<string, object>() {
                { "Model", (object)newApp }
            }));
};
await builder.Build().RunAsync();