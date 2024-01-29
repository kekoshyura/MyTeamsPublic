using System.Text.Json.Serialization;

namespace MyTeamsCore;

public class DbTeam {
    public int Id { get; set; } = 0;
    public int MatchDayId { get; set; }
    public string Name { get; set; }
    public int ParentTeamId { get; set; }

    [JsonConstructor]
    public DbTeam(int id, int matchDayId, string name) {
        Id = id;
        MatchDayId = matchDayId;
        Name = name;
    }

    public DbTeam(string name, int matchDayId) {
        Name = name;
        MatchDayId = matchDayId;
    }

    public DbTeam(int matchDayId) {
        MatchDayId = matchDayId;
    }

    public bool IsSubTeam => ParentTeamId != 0;
}

public class TeamInput {
    public string TeamName { get; set; }
    public int MatchDayId { get; set; } = 0;
    public List<int> PlayerIds { get; set; }
    public int ParentTeamId { get; set; }

    public TeamInput(string teamName, int matchDayId, List<int> playerIds, int parentTeamId) {
        TeamName = teamName;
        MatchDayId = matchDayId;
        PlayerIds = playerIds;
        ParentTeamId = parentTeamId;
    }

    public TeamInput() {
        PlayerIds = new List<int>();
    }
}

public class TeamPlayer {
    public int Id { get; set; }
    public int TeamId { get; set; }
    public int PlayerId { get; set; }

    public TeamPlayer(int id, int teamId, int playerId) {
        Id = id;
        TeamId = teamId;
        PlayerId = playerId;
    }

    [JsonConstructor]
    public TeamPlayer(int teamId, int playerId) {
        TeamId = teamId;
        PlayerId = playerId;
    }
}