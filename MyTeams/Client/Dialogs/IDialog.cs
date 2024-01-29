using MyTeamsCore;

namespace MyTeams.Client.Dialogs;

public interface IDialog { }

public class
TeamPlayersDialog : IDialog {
    public Team Team { get; set; }
    public TeamStats TeamsStats { get; set; }

    public TeamPlayersDialog(Team team, TeamStats teamsStats) {
        Team = team;
        TeamsStats = teamsStats;
    }
}

public class
MessageDialog : IDialog {
    public string Message { get; set; }

    public MessageDialog(string message) {
        Message = message;
    }
}

