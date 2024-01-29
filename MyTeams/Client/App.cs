using MyTeams.Client.Dialogs;
using MyTeamsCore;
using System.Collections.Immutable;

namespace MyTeams.Client;

public record App {
    public HttpClient Client { get; init; }
    public SessionUser? User { get; init; }
    public DatabaseCache DatabaseCache { get; set; } = DatabaseCache.Empty;
    public MatchEditor? MatchEditor { get; init; }
    public AppTabs SelectedTab { get; init; } = AppTabs.Other;
    public IDialog? Dialog { get; init; } = null;
    public PLayersReport Report { get; init; } = PLayersReport.Empty;


    public int AppStateId { get; init; }


    public MatchDay? LastMatchDay { get; init; } = null;
    public ShortMatchDay ShortLastMatchDay { get; init; } = null;


    public App(SessionUser? user) {
        User = user;
    }

    public int ReduxIndex { get; init; } = 0;

    public static App
    Initial(HttpClient client) => new App(client);

    public App(HttpClient client) {
        Client = client;
    }

    public bool UserCanEdit() {
        return true;
        return User != null;
    }

    public DbMatchDay LastDbMatchDay => DatabaseCache.MatchDays.Last();
    public ImmutableList<DbMatch> Matches => DatabaseCache.Matches;
    public ImmutableList<DbTeam> Teams => DatabaseCache.Teams;
    public ImmutableList<DbMatchGoal> Goals => DatabaseCache.Goals;


}

public class SessionUser {
    public string Name { get; }
    public string AccessToken { get; }

    public SessionUser(string name, string accessToken) {
        Name = name;
        AccessToken = accessToken;
    }
}

public enum AppTabs {
    Stats,
    Other
}
