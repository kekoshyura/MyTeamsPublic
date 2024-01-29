using Microsoft.AspNetCore.Mvc;
using MyTeams.Server.Data;

namespace MyTeams.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PlayerController : ControllerBase {

    private readonly DataContext _dataContext;

    public PlayerController(DataContext dataContext) {
        _dataContext = dataContext;
    }

    //[HttpGet]
    //public async Task<ActionResult<List<Player>>>
    //GetPlayers() {
    //    var result = await _dataContext.Players.ToListAsync();
    //    return Ok(result);
    //}

    //[HttpPost]
    //public async Task<ActionResult<List<Player>>>
    //AddPlayer(Player player) {
    //    _dataContext.Players.Add(player);
    //    await _dataContext.SaveChangesAsync();
    //    return Ok(await GetDatabasePlayers());
    //}

    //[HttpGet]
    //[Route("[action]")]
    //public async Task<ActionResult<List<Player>>>
    //GetFreePlayers() {
    //    var matchDayTeams = await _dataContext.GetCurrentTeams();
    //    var takenPlayers = matchDayTeams.Value.SelectMany(x => x.Players).ToList();
    //    var allPlayers = await _dataContext.Players.ToListAsync();
    //    var result = allPlayers.Where(x => takenPlayers.All(y => y.Id != x.Id)).ToList();
    //    return Ok(result);
    //}

    //public async Task<ActionResult<List<Player>>>
    //GetDatabasePlayers() =>
    //     await _dataContext.Players.ToListAsync();
}