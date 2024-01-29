using Microsoft.AspNetCore.Mvc;
using MyTeams.Server.Data;

namespace MyTeams.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TeamController : ControllerBase {

    private readonly DataContext _dataContext;

    public TeamController(DataContext dataContext) {
        _dataContext = dataContext;
    }

    //[HttpGet]
    //public async Task<ActionResult<List<Team>>>
    //GetTeams() {
    //    var result = await GetDatabaseTeams();
    //    return Ok(result.Value);
    //}

    //[HttpPost]
    //public async Task<ActionResult<Team>>
    //AddTeam(TeamInput team) {
    //    var newTeam = new DbTeam(team.TeamName, team.MatchDayId);
    //    newTeam.ParentTeamId = team.ParentTeamId;
    //    _dataContext.Teams.Add(newTeam);
    //    await _dataContext.SaveChangesAsync();
    //    foreach (var playerId in team.PlayerIds) {
    //        var player = _dataContext.GetPlayer(playerId);
    //        _dataContext.TeamPlayers.Add(new TeamPlayer(id: 0, teamId: newTeam.Id, playerId: player.Id));
    //    }
    //    await _dataContext.SaveChangesAsync();
    //    var res = await GetDatabaseTeam(newTeam.Id);
    //    return Ok(res.Value);
    //}

    //[HttpPost]
    //[Route("[action]")]
    //public async Task<ActionResult<bool>>
    //RemoveTeam(Team team) {
    //    foreach (var player in team.Players) {
    //        var teamPlayer = await _dataContext.TeamPlayers.Where(x => x.PlayerId == player.Id && x.TeamId == team.Id).ToListAsync();

    //        _dataContext.TeamPlayers.Remove(teamPlayer[0]);
    //    }
    //    var dbTeam = _dataContext.Teams.Find(team.Id);
    //    if (dbTeam != null)
    //        _dataContext.Teams.Remove(dbTeam);
    //    await _dataContext.SaveChangesAsync();
    //    return Ok(true);
    //}

    //[HttpPost]
    //[Route("[action]")]
    //public async Task<ActionResult<bool>>
    //RenameTeam(Team team) {
    //    var dbTeam = _dataContext.Teams.Find(team.Id);
    //    if (dbTeam != null)
    //        dbTeam.Name = team.Name;
    //    await _dataContext.SaveChangesAsync();
    //    return Ok(true);
    //}

    //public async Task<ActionResult<List<Team>>>
    //GetDatabaseTeams() {

    //    var dbTeams = await _dataContext.Teams.ToListAsync();
    //    var subTeams = dbTeams.Where(x => x.IsSubTeam).ToList();
    //    var resultTeams = GetTeamsAndSubTeams(dbTeams, subTeams).ToList();

    //    return new ActionResult<List<Team>>(value: resultTeams);

    //    IEnumerable<Team>
    //    GetTeamsAndSubTeams(List<DbTeam> teams, List<DbTeam> subTeams) {
    //        foreach (var dbTeam in teams) {
    //            var dbPlayers = _dataContext.TeamPlayers.Where(player => player.TeamId == dbTeam.Id).ToListAsync().Result;
    //            var players = new List<Player>();
    //            foreach (var dbPlayer in dbPlayers) {
    //                if (!_dataContext.TryGetPlayer(dbPlayer.PlayerId, out var player))
    //                    continue;
    //                players.Add(player);
    //            }
    //            if (players.Count == 0)
    //                continue;

    //            yield return new Team(
    //                id: dbTeam.Id,
    //                name: dbTeam.Name,
    //                subTeams: subTeams.Where(x => x.ParentTeamId == dbTeam.Id).MapToList(subTeam => {
    //                    var dbSubPlayers = _dataContext.TeamPlayers.Where(player => player.TeamId == subTeam.Id).ToListAsync().Result;
    //                    return new Team(
    //                        id: subTeam.Id,
    //                        name: subTeam.Name,
    //                        subTeams: new List<Team>(),
    //                        players: dbSubPlayers.MapToList(player => _dataContext.GetPlayer(player.PlayerId)));
    //                }
    //                ),
    //                players: players
    //            );
    //        }
    //    }
    //}

    //public async Task<ActionResult<Team>>
    //GetDatabaseTeam(int id) {

    //    var dbTeam = await _dataContext.Teams.FindAsync(id);

    //    var dbPlayers = _dataContext.TeamPlayers.Where(player => player.TeamId == dbTeam.Id).ToListAsync().Result;
    //    var team = new Team(
    //        id: dbTeam.Id,
    //        name: dbTeam.Name,
    //        subTeams: new List<Team>(),
    //        players: dbPlayers.MapToList(player => _dataContext.GetPlayer(player.PlayerId))
    //    );


    //    return new ActionResult<Team>(value: team);
    //}
}