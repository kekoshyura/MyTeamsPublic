namespace MyTeamsCore;
public record Match {
    public int Id { get; init; }
    public int MatchDayId { get; init; }
    public Team HomeTeam { get; init; }
    public Team AwayTeam { get; init; }
    public bool IsFinished { get; init; }

    public Score Score { get; set; }

    public Match(int id, int matchDayId, Team homeTeam, Team awayTeam, bool isFinished, Score score) {
        Id = id;
        MatchDayId = matchDayId;
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        IsFinished = isFinished;
        Score = score;
    }

    public int HomeGoals => Score.HomeGoals.Count;
    public int AwayGoals => Score.AwayGoals.Count;

    public int? Winner => HomeGoals > AwayGoals
       ? HomeTeam.Id
       : HomeGoals == AwayGoals
           ? null
           : AwayTeam.Id;

    public bool IsDraw => HomeGoals == AwayGoals;

    public bool
    HasTeam(Team team) =>
        HasTeamId(team.Id) ||
        team.SubTeams.Any(subTeam => HasTeamId(subTeam.Id));

    public bool
    HasTeamId(int teamId) =>
        HomeTeam.Id == teamId || AwayTeam.Id == teamId;

    public bool
    TeamWin(Team team) => Winner == team.Id || team.SubTeams.Any(subTeam => Winner == subTeam.Id);

    public int
    TeamGoals(Team team) => HomeTeam.Id == team.Id || team.SubTeams.Any(subTeam => subTeam.Id == HomeTeam.Id)
        ? HomeGoals
        : AwayTeam.Id == team.Id || team.SubTeams.Any(subTeam => subTeam.Id == AwayTeam.Id)
            ? AwayGoals
            : throw new InvalidOperationException();

    public int
    TeamGoalsAgainst(Team team) => HomeTeam.Id == team.Id || team.SubTeams.Any(subTeam => subTeam.Id == HomeTeam.Id)
       ? AwayGoals
       : AwayTeam.Id == team.Id || team.SubTeams.Any(subTeam => subTeam.Id == AwayTeam.Id)
           ? HomeGoals
           : throw new InvalidOperationException();
}

public class
Score {
    public List<Goal> HomeGoals { get; set; }
    public List<Goal> AwayGoals { get; set; }

    public Score(List<Goal> homeGoals, List<Goal> awayGoals) {
        HomeGoals = homeGoals;
        AwayGoals = awayGoals;
    }

    public Score() {
    }
}

public class Goal {
    public int Id { get; set; }
    public Player? Player { get; set; }
    public Player? Assist { get; set; }

    public Goal(int id, Player? player, Player? assist) {
        Id = id;
        Player = player;
        Assist = assist;
    }

    public Goal() {

    }
}


public static class
MatchHelper {
    public static DbMatch
    ToDbMatch(this Match match) =>
        new DbMatch(
            id: match.Id,
            matchDayId: match.MatchDayId,
            aTeamId: match.HomeTeam.Id,
            bTeamId: match.AwayTeam.Id,
            aGoals: match.HomeGoals,
            bGoals: match.AwayGoals,
            isFinished: match.IsFinished);
}
