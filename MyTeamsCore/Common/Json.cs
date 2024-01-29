using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyTeamsCore.Common;

public static class 
JsonHelper{

    public static JsonSerializerOptions
    DefaultJsonSerializerOptions => new JsonSerializerOptions() {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, 
        IncludeFields = true
    };    

    public static string
    ToJson<T>(this T item) => JsonSerializer.Serialize(item, typeof(T), DefaultJsonSerializerOptions);
    
    public static T
    ParseJson<T>(this string json) => JsonSerializer.Deserialize<T>(json,  DefaultJsonSerializerOptions);

    public static T
    ParseJson<T>(this string json, Type type) {
        var @object = JsonSerializer.Deserialize(json, type);
        if (@object == null)
            throw new InvalidOperationException("@object was null");
        var result = @object.VerifyType<T>();
        return result;
    }

}
