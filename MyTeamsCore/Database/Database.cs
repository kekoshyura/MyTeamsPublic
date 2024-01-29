using MyTeamsCore;
using System.Collections.Immutable;

public record
DatabaseCache {
    public ImmutableList<Player> Players { get; init; }
    public ImmutableList<TeamPlayer> TeamPlayers { get; init; }
    public ImmutableList<DbTeam> Teams { get; init; }
    public ImmutableList<DbMatchDay> MatchDays { get; init; }
    public ImmutableList<DbMatch> Matches { get; init; }
    public ImmutableList<DbMatchGoal> Goals { get; init; }


    public DatabaseCache(ImmutableList<Player> players, ImmutableList<TeamPlayer> teamPlayers, ImmutableList<DbTeam> teams, ImmutableList<DbMatchDay> matchDays, ImmutableList<DbMatch> matches, ImmutableList<DbMatchGoal> goals) {
        Players = players;
        TeamPlayers = teamPlayers;
        Teams = teams;
        MatchDays = matchDays;
        Matches = matches;
        Goals = goals;
    }

    public static DatabaseCache
    Empty => new DatabaseCache(
        ImmutableList<Player>.Empty,
        ImmutableList<TeamPlayer>.Empty,
        ImmutableList<DbTeam>.Empty,
        ImmutableList<DbMatchDay>.Empty,
        ImmutableList<DbMatch>.Empty,
        ImmutableList<DbMatchGoal>.Empty);


    public bool IsEmpty => Players.Count == 0;
}