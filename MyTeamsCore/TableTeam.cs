using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeamsCore;

public class 
TableTeam {
    public Team Team {get;set;}
    public int Matches { get; set; }
    public int Goals {get;set;}
    public int GoalsAgainst {get;set;}
   
    
    public int Wins {get;set;}
    public int Draws {get;set;}

    public TableTeam(Team team, int matches, int goals, int goalsAgainst, int wins, int draws) {
        Team = team;
        Matches = matches;
        Goals = goals;
        GoalsAgainst = goalsAgainst;
        Wins = wins;
        Draws = draws;
    }

    public int Loses => Matches - Wins - Draws;
    public int Points => Wins * 3 + Draws;
    public string Name => Team.Name;

    public int GoalsDifference => Goals - GoalsAgainst;
}

public static class 
TableTeamHelper {

    public static IEnumerable<TableTeam>
    GetSortedTeams(this List<Team> teams, List<DbMatch> matches) {
        foreach (var team in teams) {
            var teamMatches = matches.Where(match => match.HasTeam(team.Id)).ToList();
            if (!teamMatches.Any()) 
                continue;
            var wins = teamMatches.Count(match => match.TeamWin(team));
            var draws = teamMatches.Count(match => match.Winner == null);
            var goals = teamMatches.Sum(match => match.TeamGoals(team));
            var goalAgainst = teamMatches.Sum(match => match.TeamGoalsAgainst(team));

            yield return new TableTeam(
                team: team,
                matches: teamMatches.Count(),
                goals: goals,
                goalsAgainst: goalAgainst,
                wins: wins,
                draws: draws);
        }
    }
}
