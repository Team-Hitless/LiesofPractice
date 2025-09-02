using H.Hooks;
using LiesOfPractice.Enums;
using Newtonsoft.Json;

namespace LiesOfPractice.Models;

public class HotKeyActions
{
    public required Keys Keys { get; set; }
    public required ActionTag ActionTag { get; set; }
    [JsonIgnore]
    public Action? Action { get; set; } = null;
}

