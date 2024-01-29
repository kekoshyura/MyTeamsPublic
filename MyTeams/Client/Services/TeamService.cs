namespace MyTeams.Client.Services;

public interface ITeamService {

    //public List<Team> Teams { get; set; }

    //public Task GetTeams();

    //public Task<Team> Add(TeamInput team);

    //public Task Remove(Team team);

    //public Task Rename(Team team);

}

public class TeamService : ITeamService {
    //private readonly HttpClient _client;

    //public List<Team> Teams { get; set; } = new List<Team>();

    //public TeamService(HttpClient httpClient) {
    //    _client = httpClient;
    //}

    //public async Task
    //GetTeams() {
    //    var res1 = await _client.SendMessage<GetDatabaseTeamsMessage>();
    //    var result1 = res1.ReadMessage<TeamsMessage>();

    //    if (result1 != null)
    //        Teams = result1.Teams;
    //}

    //public async Task<Team>
    //Add(TeamInput team) {
    //    var result = await _client.PostAsJsonAsync("api/message", new AddTeamMessage(team));
    //    var response = await result.Content.ReadFromJsonAsync<Message>();
    //    var newTeam = response.ReadMessage<TeamMessage>();

    //    return newTeam.Team;
    //}

    //public async Task
    //Remove(Team team) {
    //    await _client.PostAsJsonAsync("api/message", new RemoveTeamMessage(team));
    //}

    //public async Task
    //Rename(Team team) {
    //    await _client.PostAsJsonAsync("api/message", new RenameTeamMessage(team));
    //}
}