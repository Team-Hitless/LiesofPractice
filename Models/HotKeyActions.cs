using H.Hooks;
using LiesOfPractice.Enums;
using Newtonsoft.Json;


namespace LiesOfPractice.Models;

public class HotKeyActions
{
    public required ActionTag ActionTag { get; set; }
    public Keys? Keys { get; set; } = null;
    [JsonIgnore]
    public Action? Action { get; set; } = null;
}

