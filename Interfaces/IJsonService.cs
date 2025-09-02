namespace LiesOfPractice.Interfaces;

public interface IJsonService
{
    public Task<T> DeserializeAsync<T>(string filepath) where T : new();
    public void SerializeAsync<T>(T obj, string outputPath, string filepath);
}