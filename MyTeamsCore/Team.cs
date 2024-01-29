using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeamsCore;

public class Team {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Player> Players {get; set;}

    public List<Team> SubTeams {get;set;}

    public Team(int id, string name, List<Player> players, List<Team> subTeams) {
        Id = id;
        Name = name;
        Players = players;
        SubTeams = subTeams;
    }

    public Team() {
        Players = new List<Player>();
    }

    public override string ToString() =>  Name;
}

public class TeamStats{
    public List<PlayerStats> PlayerStats {get;}

    public TeamStats(List<PlayerStats> playerStats) {
        PlayerStats = playerStats;
    }

    public static TeamStats Empty => new TeamStats(new List<PlayerStats>());

    public bool IsEmpty => PlayerStats.Count == 0;

    public int Rating => (int)PlayerStats.Average(player => player.WinRate);
    public double Goals => Math.Round((double)PlayerStats.Average(player => player.GoalsAverage), 1);
    public double Against => Math.Round((double)PlayerStats.Average(player => player.GoalsAgainstAverage), 1);

}