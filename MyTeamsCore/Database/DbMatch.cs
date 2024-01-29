using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeamsCore;
public class DbMatch {
    public int Id {get;set;}
    public int MatchDayId { get; set; }
    public int ATeamId {get;set;}
    public int BTeamId {get;set;}
    public int AGoals {get;set;}
    public int BGoals {get;set;}
    public bool IsFinished {get;set;}

    public DbMatch(int id, int matchDayId, int aTeamId, int bTeamId, int aGoals, int bGoals, bool isFinished) {
        Id = id;
        MatchDayId = matchDayId;
        ATeamId = aTeamId;
        BTeamId = bTeamId;
        AGoals = aGoals;
        BGoals = bGoals;
        IsFinished = isFinished;
    }

    public DbMatch() {
        
    }

    public int? Winner => AGoals > BGoals 
        ? ATeamId
        : AGoals == BGoals 
            ? null
            : BTeamId;

    public bool
    HasTeam(int teamId) => ATeamId == teamId || BTeamId == teamId;

    public bool
    TeamWin(Team team) => Winner == team.Id;

    public int 
    TeamGoals(Team team) => ATeamId == team.Id
        ? AGoals
        : BTeamId == team.Id 
            ? BGoals
            : throw new InvalidOperationException();

    public int
    TeamGoalsAgainst(Team team) => ATeamId == team.Id
       ? BGoals
       : BTeamId == team.Id
           ? AGoals
           : throw new InvalidOperationException();
}


public class
DbLog{
    public int id {get;set;}
    public string message {get;set;}

    public DbLog(int id, string message) {
        this.id = id;
        this.message = message;
    }
}