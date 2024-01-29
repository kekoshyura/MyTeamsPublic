using MyTeams.Client.Messages;
using System.Collections.Immutable;

namespace MyTeams.Server.Data;

public static class DatabasePlayerHelper {

    public static async Task<Message>
    AddPlayer(this DataContext dataContext, AddPlayerMessage message) {
        var player = message.Player;
        dataContext.Players.Add(player);
        await dataContext.SaveChangesAsync();
        var result = new PlayerMessage(player).ToMessage();
        return result;
    }

    public static async Task<Message>
    GetFreePlayers(this DataContext dataContext) {
        var matchDayTeams = await dataContext.GetCurrentTeams();
        var takenPlayers = matchDayTeams.Value.SelectMany(x => x.Players).ToList();
        var allPlayers = dataContext.Players.ToImmutableList();
        var result = allPlayers.Where(x => takenPlayers.All(y => y.Id != x.Id)).ToImmutableList(
            );
        var message = new PlayersMessage(result).ToMessage();
        return message;
    }

    public static async Task<Message>
    GetPlayers(this DataContext dataContext) {
        var result = dataContext.Players.ToImmutableList();
        return new PlayersMessage(result).ToMessage();
    }
}