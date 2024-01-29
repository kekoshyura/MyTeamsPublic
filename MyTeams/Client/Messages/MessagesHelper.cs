using MyTeamsCore.Common;
using System.Net.Http.Json;

namespace MyTeams.Client.Messages;

public static class
MessagesHelper {

    public static Message
    ToMessage<T>(this T message) where T : IMessage {
        var json = message.ToJson();
        var typeName = message.GetType().Name;
        return new Message(
            type: typeName,
            content: json);
    }

    public static IMessage
    ParseJson(this Message message) {
        var fullName = $"MyTeams.Client.Messages.{message.Type}";
        var type = Type.GetType(fullName);
        var result = message.Content.ParseJson<IMessage>(type);
        return result;
    }

    public static Task<HttpResponseMessage>
    SendMessage<T>(this HttpClient client, T message) where T : IMessage =>
      client.PostAsJsonAsync("api/message", message.ToMessage());

    public static async Task<Message>
    SendMessage<T>(this HttpClient client) where T : IMessage, new() {
        var message = new T();
        var result = await client.SendMessage(message);
        var messageResponse = await result.Content.ReadFromJsonAsync<Message>();
        if (messageResponse == null)
            throw new InvalidOperationException("Message was null");
        return messageResponse;
    }

    public static async Task<Message>
    SendAndGetMessage<T>(this HttpClient client, T message) where T : IMessage {
        var result = await client.SendMessage(message);
        var messageResponse = await result.Content.ReadFromJsonAsync<Message>();
        if (messageResponse == null)
            throw new InvalidOperationException("Message was null");
        return messageResponse;
    }


    public static T
    ReadMessage<T>(this Message message) => message.ParseJson().VerifyType<T>();


}