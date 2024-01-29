using MyTeamsCore.Common;

namespace MyTeamsCore;
public class 
DbMatchGoal : IHasId{
    public int Id {get;set;}
    public int MatchId {get;set;}
    public int PlayerGoalId {get;set;}
    public int PlayerPassId {get;set;}

    public DbMatchGoal(int id, int matchId, int playerGoalId, int playerPassId) {
        Id = id;
        MatchId = matchId;
        PlayerGoalId = playerGoalId;
        PlayerPassId = playerPassId;
    }
}
