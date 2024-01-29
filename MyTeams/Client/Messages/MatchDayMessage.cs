using MyTeams.Client.Inputs;
using MyTeamsCore;
using MyTeamsCore.Common;

namespace MyTeams.Client.Messages {
    public class
    MatchResultMessage : IMessage {
        public List<TableTeam> Table { get; set; }
        public List<DbMatch> Matches { get; set; }

        public MatchResultMessage(List<TableTeam> table, List<DbMatch> matches) {
            Table = table;
            Matches = matches;
        }
    }

    public class
    GetMatchDayMessage : IMessage {
        public static GetMatchDaysMessage
        Instance => new GetMatchDaysMessage();
    }

    public class
    DbTeamsMessage : IMessage {
        public List<DbTeam> Teams { get; set; }
        public DbTeamsMessage(List<DbTeam> teams) {
            Teams = teams;
        }
    }
    public class
    TeamsMessage : IMessage {
        public List<Team> Teams { get; set; }
        public TeamsMessage(List<Team> teams) {
            Teams = teams;
        }
    }

    public class
    MatchErrorMessage : IMessage {
        public IsAddedMessageTypes Error { get; set; }
        public DbMatch Match { get; set; } = new DbMatch();

        public MatchErrorMessage(IsAddedMessageTypes error, DbMatch match = null) {
            Error = error;
            Match = match ?? new DbMatch();
        }
    }

    public class
    GoalAddedMessage : IMessage {
        public ResultMessageTypes Error { get; set; }
        public GoalAddedMessage(ResultMessageTypes error) {
            Error = error;
        }
    }

    public class
    GetLastMatchDayTeamsMessage : IMessage {
        public static GetLastMatchDayTeamsMessage
        Instance => new GetLastMatchDayTeamsMessage();
    }

    public class
    AddMatchDayMessage : IMessage {
        public MatchDayInput MatchDay { get; set; }
        public AddMatchDayMessage(MatchDayInput matchDay) {
            MatchDay = matchDay;
        }
    }

    public class
    AddShortMatchDayMessage : IMessage {
        public ShortMatchDay ShortMatchDay { get; set; }
        public AddShortMatchDayMessage(ShortMatchDay matchDay) {
            ShortMatchDay = matchDay;
        }
    }



    public class DatabaseCacheMessage : IMessage {
        public DatabaseCache DatabaseCache { get; set; }
        public DatabaseCacheMessage(DatabaseCache databaseCache) {
            DatabaseCache = databaseCache;
        }
    }

    public class GetMatchIdMessage : IMessage {
        public static GetMatchIdMessage
        Instanse => new GetMatchIdMessage();
    }

    public class
    GetDatabaseCacheMessage : IMessage {
        public static GetDatabaseCacheMessage
        Instance => new GetDatabaseCacheMessage();
    }
}
