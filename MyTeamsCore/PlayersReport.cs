using System.Collections.Immutable;

namespace MyTeamsCore {

    public record
    PLayersReport {
        public ImmutableList<PlayerStats> Stats { get; init; }
        public ImmutableList<PlayerReportColumns> Columns { get; }
        public PlayersStatsSeason PlayersStatsSeason { get; init; }

        public MyTeamsCore.SortOrder? sortOrder { get; set; }

        public PLayersReport(ImmutableList<PlayerStats> stats, PlayersStatsSeason playersStatsSeason, ImmutableList<PlayerReportColumns> columns = null) {
            Stats = stats;
            Columns = columns ?? ImmutableList<PlayerReportColumns>.Empty;
            PlayersStatsSeason = playersStatsSeason;
        }

        public static PLayersReport
        Empty => new PLayersReport(
            ImmutableList<PlayerStats>.Empty,
            PlayersStatsSeason.AllTime,
            ImmutableList<PlayerReportColumns>.Empty);
    }


    public enum PlayerReportColumns {
        Matches = 0,
        Wins = 1,
        Draws = 2,
        Loses = 3,
        GoalsAvg = 4,
        GoalsAgainstAvg = 5,
        WinRate = 6
    }
}
