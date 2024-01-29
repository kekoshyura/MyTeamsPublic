namespace MyTeams.Client.Services;

public interface IPlayerService {

    //public List<Player> Players { get; set; }
    //public List<Player> FreePlayers { get; set; }

    //public Task<List<Player>> GetPlayers();
    //public Task<List<Player>?> GetFreePlayers();

    //public Task AddPlayer(Player player);

    //public Task RemoveTeamPlayer(Player player, int teamPlayerId);
    //public Task<TeamPlayer> AddTeamPlayer(Player player, int teamPlayerId);
}

public class PlayerService : IPlayerService {
    //private readonly HttpClient _client;

    //public List<Player> Players { get; set; } = new List<Player>();
    //public List<Player> FreePlayers { get; set; } = new List<Player>();


    //public PlayerService(HttpClient client) {
    //    _client = client;
    //}

    //public async Task<List<Player>>
    //GetPlayers() {
    //    var result = await _client.SendMessage<GetPlayersMessage>();

    //    var playersMessage = result.ReadMessage<PlayersMessage>();
    //    Players = playersMessage.Players;
    //    return Players;
    //}

    //public async Task<List<Player>>
    //GetFreePlayers() {
    //    var res = await _client.SendMessage<GetFreePlayersMessage>();
    //    var freePlayers = res.ReadMessage<PlayersMessage>();
    //    FreePlayers = freePlayers.Players;
    //    return FreePlayers;
    //}

    //public async Task
    //AddPlayer(Player player) {
    //    var result = await _client.PostAsJsonAsync("api/message", new AddPlayerMessage(player));
    //    var response = await _client.PostAsJsonAsync("api/message", new GetPlayersMessage());
    //    if (response != null) {
    //        var res = await response.Content.ReadFromJsonAsync<Message>();
    //        var res1 = res.ReadMessage<PlayersMessage>();
    //        Players = res1.Players;
    //    }
    //}

    //public async Task
    //RemoveTeamPlayer(Player player, int teamPlayerId) {
    //    await _client.PostAsJsonAsync("api/message", new TeamPlayerRemoveMessage(player, teamPlayerId));
    //}

    //public async Task<TeamPlayer>
    //AddTeamPlayer(Player player, int teamPlayerId) {
    //    var result = await _client.PostAsJsonAsync("api/message", new TeamPlayerAddMessage(player, teamPlayerId));
    //    var response = await _client.PostAsJsonAsync("api/message", new GetTeamPlayerMessage(teamPlayerId));
    //    if (response == null) {
    //        throw new NullReferenceException();
    //    }
    //    var res = await response.Content.ReadFromJsonAsync<Message>();
    //    var res1 = res.ReadMessage<TeamPlayerMessage>();
    //    return res1.TeamPlayer;
    //}
}
