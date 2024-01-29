using MyTeamsCore;

namespace MyTeams.Client.Messages {



    public class
        TeamMessage : IMessage {
        public Team Team { get; set; }
        public TeamMessage(Team team) {
            Team = team;
        }
    }

    public class
    GetDatabaseTeamMessage : IMessage {
        public int TeamId { get; set; }
        public GetDatabaseTeamMessage(int teamId) {
            TeamId = teamId;
        }

    }

    public class
    TeamPlayerAddMessage : IMessage {
        public Player Player { get; set; }
        public int TeamId { get; set; }
        public TeamPlayerAddMessage(Player player, int teamId) {
            Player = player;
            TeamId = teamId;
        }
    }

    public class TeamPlayerAddRemoveMessage : IMessage {
        public Player AddPlayer { get; set; }
        public Player RemovePlayer { get; set; }
        public int TeamId { get; set; }
        public TeamPlayerAddRemoveMessage(Player addPlayer, Player removePlayer, int teamId) {
            AddPlayer = addPlayer;
            RemovePlayer = removePlayer;
            TeamId = teamId;
        }
    }

    public class
    RenameTeamMessage : IMessage {
        public Team Team { get; set; }
        public RenameTeamMessage(Team team) {
            Team = team;
        }
    }

    public class
    RemoveTeamMessage : IMessage {
        public Team Team { get; set; }
        public RemoveTeamMessage(Team team) {
            Team = team;
        }
    }

    public class
    AddTeamMessage : IMessage {
        public TeamInput Team { get; set; }
        public AddTeamMessage(TeamInput team) {
            Team = team;
        }
    }

    public class
    TeamPlayerRemoveMessage : IMessage {
        public Player Player { get; set; }
        public int TeamId { get; set; }
        public TeamPlayerRemoveMessage(Player player, int teamId) {
            Player = player;
            TeamId = teamId;
        }
    }

    public class
    DbTeamMessage : IMessage {
        public DbTeam DbTeam { get; set; }
        public DbTeamMessage(DbTeam dbTeam) {
            DbTeam = dbTeam;
        }
    }

    public class
    TeamPlayerMessage : IMessage {
        public TeamPlayer TeamPlayer { get; set; }
        public TeamPlayerMessage(TeamPlayer teamPlayer) {
            TeamPlayer = teamPlayer;
        }
    }

    public class
    GetTeamPlayerMessage : IMessage {
        public int TeamPlayerId { get; set; }
        public GetTeamPlayerMessage(int teamPlayerId) {
            TeamPlayerId = teamPlayerId;
        }
    }



    public class
    GetDatabaseTeamsMessage : IMessage {
        public static GetDatabaseTeamsMessage
        Instance => new GetDatabaseTeamsMessage();
    }

    public class GetMatchDayTeamsMessage : IMessage {
        public DbMatchDay MatchDay { get; set; }
        public GetMatchDayTeamsMessage(DbMatchDay matchDay) {
            MatchDay = matchDay;
        }
    }
}
