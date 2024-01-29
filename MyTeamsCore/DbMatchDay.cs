using System.Data;

namespace MyTeamsCore;
public class
DbMatchDay {
    public int Id { get; set; } = 0;
    public bool IsFinished { get; set; }
    public DateTime Date { get; set; }
    public DbMatchDay() { }

}

public class
DbMatchDayTeam {
    public int Id { get; set; } = 0;
    public int MatchDayId { get; set; }
    public int TeamId { get; set; }

    public DbMatchDayTeam(int id, int matchDayId, int teamId) {
        Id = id;
        MatchDayId = matchDayId;
        TeamId = teamId;
    }
}

public static class MatchDayHelper {

    public static HashSet<string> TeamNames = new HashSet<string>() {
        "Team1",
        "Team2",
        "Team3"
    };

    public static string
    GetNewTeamName(this List<DbTeam> currentTeams) {
        foreach (var team in TeamNames) {
            if (currentTeams.All(x => x.Name != team))
                return team;
        }
        throw new InRowChangingEventException("No free team name");
    }
}

public struct
MatchDayResult {
    public List<TableTeam> Table { get; }
    public List<DbMatch> Matches { get; }

    public MatchDayResult(List<TableTeam> table, List<DbMatch> matches) {
        Table = table;
        Matches = matches;
    }
}