using Microsoft.AspNetCore.Mvc;
using MyTeams.Server.Data;


namespace MyTeams.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MatchController : ControllerBase {

    private readonly DataContext _dataContext;

    public MatchController(DataContext dataContext) {
        _dataContext = dataContext;
    }



    //[HttpGet]
    //[Route("[action]")]
    //public async Task<ActionResult<List<DbMatch>>>
    //GetLastMatchDayMatches() {
    //    var lastDay = await _dataContext.GetLastMatchDayDb();
    //    var result = await _dataContext.GetMatchDayMatches(lastDay);
    //    return Ok(result);
    //}

    //[HttpGet]
    //[Route("[action]")]
    //public async Task<ActionResult<DbMatchDay>>
    //GetLastMatchDay() {
    //    var lastDay = await _dataContext.GetLastMatchDayDb();
    //    return Ok(lastDay);
    //}


    //[HttpGet]
    //[Route("[action]")]
    //public async Task<ActionResult<List<DbMatchDay>>>
    //GetMatchDays() {
    //    var result = await _dataContext.GetMatchDaysDb();
    //    return Ok(result);
    //}

    //[HttpGet]
    //[Route("[action]")]
    //public async Task<ActionResult<DbMatch>>
    //GetLastMatch() {
    //    var result = await _dataContext.GetLastMatchDb();
    //    return Ok(result);
    //}

    //[HttpPost]
    //[Route("[action]")]
    //public async Task<ActionResult<DbMatch>>
    //AddMatch(int[] teams) {
    //    var currentMatchDate = _dataContext.GetCurrentMatchDay().Result;
    //    var match = new DbMatch(
    //        id: 0,
    //        matchDayId: currentMatchDate.Id,
    //        aTeamId: teams[0],
    //        bTeamId: teams[1],
    //        aGoals: 0,
    //        bGoals: 0,
    //        isFinished: false);
    //    _dataContext.Matches.Add(match);
    //    await _dataContext.SaveChangesAsync();
    //    return Ok(match);
    //}

    //[HttpPost]
    //[Route("[action]")]
    //public async Task<ActionResult<List<DbMatch>>>
    //DeleteMatch(DbMatch match) {
    //    var currentMatchDate = _dataContext.GetCurrentMatchDay().Result;
    //    var dbMatch = _dataContext.Matches.Find(match.Id);
    //    if (dbMatch == null)
    //        return BadRequest();
    //    _dataContext.Remove(dbMatch);
    //    await _dataContext.SaveChangesAsync();
    //    return Ok((await GetMatches()).Result);
    //}

    //[HttpGet]
    //[Route("[action]")]
    //public async Task<ActionResult<bool>>
    //FinishMatchDay() {
    //    await _dataContext.FinishMatchDayDb();
    //    return Ok(true);
    //}

    //[HttpPost]
    //[Route("[action]")]
    //public async Task<ActionResult<bool>>
    //SetMatchResult(DbMatch dbMatch) {
    //    var result = _dataContext.Matches.Find(dbMatch.Id);
    //    result.AGoals = dbMatch.AGoals;
    //    result.BGoals = dbMatch.BGoals;
    //    await _dataContext.SaveChangesAsync();
    //    return Ok(true);
    //}

    //[HttpPost]
    //[Route("[action]")]
    //public async Task<ActionResult<bool>>
    //ReplaceMatchTeams(DbMatch dbMatch) {
    //    var result = _dataContext.Matches.Find(dbMatch.Id);
    //    result.ATeamId = dbMatch.ATeamId;
    //    result.BTeamId = dbMatch.BTeamId;
    //    await _dataContext.SaveChangesAsync();
    //    return Ok(true);
    //}

    //[HttpPost]
    //[Route("[action]")]
    //public async Task<ActionResult<DbMatchGoal>>
    //SetGoal(DbMatchGoal dbGoal) {
    //    var result = _dataContext.Goals.Find(dbGoal.Id);

    //    if (result == null) {
    //        _dataContext.Goals.Add(dbGoal);
    //        result = dbGoal;
    //    }
    //    else {
    //        result.PlayerGoalId = dbGoal.PlayerGoalId;
    //        result.PlayerPassId = dbGoal.PlayerPassId;
    //    }
    //    await _dataContext.SaveChangesAsync();
    //    return Ok(result);
    //}
}