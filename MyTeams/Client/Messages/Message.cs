using MyTeamsCore;
using System.Collections.Immutable;

namespace MyTeams.Client.Messages;

public class Message {

    public string Type { get; }
    /// <summary>
    /// Json string
    /// </summary>
    public string Content { get; }

    public Message(string type, string content) {
        Type = type;
        Content = content;
    }
}

public interface IMessage { }

public class
OkMessage : IMessage {
    public static OkMessage
    Instance => new OkMessage();
}



public class
AddPlayerMessage : IMessage {
    public Player Player { get; }

    public AddPlayerMessage(Player player) {
        Player = player;
    }
}

public class
GetPlayersMessage : IMessage {
    public static GetPlayersMessage
    Instance => new GetPlayersMessage();
}

public class
PlayersMessage : IMessage {
    public ImmutableList<Player> Players { get; }

    public PlayersMessage(ImmutableList<Player> players) {
        Players = players;
    }
}

public class
PlayerMessage : IMessage {
    public Player Player { get; }
    public PlayerMessage(Player player) {
        Player = player;
    }
}
public class GetFreePlayersMessage : IMessage { }
