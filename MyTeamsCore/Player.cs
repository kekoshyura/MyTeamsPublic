
namespace MyTeamsCore;

public class Player : IEquatable<Player> {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Nickname { get; set; }
    public int TelegramId { get; set; }

    public Player(int id, string name, string nickname, int telegramId) {
        Id = id;
        Name = name;
        Nickname = nickname;
        TelegramId = telegramId;
    }

    public Player() { }

    public bool Equals(Player? other) {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Id == other.Id && Name == other.Name && Nickname == other.Nickname && TelegramId == other.TelegramId;
    }

    public override bool Equals(object? obj) {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((Player)obj);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Id, Name, Nickname, TelegramId);
    }
}

public class PlayerInput {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Nickname { get; set; }
    public string TelegramId { get; set; }

    public PlayerInput(int id, string name, string nickname, string telegramId) {
        Id = id;
        Name = name;
        Nickname = nickname;
        TelegramId = telegramId;
    }

    public PlayerInput() {

    }
}

public class PlayerStats {
    public Player Player { get; }
    public int Matches { get; }
    public int Wins { get; }
    public int Draws { get; }
    public int TeamGoals { get; }
    public int TeamGoalsAgainst { get; }
    public bool IsSelected { get; }


    public PlayerStats(Player player, int matches, int wins, int draws, int teamGoals, int teamGoalsAgainst, bool isSelected) {
        Player = player;
        Matches = matches;
        Wins = wins;
        Draws = draws;
        TeamGoals = teamGoals;
        TeamGoalsAgainst = teamGoalsAgainst;
        IsSelected = isSelected;
    }

    public PlayerStats() {

    }

    public int Loses => Matches - Wins - Draws;
    public int Points => Wins * 3 + Draws;

    public double WinRate => Math.Round((double)Points / (Matches * 3) * 100, 1);

    public double GoalsAverage => Math.Round((double)TeamGoals / Matches, 2);
    public double GoalsAgainstAverage => Math.Round((double)TeamGoalsAgainst / Matches, 2);

}

public class PlayersStatsAndSeason {
    public List<PlayerStats> PlayersStats { get; set; }
    public PlayersStatsSeason Season { get; set; }
}






public class PlayerStatsBuilder {
    public Dictionary<int, PlayerStats> StatsById { get; }
    public HashSet<int>? PlayersToBuild { get; }

    public PlayerStatsBuilder(Dictionary<int, PlayerStats> statsById, HashSet<int>? playersToBuild) {
        StatsById = statsById;
        PlayersToBuild = playersToBuild;
    }

    public static PlayerStatsBuilder
    Default => new PlayerStatsBuilder(new Dictionary<int, PlayerStats>(), null);

    public void
    HandleMatch(Match match) {
        foreach (var player in match.HomeTeam.Players)
            if (PlayersToBuild == null || PlayersToBuild.Contains(player.TelegramId))
                HandlePlayer(player, match, match.HomeTeam);

        foreach (var player in match.AwayTeam.Players)
            if (PlayersToBuild == null || PlayersToBuild.Contains(player.TelegramId))
                HandlePlayer(player, match, match.AwayTeam);

    }

    private void
    HandlePlayer(Player player, Match match, Team playerTeam) {
        if (!StatsById.TryGetValue(player.Id, out var stats)) {
            stats = new PlayerStats(
                player: player,
                matches: 0,
                wins: 0,
                draws: 0,
                teamGoals: 0,
                teamGoalsAgainst: 0,
                isSelected: false
            );
            StatsById.Add(player.Id, stats);
        }

        PlayerStats newStats = new PlayerStats(
            player: player,
            matches: stats.Matches + 1,
            wins: stats.Wins + (match.TeamWin(playerTeam) ? 1 : 0),
            draws: stats.Draws + ((match.IsDraw) ? 1 : 0),
            teamGoals: stats.TeamGoals + match.TeamGoals(playerTeam),
            teamGoalsAgainst: stats.TeamGoalsAgainst + match.TeamGoalsAgainst(playerTeam),
            isSelected: stats.IsSelected);

        StatsById[player.Id] = newStats;
        //stats.Matches += 1;
        //stats.Wins += match.TeamWin(playerTeam) ? 1 : 0;
        //stats.Draws += match.IsDraw ? 1 : 0;
        //stats.TeamGoals += match.TeamGoals(playerTeam);
        //stats.TeamGoalsAgainst += match.TeamGoalsAgainst(playerTeam);

    }
}

public class PlayerStatsElement {
    public string Name { get; set; }

    public string Code { get; set; }
    public bool IsSelected { get; set; }

}

public enum
SortOrder {
    Ascending,
    Descending
}

public enum
PlayersStatsSeason {
    Season1,
    Season2,
    AllTime
}