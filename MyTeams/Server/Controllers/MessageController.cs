using Microsoft.AspNetCore.Mvc;
using MyTeams.Client.Messages;
using MyTeams.Server.Data;

namespace MyTeams.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase {

    private readonly DataContext _dataContext;

    public MessageController(DataContext dataContext) {
        _dataContext = dataContext;
    }

    [HttpPost]
    public async Task<ActionResult<Message>>
    Post(Message message) {

        var type = message.ParseJson();
        var result = type switch {
            AddPlayerMessage addPlayerMessage => await _dataContext.AddPlayer(addPlayerMessage),
            GetPlayersMessage => await _dataContext.GetPlayers(),
            GetFreePlayersMessage => await _dataContext.GetFreePlayers(),
            GetMatchesMessage => await _dataContext.GetMatches(),
            GetLastMatchDayMatchesMessage => await _dataContext.GetLastMatchDayMatches(),
            GetLastMatchDayMessage => await _dataContext.GetLastMatchDay(),
            GetMatchDaysMessage => await _dataContext.GetMatchDays(),
            GetLastMatchMessage => await _dataContext.GetLastMatch(),
            AddMatchMessage addMatchMessage => await _dataContext.AddMatch(addMatchMessage),
            AddTeamsToExistMatchDayMessage addTeamsToExistMatchDayMessage => await _dataContext.AddTeamsToMatchDay(addTeamsToExistMatchDayMessage),
            DeleteMatchMessage deleteMatchMessage => await _dataContext.DeleteMatch(deleteMatchMessage),
            FinishMatchMessage => await _dataContext.FinishMatchDay(),
            //SetMatchResultMessage setMatchResultMessage => await _dataContext.SetMatchResult(setMatchResultMessage),
            SetHomeTeamMatchResultMessage setHomeTeamMatchResult => await _dataContext.SetHomeTeamMatchResult(setHomeTeamMatchResult),
            SetAwayTeamMatchResultMessage setAwayTeamMatchResult => await _dataContext.SetAwayTeamMatchResult(setAwayTeamMatchResult),
            ReplaceMatchTeamsMessage replaceMatchTeamsMessage => await _dataContext.ReplaceMatchTeams(replaceMatchTeamsMessage),
            SetGoalMessage setGoalMessage => await _dataContext.SetGoal(setGoalMessage),
            GetMatchDayMessage => await _dataContext.GetMatchDay(),
            AddMatchDayMessage addMatchDayMessage => await _dataContext.AddNewMatchDay(addMatchDayMessage),
            //AddShortMatchDayMessage addShortMatchDayMessage => await _dataContext.AddNewShortMatchDay(addShortMatchDayMessage),
            GetDatabaseTeamMessage getDatabaseTeam => await _dataContext.GetDatabaseTeam(getDatabaseTeam),
            RenameTeamMessage renameTeamMessage => await _dataContext.RenameTeam(renameTeamMessage),
            RemoveTeamMessage removeTeamMessage => await _dataContext.RemoveTeam(removeTeamMessage),
            AddTeamMessage addTeamMessage => await _dataContext.AddTeam(addTeamMessage),
            GetDatabaseTeamsMessage => await _dataContext.GetDatabaseTeams(),
            GetLastMatchDayTeamsMessage => await _dataContext.Teams(),
            GetDatabaseCacheMessage => await _dataContext.GetDatabaseCache1(),
            TeamPlayerAddRemoveMessage teamPlayerAddRemoveMessage => await _dataContext.TeamPlayerAddRemove(teamPlayerAddRemoveMessage),
            //TeamPlayerAddMessage teamPlayerAddMessage => await _dataContext.TeamPlayerAdd(teamPlayerAddMessage),
            //TeamPlayerRemoveMessage teamPlayerRemoveMessage => await _dataContext.TeamPlayerRemove(teamPlayerRemoveMessage),


            _ => throw new NotImplementedException($"{type.GetType()} is not implemented")
        };

        return Ok(result);
    }

}