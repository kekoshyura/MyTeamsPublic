using Microsoft.AspNetCore.Mvc;
using MyTeams.Server.Data;
using System.Text.Json;
public static class
MatchDayControllerHelper {

    public static HashSet<string> AccessTokens = new HashSet<string>() {
        "669944831"
    };

    public static bool
    HasAccess(this string accessToken) => AccessTokens.Contains(accessToken);


}

[Route("api/[controller]")]
[ApiController]
public class MatchDayController : ControllerBase {

    private readonly DataContext _dataContext;

    public MatchDayController(DataContext dataContext) {
        _dataContext = dataContext;
    }

    //[HttpGet]
    //public async Task<ActionResult<List<MatchDayResult>>>
    //GetMatchDay() {
    //    var lastMatchDay = await _dataContext.GetLastMatchDayDb();
    //    var result = await _dataContext.GetMatchDayMatches(lastMatchDay);
    //    var teams = await _dataContext.GetMatchDayTeams(lastMatchDay);
    //    if (teams.Value == null)
    //        return NoContent();
    //    var tableTeams = teams.Value.GetSortedTeams(result).OrderByDescending(x => x.Points).ToList();
    //    return Ok(new MatchDayResult(tableTeams, result));
    //}

    //[HttpGet]
    //[Route("[action]")]
    //public async Task<ActionResult<List<DbTeam>>>
    //Teams() {
    //    var lastMatchDay = await _dataContext.GetLastMatchDayDb();
    //    var teams = await _dataContext.GetMatchDayTeams(lastMatchDay);
    //    if (teams.Value == null)
    //        return NoContent();
    //    return Ok(teams.Value);
    //}

    //[HttpPost]
    //[Route("[action]")]
    //public async Task<ActionResult<string>>
    //AddNewMatchDay(MatchDayInput matchDay) {
    //    //await _dataContext.Log($"Received, trying to serialize");
    //    var message = JsonSerializer.Serialize(matchDay);
    //    //await _dataContext.Log($"Received: \n{message}");
    //    await _dataContext.AddMatchDay(matchDay.Teams);

    //    return Ok($"Success. New match day added: \n{message}");
    //}

    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult<string>>
    AddNewMatchDay(BotMatchDay matchDay) {
        var message = JsonSerializer.Serialize(matchDay);
        await _dataContext.AddMatchDay(matchDay.Teams);

        return Ok($"Success. New match day added: \n{message}");
    }



    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult<BotPlayerRatings>>
    GetPlayerRatings(BotPlayers players) {
        await _dataContext.Log($"Received, trying to serialize");
        var message = JsonSerializer.Serialize(players);
        await _dataContext.Log($"Received: \n{message}");
        var result = await _dataContext.GetRatings(players);

        return Ok(result);
    }
}

public struct BotMatchDay {
    public BotMatchDayTeam[] Teams { get; set; }

    public BotMatchDay(BotMatchDayTeam[] teams) {
        Teams = teams;
    }
}

public struct BotMatchDayTeam {
    public string Name { get; set; }
    public BotMatchDayTeamPlayer[] players { get; set; }

    public BotMatchDayTeam(string name, BotMatchDayTeamPlayer[] players) {
        Name = name;
        this.players = players;
    }
}

public struct BotMatchDayTeamPlayer {
    public string name { get; set; }
    public string nickname { get; set; }
    public int telegramId { get; set; }

    public BotMatchDayTeamPlayer(string name, string nickname, int telegramId) {
        this.name = name;
        this.nickname = nickname;
        this.telegramId = telegramId;
    }
}

public struct BotPlayers {
    public int[] PlayerTelegramIds { get; set; }

    public BotPlayers(int[] playerTelegramIds) {
        PlayerTelegramIds = playerTelegramIds;
    }
}

public struct BotPlayerRatings {
    public Dictionary<int, double> PlayerRatings { get; set; }

    public BotPlayerRatings(Dictionary<int, double> playerRatings) {
        PlayerRatings = playerRatings;
    }
}