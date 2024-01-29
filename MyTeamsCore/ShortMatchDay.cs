namespace MyTeamsCore {

    public record
    ShortMatchDay {
        public int Id { get; }
        public List<Match> Matches { get; init; }
        public List<Player> Players { get; init; }
        public List<Team> Teams { get; init; }

        public ShortMatchDay(List<Match> matches, List<Player> players, List<Team> teams) {
            Matches = matches;
            Players = players;
            Teams = teams;
        }

        public static ShortMatchDay
        Empty => new ShortMatchDay(
            new List<Match>(),
            new List<Player>(),
            new List<Team>());
    }
}
