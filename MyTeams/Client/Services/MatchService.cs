namespace MyTeams.Client.Services;

public interface IMatchService {
    //public List<DbMatch> Matches { get; set; }
    //public List<DbMatchDay> MatchDays { get; set; }

    //public Task GetMatches();
    //public Task<List<DbMatch>> GetMatchDayMatches(DbMatchDay dbMatchDay);
    //public Task<List<DbMatch>> GetLastMatches();

    //public Task<DbMatch?> Add((Team team, Team other) teams);

    //public Task Delete(Match dbMatch);

    //public Task FinishMatchDay();

    //public Task SetMatchResult(DbMatch dbMatch);
    //public Task ReplaceMatchTeams(DbMatch dbMatch);
    //public Task<List<DbMatchDay>?> GetMatchDays();

    //public Task<DbMatchDay> GetLastMatchDay();


    //public Task<DbMatchGoal> SetGoal(DbMatchGoal goal);

    //public Task<List<DbTeam>>
    //GetLastMatchDayTeams();

    //public Task<DatabaseCache>
    //GetDatabaseCache();

}

public class MatchService : IMatchService {
    //private readonly HttpClient _client;

    //public List<DbMatch> Matches { get; set; } = new List<DbMatch>();
    //public List<DbMatch> LastMatches { get; set; } = new List<DbMatch>();
    //public List<DbMatchDay> MatchDays { get; set; } = new List<DbMatchDay>();

    //public MatchService(HttpClient httpClient) {
    //    _client = httpClient;
    //}



    //public async Task
    //GetMatches() {
    //    var res = await _client.SendMessage<GetMatchesMessage>();

    //    if (res != null) {
    //        var resp1 = res.ReadMessage<MatchesMessage>();
    //        Matches = resp1.Matches;
    //    }
    //}

    //public async Task<List<DbMatch>>
    //GetLastMatches() {
    //    if (LastMatches.Count > 0)
    //        return LastMatches;
    //    var res1 = await _client.SendMessage<GetLastMatchMessage>();
    //    var lastMatchesMessage = res1.ReadMessage<MatchesMessage>();

    //    if (lastMatchesMessage != null) {
    //        LastMatches = lastMatchesMessage.Matches;
    //    }
    //    return lastMatchesMessage.Matches;
    //}

    //public async Task<List<DbMatchDay>?>
    //GetMatchDays() {
    //    var result = await _client.SendMessage<GetMatchDaysMessage>();
    //    var matchDaysMessage = result.ReadMessage<MatchDaysMessage>();

    //    if (matchDaysMessage != null)
    //        MatchDays = matchDaysMessage.MatchDays;
    //    return matchDaysMessage.MatchDays;
    //}


    //public async Task<List<DbMatch>>
    //GetMatchDayMatches(DbMatchDay dbMatchDay) {
    //    var result = await _client.SendMessage<GetMatchesMessage>();
    //    var matchDaysMessage = result.ReadMessage<MatchesMessage>();

    //    if (matchDaysMessage == null)
    //        return new List<DbMatch>();

    //    return matchDaysMessage.Matches.Where(x => dbMatchDay.Id == x.MatchDayId).ToList();
    //}


    //public async Task<DbMatch?>
    //Add((Team team, Team other) teams) {

    //    var result = await _client.PostAsJsonAsync("api/message", new AddMatchMessage(new int[2] { teams.team.Id, teams.other.Id }));
    //    var response = await _client.PostAsJsonAsync("api/message", new GetMatchesMessage());

    //    if (response == null) {
    //        throw new NullReferenceException();
    //    }
    //    var res1 = await response.Content.ReadFromJsonAsync<Message>();
    //    var res2 = res1.ReadMessage<MatchMessage>();
    //    return res2.Match;
    //}

    //public async Task
    //Delete(Match Match) {
    //    var result = await _client.PostAsJsonAsync("api/message", new DeleteMatchMessage(Match));
    //    var response = await _client.PostAsJsonAsync("api/message", new GetMatchesMessage());
    //    if (response != null) {
    //        var res1 = await response.Content.ReadFromJsonAsync<Message>();
    //        var res2 = res1.ReadMessage<MatchesMessage>();
    //        Matches = res2.Matches;
    //    }
    //}


    //public async Task
    //FinishMatchDay() {
    //    var result = await _client.SendMessage<FinishMatchMessage>();
    //    if (result == null)
    //        return; 
    //}

    //public async Task
    //SetMatchResult(DbMatch dbMatch) {
    //    var result = await _client.PostAsJsonAsync("api/message", new SetMatchResultMessage(dbMatch));
    //    if (result == null)
    //        return;
    //}

    //public async Task
    //ReplaceMatchTeams(DbMatch dbMatch) {
    //    var result = await _client.PostAsJsonAsync("api/message", new ReplaceMatchTeamsMessage(dbMatch));
    //    if (result == null)
    //        return;
    //}


    //public async Task<DbMatchDay>
    //GetLastMatchDay() {
    //    if (Matches.Count != 0 && MatchDays.Count != 0) {
    //        var lastDays = MatchDays.Where(x => Matches.Any(match => x.Id == match.MatchDayId)).ToList();
    //        if (lastDays.Count > 0)
    //            return lastDays.Last();
    //    }
    //    var result = await _client.SendMessage<GetLastMatchDayMessage>();
    //    var lastMatchDay = result.ReadMessage<MatchDayMessage>();
    //    return lastMatchDay.Match;
    //}

    //public async Task<DbMatchGoal>
    //SetGoal(DbMatchGoal goal) {
    //    var res12 = await _client.PostAsJsonAsync("api/message", new SetGoalMessage(goal));
    //    var resp = await res12.Content.ReadFromJsonAsync<Message>();
    //    if (resp == null) {
    //        throw new NullReferenceException();
    //    }
    //    var resp1 = resp.ReadMessage<SetGoalMessage>();
    //    return resp1.Goal;
    //}

    //public async Task<List<DbTeam>>
    //GetLastMatchDayTeams() {
    //    var res1 = await _client.SendMessage<GetLastMatchDayTeamsMessage>();
    //    if (res1 == null)
    //        return new List<DbTeam>();
    //    var result = res1.ReadMessage<DbTeamsMessage>();
    //    return result.Teams;
    //}

    //public async Task<List<DbTeam>>
    //GetMatchDayTeams() {
    //    var res1 = await _client.SendMessage<GetLastMatchDayMessage>();
    //    if (res1 == null)
    //        return new List<DbTeam>();
    //    var result = res1.ReadMessage<DbTeamsMessage>();
    //    return result.Teams ?? new List<DbTeam>();
    //}

    //public async Task<DatabaseCache>
    //GetDatabaseCache() {
    //    var res = await _client.SendMessage<GetDatabaseCacheMessage>();
    //    var cache1 = res.ReadMessage<DatabaseCacheMessage>();
    //    return cache1.DatabaseCache;
    //}
}