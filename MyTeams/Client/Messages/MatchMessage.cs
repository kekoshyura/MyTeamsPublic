using MyTeams.Client.Inputs;
using MyTeamsCore;

namespace MyTeams.Client.Messages {

    public class
    MatchesMessage : IMessage {
        public List<DbMatch> Matches { get; set; }
        public MatchesMessage(List<DbMatch> matches) {
            Matches = matches;
        }
    }

    public class
    MatchDayMessage : IMessage {
        public DbMatchDay Match { get; set; }
        public MatchDayMessage(DbMatchDay match) {
            Match = match;
        }
    }

    public class
    MatchDaysMessage : IMessage {
        public List<DbMatchDay> MatchDays { get; set; }
        public MatchDaysMessage(List<DbMatchDay> matchDays) {
            MatchDays = matchDays;
        }
    }

    public class
    GetMatchesMessage : IMessage {
        public static GetMatchesMessage
        Instance => new GetMatchesMessage();
    }
    public class
    MatchMessage : IMessage {
        public DbMatch Match { get; set; }
        public MatchMessage(DbMatch match) {
            Match = match;
        }
    }
    //public class
    //AddedMessage : IMessage {
    //    public int Id { get; set; }
    //    public AddedMessage(int id) {
    //        Id = id;
    //    }
    //}


    public class
    GetLastMatchDayMatchesMessage : IMessage {
        public static GetLastMatchDayMatchesMessage
        Instance => new GetLastMatchDayMatchesMessage();
    }


    public class
    GetLastMatchDayMessage : IMessage {
        public static GetLastMatchDayMessage
        Instance => new GetLastMatchDayMessage();
    }

    public class
    GetMatchDaysMessage : IMessage {
        public static GetMatchDaysMessage
        Instance => new GetMatchDaysMessage();
    }

    public class
    GetLastMatchMessage : IMessage {
        public static GetLastMatchMessage
        Instance => new GetLastMatchMessage();
    }

    public class
    AddMatchMessage : IMessage {
        public int[] Teams { get; set; }
        public AddMatchMessage(int[] teams) {
            Teams = teams;
        }
    }

    public class
    AddTeamsToExistMatchDayMessage : IMessage {
        public MatchDayTeamInput[] Teams { get; set; }
        public int MatchDayId { get; set; }
        public AddTeamsToExistMatchDayMessage(MatchDayTeamInput[] teams, int matchDayId) {
            Teams = teams;
            MatchDayId = matchDayId;
        }
    }

    public class
    DeleteMatchMessage : IMessage {
        public Match Match { get; set; }
        public DeleteMatchMessage(Match match) {
            Match = match;
        }
    }

    public class
    FinishMatchMessage : IMessage {
        public static FinishMatchMessage
        Instance => new FinishMatchMessage();
    }

    public class
    SetMatchResultMessage : IMessage {

        public DbMatch Match { get; set; }
        public SetMatchResultMessage(DbMatch match) {
            Match = match;
        }
    }

    public class
    SetHomeTeamMatchResultMessage : IMessage {
        public DbMatch Match { get; set; }
        public SetHomeTeamMatchResultMessage(DbMatch match) {
            Match = match;
        }
    }

    public class
    SetAwayTeamMatchResultMessage
        : IMessage {
        public DbMatch Match { get; set; }
        public SetAwayTeamMatchResultMessage(DbMatch match) {
            Match = match;
        }
    }

    public class
    ReplaceMatchTeamsMessage : IMessage {
        public DbMatch Match { get; set; }
        public ReplaceMatchTeamsMessage(DbMatch match) {
            Match = match;
        }
    }

    public class
    SetGoalMessage : IMessage {
        public DbMatchGoal Goal { get; set; }
        public SetGoalMessage(DbMatchGoal goal) {
            Goal = goal;
        }
    }


}
