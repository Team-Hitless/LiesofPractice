using LiesOfPractice.Interfaces;
using Newtonsoft.Json;
using System.IO;

namespace LiesOfPractice.Services;

public class JsonService : IJsonService
{
    public async void SerializeAsync<T>(T obj, string outputPath, string filepath)
    {
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        await File.WriteAllTextAsync(filepath, JsonConvert.SerializeObject(obj));
    }

    public async Task<T> DeserializeAsync<T>(string filepath) where T : new()
    {
        if (File.Exists(filepath))
        {
            var text = await File.ReadAllTextAsync(filepath);
            return JsonConvert.DeserializeObject<T>(text) ?? new T();
        }
        return new T();
    }
}
