using LiesOfPractice.Interfaces;
using Newtonsoft.Json;
using System.IO;

namespace LiesOfPractice.Services;

public class JsonService : IJsonService
{
    public async void SerializeAsync<T>(T obj, string outputPath)
    {
        await File.WriteAllTextAsync(outputPath, JsonConvert.SerializeObject(obj));
    }

    public async Task<T> DeserializeAsync<T>(string inputPath) where T : new()
    {
        var text = await File.ReadAllTextAsync(inputPath);
        return JsonConvert.DeserializeObject<T>(text) ?? new T();
    }
}
