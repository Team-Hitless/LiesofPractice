using LiesOfPractice.Models;
using Newtonsoft.Json;

namespace LiesOfPractice.Converters;


public class HotkeyListConverter : JsonConverter<List<HotKeyActions>>
{
    public override void WriteJson(JsonWriter writer, List<HotKeyActions>? value, JsonSerializer serializer)
    {
        var filtered = value?.Where(h => h.Keys is not null).ToList();
        serializer.Serialize(writer, filtered);
    }

    public override List<HotKeyActions>? ReadJson(JsonReader reader, Type objectType, List<HotKeyActions>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return serializer.Deserialize<List<HotKeyActions>>(reader);
    }
}


