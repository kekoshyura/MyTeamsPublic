namespace MyTeamsCore;
public class MatchDay {
    public int Id { get; }
    public List<Match> Matches { get; }
    public List<Team> Teams { get; }

    public MatchDay(int id, List<Match> matches, List<Team> teams) {
        Id = id;
        Matches = matches;
        Teams = teams;
    }

    public bool IsFinished => Matches.Count > 0 && Matches[0].IsFinished;
}

public enum MatchDayType {
    Default,
    Short
}
